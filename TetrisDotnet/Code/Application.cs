using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Tetris;
using TetrisDotnet.Code.Game;
using TetrisDotnet.Code.Game.World;
using TetrisDotnet.Code.UI;
using TetrisDotnet.Code.Utils;

namespace TetrisDotnet.Code
{
	// TODO: Go over and refactor this code.
	class Application
	{
		#region Global vars

		private RenderWindow window;

		Clock clock = new Clock();
		Time gameTime;

		//FPS vars
		float timeElapsed;
		int fps;

		public static int WINDOW_HEIGHT { get; set; }
		public static int WINDOW_WIDTH { get; set; }

		#endregion

		Grid grid;
		Piece activePiece;
		GameState gameState;
		float dropTime;
		Text endText;
		PieceQueue pieceQueue;
		Hold holdManager;

		Sprite[][,] queueSpriteArray4x4;
		Sprite[][,] queueSpriteArray1x4;
		Sprite[][,] queueSpriteArray3x3;


		Sprite[,] holdSprite4x4;
		Sprite[,] holdSprite1x4;
		Sprite[,] holdSprite3x3;

		ScoreText scoreText;
		LevelText levelText;
		RealTimeText realTimeText;
		List<int> idxRowFull = new List<int>();
		StatsTextBlock statsTextBlock;

		public Application(uint windowHeight = 768, uint windowWidth = 1024, string title = "Tetris",
			Styles style = Styles.Close)
		{
			window = new RenderWindow(new VideoMode(windowWidth, windowHeight), title, style);
			fps = 0;

			window.KeyPressed += window_KeyPressed;

			window.Closed += window_Closed;

			window.SetKeyRepeatEnabled(false);
			window.SetMouseCursorVisible(false);

			window.SetIcon(48, 48, IconGenerator.IconToBytes("Art/icon.png"));

			WINDOW_HEIGHT = (int) windowHeight;
			WINDOW_WIDTH = (int) windowWidth;
		}

		public void Run()
		{
			window.SetVisible(true);

			StartNewGame();
			StartNewGame();

			gameState = GameState.Playing;

			while (window.IsOpen)
			{
				//Call the Events
				window.DispatchEvents();

				//Update the game
				Update();

				//Draw the updated app
				Draw();
			}
		}

		#region Helper functions

		private void StartNewGame()
		{
			SetUpGlobalVars();

			clock.Restart();

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		private void Quit()
		{
			window.Close();
		}

		private void DrawActivePieceHardDrop()
		{
			foreach (Vector2i block in activePiece.getGlobalBlocks)
			{
				StaticVars.drawGrid[block.X, Math.Max(0, block.Y - 2)].Texture =
					StaticVars.blockTextures[(int) activePiece.type];
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
				grid.MovePiece(activePiece, StaticVars.flat);

				statsTextBlock.AddToCounter(activePiece.type);
			}
		}

		#region Check if you completed rows

		private void CheckFullRows()
		{
			idxRowFull = grid.GetFullRows();

			scoreText.CountScore(idxRowFull, levelText);

			RemoveRows(idxRowFull);
		}

		void RemoveRows(List<int> fullRows)
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

		#endregion

		#endregion

		/// <summary>
		/// Sets up global vars to the program
		/// </summary>
		void SetUpGlobalVars()
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

			#region End text

			endText = new Text("You have lost\npress space\nto reset\nthe game", StaticVars.font);

			FloatRect textRect = endText.GetLocalBounds();
			endText.Origin = new Vector2f(textRect.Left + textRect.Width / 2.0f, textRect.Top + textRect.Height / 2.0f);
			endText.Position = new Vector2f(WINDOW_WIDTH / 2f, WINDOW_HEIGHT / 2f);

			#endregion

			FloatRect textRectPause = StaticVars.pauseText.GetLocalBounds();
			StaticVars.pauseText.Origin = new Vector2f(textRectPause.Left + textRectPause.Width / 2.0f,
				textRectPause.Top + textRectPause.Height / 2.0f);
			StaticVars.pauseText.Position = new Vector2f(WINDOW_WIDTH / 2f, WINDOW_HEIGHT / 2f);

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

			#region Queue setup

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

			#endregion

			#region Hold queue

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

			#endregion

			#region Text

			scoreText = new ScoreText();

			levelText = new LevelText();

			realTimeText = new RealTimeText();

			#endregion

			StaticVars.statsSprite.Position = new Vector2f(StaticVars.holdSprite.Position.X,
				realTimeText.Position.Y + StaticVars.imageSize.Y);

			StaticVars.controlsText.Position = new Vector2f(StaticVars.queueSprite.Position.X,
				StaticVars.queueSprite.Position.Y + StaticVars.queueTexture.Size.Y + StaticVars.imageSize.Y);
		}

		#region Input functions

		// (C)
		/// <summary>
		/// Appeler chaque fois qu’une touche est appuyée.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e">The key event</param>
		void window_KeyPressed(object sender, KeyEventArgs e)
		{
			if (gameState == GameState.Playing)
			{
				switch (e.Code)
				{
					// Debug function to level up automagically.
					case Keyboard.Key.L:
						levelText.LevelUp();
						break;

					// Rotate active piece clockwise.
					case Keyboard.Key.Up:
					case Keyboard.Key.W:
						if (grid.RotatePiece(activePiece, Rotation.Clockwise))
						{
							dropTime -= levelText.sideMoveSpeed;
						}

						break;

					// Rotate active piece counter clockwise.
					case Keyboard.Key.E:
						if (grid.RotatePiece(activePiece, Rotation.CounterClockwise))
						{
							dropTime -= levelText.sideMoveSpeed;
						}

						break;

					// Move active piece left.
					case Keyboard.Key.Left:
					case Keyboard.Key.A:
						if (!grid.CanPlacePiece(activePiece, StaticVars.down))
						{
							dropTime -= levelText.sideMoveSpeed;
						}

						grid.MovePiece(activePiece, StaticVars.left);

						break;

					// Move active piece down.
					case Keyboard.Key.Down:
					case Keyboard.Key.S:
						if (grid.CanPlacePiece(activePiece, StaticVars.down))
						{
							grid.MovePiece(activePiece, StaticVars.down);
							scoreText.AddScore(1);
						}
						else
						{
							dropTime = 1;
						}

						break;

					// Move active piece right.
					case Keyboard.Key.Right:
					case Keyboard.Key.D:
						if (!grid.CanPlacePiece(activePiece, StaticVars.down))
						{
							dropTime -= levelText.sideMoveSpeed;
						}

						grid.MovePiece(activePiece, StaticVars.right);
						break;

					// Holds the active piece (if a held piece exists, it will be selected as the new active piece).
					case Keyboard.Key.LShift:
					case Keyboard.Key.C:
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

						break;

					// Hard drop active piece.
					case Keyboard.Key.Space:
						int spacesMoved = grid.DetermineDropdownPosition(activePiece);
						scoreText.AddScore(2 * spacesMoved);

						grid.MovePiece(activePiece, StaticVars.down * (spacesMoved));

						dropTime = levelText.dropSpeed;

						DrawActivePieceHardDrop();
						break;

					// Opens the menu.
					case Keyboard.Key.Escape:
//						soundplayer.Stop();
						Quit();
						break;

					// Pause game.
					case Keyboard.Key.P:
						gameState = GameState.Pause;
						break;
				}
			}
			else if (gameState == GameState.Pause)
			{
				switch (e.Code)
				{
					case Keyboard.Key.Escape:
					case Keyboard.Key.P:
						gameState = GameState.Playing;
						break;
				}
			}
		}

		void window_Closed(object sender, EventArgs e)
		{
			Quit();
		}

		#endregion

		private void Update()
		{
			gameTime = clock.Restart();

			#region FPS stuff, no touchy

			timeElapsed += gameTime.AsSeconds();

			//If 1 second has passed in the game
			if (timeElapsed > 1)
			{
				realTimeText.UpdateTimeString();

				Console.WriteLine("FPS: {0}", fps);

				fps = 0;
				timeElapsed = 0;
			}

			fps++;

			#endregion

			if (gameState == GameState.Playing)
			{
				dropTime += gameTime.AsSeconds();

				realTimeText.realTime += gameTime.AsSeconds();

				if (dropTime > levelText.dropSpeed)
				{
					dropTime = 0;

					if (grid.CanPlacePiece(activePiece, StaticVars.down))
					{
						grid.MovePiece(activePiece, StaticVars.down);
					}
					else
					{
						grid.KillPiece(activePiece);

						CheckFullRows();

						NewPiece();
					}
				}
			}
		}

		private void Draw()
		{
			window.Clear();

			//TODO: make this change the textures of the sprites that has moved
			if (gameState == GameState.Playing || gameState == GameState.Pause)
			{
				//Draw the background first, so it's in the back
				window.Draw(StaticVars.backDrop);
				window.Draw(StaticVars.holdSprite);
				window.Draw(StaticVars.queueSprite);
				window.Draw(StaticVars.drawGridSprite);

				#region Draw main grid

				//Get a temp grid to stop calling a function in a class, more FPS
				PieceType[,] drawArray = grid.GetDrawable();

				//Loop through the drawable grid elements
				for (int x = 0; x < drawArray.GetLength(0); x++)
				{
					for (int y = 0; y < drawArray.GetLength(1); y++)
					{
						//Slower code?
						/*
						switch (drawArray[i, j])
						{

						    case PieceType.Dead:
						        //drawGrid[i, j].Texture = Color.White;
						        break;

						    default:
						        drawGrid[i, j].Texture = blockTextures[(int)drawArray[i, j]];
						        break;

						}
						*/

						//Update the textures except dead pieces, since we want them to keep their original colors
						if (drawArray[x, y] != PieceType.Dead)
						{
							//Associated the blockTextures with the same indexes as the Enum
							//Look at both and you will understand
							StaticVars.drawGrid[x, y].Texture = StaticVars.blockTextures[(int) drawArray[x, y]];
						}

						//Finally draw to the screen the final results
						window.Draw(StaticVars.drawGrid[x, y]);
					}
				}

				#endregion

				//Draw the queue

				#region Queue

				List<PieceType> queueArray = pieceQueue.Get().ToList();

//				for (int i = 0; i < queueArray.Count; i++)
//				{
//					List<PieceType> blocks = queueArray[i].
//					PieceType[,] queuePieceToDraw = StaticVars.GetPieceArray(queueArray[i]);
//
//					if (queueArray[i] == PieceType.I)
//					{
//						for (int j = 0; j < queueSpriteArray1x4[0].GetLength(0); j++)
//						{
//							for (int k = 0; k < queuePieceToDraw.GetLength(1); k++)
//							{
//								if (queuePieceToDraw[j + 1, k] != PieceType.Empty)
//								{
//									queueSpriteArray1x4[i][j, k].Texture =
//										StaticVars.blockTextures[(int) queuePieceToDraw[j + 1, k]];
//
//									window.Draw(queueSpriteArray1x4[i][j, k]);
//								}
//							}
//						}
//					}
//					else if (queuePieceToDraw.GetLength(1) == 4)
//					{
//						for (int j = 0; j < queuePieceToDraw.GetLength(0); j++)
//						{
//							for (int k = 0; k < queuePieceToDraw.GetLength(1); k++)
//							{
//								if (queuePieceToDraw[j, k] != PieceType.Empty)
//								{
//									queueSpriteArray4x4[i][j, k].Texture =
//										StaticVars.blockTextures[(int) queuePieceToDraw[j, k]];
//
//									window.Draw(queueSpriteArray4x4[i][j, k]);
//								}
//							}
//						}
//					}
//					else
//					{
//						for (int j = 0; j < queuePieceToDraw.GetLength(0); j++)
//						{
//							for (int k = 0; k < queuePieceToDraw.GetLength(1); k++)
//							{
//								if (queuePieceToDraw[j, k] != PieceType.Empty)
//								{
//									queueSpriteArray3x3[i][j, k].Texture =
//										StaticVars.blockTextures[(int) queuePieceToDraw[j, k]];
//									window.Draw(queueSpriteArray3x3[i][j, k]);
//								}
//							}
//						}
//					}
//				}

				#endregion

				#region Hold

//				PieceType[,] pieceToDraw = StaticVars.GetPieceArray(holdManager.currentPiece);
//
//				if (holdManager.currentPiece == PieceType.I)
//				{
//					for (int j = 0; j < holdSprite1x4.GetLength(0); j++)
//					{
//						for (int k = 0; k < pieceToDraw.GetLength(1); k++)
//						{
//							if (pieceToDraw[j + 1, k] != PieceType.Empty)
//							{
//								holdSprite1x4[j, k].Texture = StaticVars.blockTextures[(int) pieceToDraw[j + 1, k]];
//
//								window.Draw(holdSprite1x4[j, k]);
//							}
//						}
//					}
//				}
//				else if (pieceToDraw.GetLength(1) == 4)
//				{
//					for (int j = 0; j < pieceToDraw.GetLength(0); j++)
//					{
//						for (int k = 0; k < pieceToDraw.GetLength(1); k++)
//						{
//							if (pieceToDraw[j, k] != PieceType.Empty)
//							{
//								holdSprite4x4[j, k].Texture = StaticVars.blockTextures[(int) pieceToDraw[j, k]];
//
//								window.Draw(holdSprite4x4[j, k]);
//							}
//						}
//					}
//				}
//				else
//				{
//					for (int j = 0; j < pieceToDraw.GetLength(0); j++)
//					{
//						for (int k = 0; k < pieceToDraw.GetLength(1); k++)
//						{
//							if (pieceToDraw[j, k] != PieceType.Empty)
//							{
//								holdSprite3x3[j, k].Texture = StaticVars.blockTextures[(int) pieceToDraw[j, k]];
//								window.Draw(holdSprite3x3[j, k]);
//							}
//						}
//					}
//				}

				#endregion

				window.Draw(scoreText);

				window.Draw(levelText);

				window.Draw(realTimeText);

				window.Draw(StaticVars.statsSprite);

				window.Draw(StaticVars.controlsText);

				foreach (var item in statsTextBlock.GetDrawable())
				{
					window.Draw(item);
				}

				if (gameState == GameState.Pause)
				{
					window.Draw(StaticVars.pauseText);
				}
			}

			window.Display();
		}
	}
}