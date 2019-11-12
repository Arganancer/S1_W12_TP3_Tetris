using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
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
		// Logic Elements
		private Grid grid = new Grid();
		private PieceQueue pieceQueue = new PieceQueue();
		private Piece activePiece;
		private Hold holdManager = new Hold();
		private SceneType nextScene;
		private List<int> idxRowFull = new List<int>();
		private GameState gameState;
		private float dropTime;

		// UI Elements
		private readonly GridUI gridUi = new GridUI();
		private ScoreText scoreText;
		private LevelText levelText;
		private RealTimeText realTimeText;
		private StatsTextBlock statsTextBlock;
		private readonly ControlsText controlsText;
		private readonly PauseText pauseText = new PauseText();

		~GameScene()
		{
			Main.eventSystem.Unsubscribe(EventType.InputRotateClockwise, OnInputRotateClockwise);
			Main.eventSystem.Unsubscribe(EventType.InputRotateCounterClockwise, OnInputRotateCounterClockwise);
			Main.eventSystem.Unsubscribe(EventType.InputDown, OnInputDown);
			Main.eventSystem.Unsubscribe(EventType.InputLeft, OnInputLeft);
			Main.eventSystem.Unsubscribe(EventType.InputRight, OnInputRight);
			Main.eventSystem.Unsubscribe(EventType.InputHold, OnInputHold);
			Main.eventSystem.Unsubscribe(EventType.InputHardDrop, OnInputHardDrop);
			Main.eventSystem.Unsubscribe(EventType.InputPause, OnInputPause);
		}

		public GameScene() : base(SceneType.Game)
		{
			gameState = GameState.Playing;
			nextScene = SceneType;

			Main.eventSystem.Subscribe(EventType.InputRotateClockwise, OnInputRotateClockwise);
			Main.eventSystem.Subscribe(EventType.InputRotateCounterClockwise, OnInputRotateCounterClockwise);
			Main.eventSystem.Subscribe(EventType.InputDown, OnInputDown);
			Main.eventSystem.Subscribe(EventType.InputLeft, OnInputLeft);
			Main.eventSystem.Subscribe(EventType.InputRight, OnInputRight);
			Main.eventSystem.Subscribe(EventType.InputHold, OnInputHold);
			Main.eventSystem.Subscribe(EventType.InputHardDrop, OnInputHardDrop);
			Main.eventSystem.Subscribe(EventType.InputPause, OnInputPause);
			
			scoreText = new ScoreText();
			levelText = new LevelText();
			realTimeText = new RealTimeText();
			statsTextBlock = new StatsTextBlock();
			controlsText = new ControlsText();
			
			StartNewGame();
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
			if (gameState == GameState.Playing || gameState == GameState.Pause)
			{
				window.Draw(AssetPool.backDrop);
				window.Draw(AssetPool.holdSprite);
				window.Draw(AssetPool.queueSprite);
				window.Draw(AssetPool.drawGridSprite);

				// TODO: This whole bit of code should go.
				for (int x = 0; x < Grid.GridWidth; x++)
				{
					for (int y = 0; y < Grid.GridHeight - 2; y++)
					{
						//Update the textures except dead pieces, since we want them to keep their original colors
						PieceType block = grid.GetBlock(x, y + 2);
						if (block != PieceType.Dead)
						{
							//Associated the blockTextures with the same indexes as the Enum
							//Look at both and you will understand
							gridUi.DrawableGrid[x, y].Texture = AssetPool.blockTextures[(int) block];
						}

						//Finally draw to the screen the final results
						window.Draw(gridUi.DrawableGrid[x, y]);
					}
				}

				window.Draw(scoreText);
				window.Draw(levelText);
				window.Draw(realTimeText);
				window.Draw(controlsText);
				window.Draw(AssetPool.statsSprite);
				
				foreach (Text statTextBlock in statsTextBlock.GetDrawable())
				{
					window.Draw(statTextBlock);
				}

				if (gameState == GameState.Pause)
				{
					window.Draw(pauseText);
				}
			}
		}

		private void OnInputRotateClockwise()
		{
			if (grid.RotatePiece(activePiece, Rotation.Clockwise))
			{
				dropTime -= levelText.sideMoveSpeed;
			}
		}

		private void OnInputRotateCounterClockwise()
		{
			if (grid.RotatePiece(activePiece, Rotation.CounterClockwise))
			{
				dropTime -= levelText.sideMoveSpeed;
			}
		}

		private void OnInputDown()
		{
			if (grid.CanPlacePiece(activePiece, Vector2iUtils.down))
			{
				grid.MovePiece(activePiece, Vector2iUtils.down);
				scoreText.AddScore(1);
			}
			else
			{
				dropTime = 1;
			}
		}

		private void OnInputLeft()
		{
			if (!grid.CanPlacePiece(activePiece, Vector2iUtils.down))
			{
				dropTime -= levelText.sideMoveSpeed;
			}

			grid.MovePiece(activePiece, Vector2iUtils.left);
		}

		private void OnInputRight()
		{
			if (!grid.CanPlacePiece(activePiece, Vector2iUtils.down))
			{
				dropTime -= levelText.sideMoveSpeed;
			}

			grid.MovePiece(activePiece, Vector2iUtils.right);
		}

		private void OnInputHold()
		{
			if (holdManager.canSwap)
			{
				PieceType oldPiece = activePiece.type;

				grid.RemovePiece(activePiece);

				if (holdManager.currentPiece == PieceType.Empty)
				{
					NewPiece();
				}
				else
				{
					NewPiece(holdManager.currentPiece);
				}

				holdManager.currentPiece = oldPiece;
				holdManager.canSwap = false;
			}
		}

		private void OnInputHardDrop()
		{
			int spacesMoved = grid.DetermineDropdownPosition(activePiece);
			scoreText.AddScore(2 * spacesMoved);

			grid.MovePiece(activePiece, Vector2iUtils.down * (spacesMoved));

			dropTime = levelText.dropSpeed;

			DrawActivePieceHardDrop();
		}

		private void OnInputPause()
		{
			gameState = GameState.Pause;
		}

		private void InitializeGame()
		{
			//Select a new active piece
			NewPiece();

			//Add the piece to the grid, with a movement of 0,0
			grid.AddPiece(activePiece, new Vector2i(0, 0));

			AssetPool.drawGridSprite.Position = new Vector2f(gridUi.position.X - AssetPool.blockSize.X * 1.5f,
				gridUi.position.Y - AssetPool.blockSize.Y * 2f);

			scoreText = new ScoreText();

			levelText = new LevelText();

			realTimeText = new RealTimeText();

			AssetPool.statsSprite.Position = new Vector2f(AssetPool.holdSprite.Position.X,
				realTimeText.Position.Y + AssetPool.blockSize.Y);

			controlsText.Position = new Vector2f(AssetPool.queueSprite.Position.X,
				AssetPool.queueSprite.Position.Y + AssetPool.queueTexture.Size.Y + AssetPool.blockSize.Y);
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
				gridUi.DrawableGrid[block.X, Math.Max(0, block.Y - 2)].Texture =
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
					for (int x = 0; x < Grid.GridWidth; x++)
					{
						//Only move "Dead" textures since the rest gets sorted out
						if (drawableGrid[x, y] == PieceType.Dead)
						{
							gridUi.DrawableGrid[x, y].Texture = gridUi.DrawableGrid[x, y - 1].Texture;
						}
					}
				}
			}
		}
	}
}