using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Game;
using TetrisDotnet.Code.Game.World;
using TetrisDotnet.Code.UI;
using TetrisDotnet.Code.UI.Elements;
using TetrisDotnet.Code.Utils;
using TetrisDotnet.Code.Utils.Enums;

namespace TetrisDotnet.Code.Scenes
{
	public class GameScene : Scene
	{
		private Grid grid;
		private Piece activePiece;
		private GameState gameState;
		private float dropTime;
		private PieceQueue pieceQueue;
		private Hold holdManager;

		private ScoreText scoreText;
		private LevelText levelText;
		private RealTimeText realTimeText;
		private List<int> idxRowFull = new List<int>();
		private StatsTextBlock statsTextBlock;

		private SceneType nextScene;

		public GameScene() : base(SceneType.Game)
		{
			nextScene = SceneType;
			gameState = GameState.Playing;
		}

		public override void Resume()
		{
			nextScene = SceneType;
		}

		public override SceneType Update(float deltaTime)
		{
			dropTime += deltaTime;

			realTimeText.realTime += deltaTime;

			if (dropTime > levelText.dropSpeed)
			{
				dropTime = 0;

				if (grid.CanPlacePiece(activePiece, Vector2iUtils.down))
				{
					grid.MovePiece(activePiece, Vector2iUtils.down);
				}
				else
				{
					grid.KillPiece(activePiece);

					CheckFullRows();

					NewPiece();
				}
			}

			return nextScene;
		}

		public override void Draw(RenderWindow window)
		{
			//TODO: make this change the textures of the sprites that has moved
			if (gameState == GameState.Playing || gameState == GameState.Pause)
			{
				//Draw the background first, so it's in the back
				window.Draw(AssetPool.backDrop);
				window.Draw(AssetPool.holdSprite);
				window.Draw(AssetPool.queueSprite);
				window.Draw(AssetPool.drawGridSprite);

				//Get a temp grid to stop calling a function in a class, more FPS
				PieceType[,] drawArray = grid.GetDrawable();

				//Loop through the drawable grid elements
				for (int x = 0; x < drawArray.GetLength(0); x++)
				{
					for (int y = 0; y < drawArray.GetLength(1); y++)
					{
						//Update the textures except dead pieces, since we want them to keep their original colors
						if (drawArray[x, y] != PieceType.Dead)
						{
							//Associated the blockTextures with the same indexes as the Enum
							//Look at both and you will understand
							AssetPool.drawGrid[x, y].Texture = AssetPool.blockTextures[(int) drawArray[x, y]];
						}

						//Finally draw to the screen the final results
						window.Draw(AssetPool.drawGrid[x, y]);
					}
				}

				window.Draw(scoreText);

				window.Draw(levelText);

				window.Draw(realTimeText);

				window.Draw(AssetPool.statsSprite);

				window.Draw(AssetPool.controlsText);

				foreach (var item in statsTextBlock.GetDrawable())
				{
					window.Draw(item);
				}

				if (gameState == GameState.Pause)
				{
					window.Draw(AssetPool.pauseText);
				}
			}
		}

		private void InitializeGame()
		{
						//Create the BACKEND grid for the game
			grid = new Grid();

			holdManager = new Hold();

			for (int x = 0; x < StaticVars.drawGrid.GetLength(0); x++)
			{
				for (int y = 0; y < StaticVars.drawGrid.GetLength(1); y++)
				{
					StaticVars.drawGrid[x, y] = new Sprite
					{
						Position = new Vector2f(x * StaticVars.imageSize.X + StaticVars.gridXPos,
							y * StaticVars.imageSize.Y + StaticVars.gridYPos + StaticVars.imageSize.Y)
					};

					//Place each block of the grid at the corresponding position of the array
				}
			}

			endText = new Text("You have lost\npress space\nto reset\nthe game", AssetPool.font);

			FloatRect textRect = endText.GetLocalBounds();
			endText.Origin = new Vector2f(textRect.Left + textRect.Width / 2.0f, textRect.Top + textRect.Height / 2.0f);
			endText.Position = new Vector2f(WindowWidth / 2f, WindowHeight / 2f);

			FloatRect textRectPause = StaticVars.pauseText.GetLocalBounds();
			StaticVars.pauseText.Origin = new Vector2f(textRectPause.Left + textRectPause.Width / 2.0f,
				textRectPause.Top + textRectPause.Height / 2.0f);
			StaticVars.pauseText.Position = new Vector2f(WindowWidth / 2f, WindowHeight / 2f);

			//Make it so the game is playing
			gameState = GameState.Playing;

			pieceQueue = new PieceQueue();

			statsTextBlock = new StatsTextBlock();

			//Select a new active piece
			NewPiece();

			//Add the piece to the grid, with a movement of 0,0
			grid.AddPiece(activePiece, new Vector2i(0, 0));

			StaticVars.queueSprite.Position =
				new Vector2f(StaticVars.gridXPos + StaticVars.imageSize.X * (StaticVars.drawGrid.GetLength(1) + 2.25f),
					StaticVars.gridYPos);
			StaticVars.holdSprite.Position =
				new Vector2f(StaticVars.gridXPos - StaticVars.holdTexture.Size.X - (StaticVars.imageSize.X * 2.25f),
					StaticVars.gridYPos);
			StaticVars.drawGridSprite.Position = new Vector2f(StaticVars.gridXPos - StaticVars.imageSize.X * 1.5f,
				StaticVars.gridYPos - StaticVars.imageSize.Y * 2f);

			queueSpriteArray4x4 = new[]
			{
				new Sprite[4, 4],
				new Sprite[4, 4],
				new Sprite[4, 4]
			};

			for (int i = 0; i < queueSpriteArray4x4.Length; i++)
			{
				for (int j = 0; j < queueSpriteArray4x4[0].GetLength(0); j++)
				{
					for (int k = 0; k < queueSpriteArray4x4[0].GetLength(1); k++)
					{
						queueSpriteArray4x4[i][j, k] = new Sprite();

						queueSpriteArray4x4[i][j, k].Position = new Vector2f(
							StaticVars.queueSprite.Position.X + StaticVars.imageSize.X * 1.5f +
							StaticVars.imageSize.X * k,
							StaticVars.queueSprite.Position.Y + StaticVars.imageSize.Y * 2.5f +
							StaticVars.imageSize.Y * j +
							StaticVars.imageSize.Y * (queueSpriteArray4x4[0].GetLength(0) - 1) * i -
							StaticVars.imageSize.Y);
					}
				}
			}

			queueSpriteArray1x4 = new[]
			{
				new Sprite[1, 4],
				new Sprite[1, 4],
				new Sprite[1, 4]
			};

			for (int i = 0; i < queueSpriteArray1x4.Length; i++)
			{
				for (int j = 0; j < queueSpriteArray1x4[0].GetLength(0); j++)
				{
					for (int k = 0; k < queueSpriteArray1x4[0].GetLength(1); k++)
					{
						queueSpriteArray1x4[i][j, k] = new Sprite();

						queueSpriteArray1x4[i][j, k].Position = new Vector2f(
							StaticVars.queueSprite.Position.X + StaticVars.imageSize.X * 1.5f +
							StaticVars.imageSize.X * k,
							StaticVars.queueSprite.Position.Y + StaticVars.imageSize.Y * 2.5f +
							StaticVars.imageSize.Y * j +
							StaticVars.imageSize.Y * (queueSpriteArray4x4[0].GetLength(0) - 1) * i +
							StaticVars.imageSize.Y * 0.5f);
					}
				}
			}


			queueSpriteArray3x3 = new[]
			{
				new Sprite[3, 3],
				new Sprite[3, 3],
				new Sprite[3, 3]
			};

			for (int i = 0; i < queueSpriteArray3x3.Length; i++)
			{
				for (int j = 0; j < queueSpriteArray3x3[0].GetLength(0); j++)
				{
					for (int k = 0; k < queueSpriteArray3x3[0].GetLength(1); k++)
					{
						queueSpriteArray3x3[i][j, k] = new Sprite();

						queueSpriteArray3x3[i][j, k].Position = new Vector2f(
							StaticVars.queueSprite.Position.X + StaticVars.imageSize.X * 1.5f +
							StaticVars.imageSize.X * k + StaticVars.imageSize.X * 0.5f,
							StaticVars.queueSprite.Position.Y + StaticVars.imageSize.Y * 2.5f +
							StaticVars.imageSize.Y * j +
							StaticVars.imageSize.Y * queueSpriteArray3x3[0].GetLength(0) * i);
					}
				}
			}

			holdSprite3x3 = new Sprite[3, 3];

			for (int i = 0; i < holdSprite3x3.GetLength(0); i++)
			{
				for (int j = 0; j < holdSprite3x3.GetLength(1); j++)
				{
					holdSprite3x3[i, j] = new Sprite();

					holdSprite3x3[i, j].Position = new Vector2f(
						StaticVars.holdSprite.Position.X + StaticVars.imageSize.X * 1.5f + StaticVars.imageSize.X * j +
						StaticVars.imageSize.X * 0.5f,
						StaticVars.holdSprite.Position.Y + StaticVars.imageSize.Y * 3.5f + StaticVars.imageSize.Y * i);
				}
			}

			holdSprite4x4 = new Sprite[4, 4];

			for (int i = 0; i < holdSprite4x4.GetLength(0); i++)
			{
				for (int j = 0; j < holdSprite4x4.GetLength(1); j++)
				{
					holdSprite4x4[i, j] = new Sprite();

					holdSprite4x4[i, j].Position = new Vector2f(
						StaticVars.holdSprite.Position.X + StaticVars.imageSize.X * 1.5f + StaticVars.imageSize.X * j,
						StaticVars.holdSprite.Position.Y + StaticVars.imageSize.Y * 2.5f + StaticVars.imageSize.Y * i);
				}
			}

			holdSprite1x4 = new Sprite[1, 4];

			for (int i = 0; i < holdSprite1x4.GetLength(0); i++)
			{
				for (int j = 0; j < holdSprite1x4.GetLength(1); j++)
				{
					holdSprite1x4[i, j] = new Sprite();

					holdSprite1x4[i, j].Position = new Vector2f(
						StaticVars.holdSprite.Position.X + StaticVars.imageSize.X * 1.5f + StaticVars.imageSize.X * j,
						StaticVars.holdSprite.Position.Y + StaticVars.imageSize.Y * 2.5f + StaticVars.imageSize.Y * i +
						StaticVars.imageSize.Y * 1.5f);
				}
			}

			scoreText = new ScoreText();

			levelText = new LevelText();

			realTimeText = new RealTimeText();

			StaticVars.statsSprite.Position = new Vector2f(StaticVars.holdSprite.Position.X,
				realTimeText.Position.Y + StaticVars.imageSize.Y);

			StaticVars.controlsText.Position = new Vector2f(StaticVars.queueSprite.Position.X,
				StaticVars.queueSprite.Position.Y + StaticVars.queueTexture.Size.Y + StaticVars.imageSize.Y);
		}

		private void StartNewGame()
		{
			InitializeGame();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		
		private void DrawActivePieceHardDrop()
		{
			foreach (Vector2i block in activePiece.getGlobalBlocks)
			{
				StaticVars.drawGrid[block.X, Math.Max(0, block.Y - 2)].Texture =
					AssetPool.blockTextures[(int) activePiece.type];
			}
		}

		private void NewPiece(PieceType type = PieceType.Empty)
		{
			holdManager.canSwap = true;

			activePiece = type == PieceType.Empty ? new Piece(pieceQueue.GrabNext()) : new Piece(type);

			if (grid.CheckLose())
			{
				StartNewGame();
			}
			else
			{
				grid.MovePiece(activePiece, Vector2iUtils.flat);

				statsTextBlock.AddToCounter(activePiece.type);
			}
		}
		
		private void CheckFullRows()
		{
			idxRowFull = grid.GetFullRows();

			scoreText.CountScore(idxRowFull, levelText);

			RemoveRows(idxRowFull);
		}

		private void RemoveRows(List<int> fullRows)
		{
			foreach (int rowIdx in fullRows)
			{
				grid.RemoveRow(rowIdx);

				PieceType[,] drawableGrid = grid.GetDrawable();

				for (int y = rowIdx - 2; y > 0; y--)
				{
					for (int x = 0; x < StaticVars.drawGrid.GetLength(0); x++)
					{
						//Only move "Dead" textures since the rest gets sorted out
						if (drawableGrid[x, y] == PieceType.Dead)
						{
							StaticVars.drawGrid[x, y].Texture = StaticVars.drawGrid[x, y - 1].Texture;
						}
					}
				}
			}
		}
	}
}