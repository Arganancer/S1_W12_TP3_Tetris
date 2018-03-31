using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Diagnostics;
using System.Media;

namespace Tetris
{
    // TETRIS
	class Application
	{

		#region Global vars

		//The window of application
		private RenderWindow window;

		//Used to time the movement of the objects on screen
		Clock clock = new Clock();
		Time gameTime = new Time();

		//FPS vars
		float timeElapsed = 0;
		int fps = 0;

		public static int WINDOW_HEIGHT { get; set; }
		public static int WINDOW_WIDTH { get; set; }

		#endregion

		//The main grid of the game
		Grid grid;

		//The current piece the player is controlling
		Piece activePiece;

		//The current state of the game
		GameState gameState;

		float dropTime = 0;

		Text endText;

		PieceQueue pieceQueue;

		Sprite[][,] queueSpriteArray4x4;
		Sprite[][,] queueSpriteArray1x4;
		Sprite[][,] queueSpriteArray3x3;

		Hold holdManager;

		Sprite[,] holdSprite4x4;
		Sprite[,] holdSprite1x4;
		Sprite[,] holdSprite3x3;

		Score score;

		Level level;

		RealTime realTime;

		Menu menu;

		Options optionsMenu;

		List<int> idxRowFull = new List<int>();

		SoundPlayer soundplayer;
		bool musicOff = true;
		string selectedSong = "Music/korobeiniki.wav";

		Stats stats;

		bool teacher;

        // (C)
        /// <summary>
        /// Constructeur de la fenêtre du jeu.
        /// </summary>
        /// 
        /// <param name="windowHeight">
        /// La hauteur de la fenêtre en pixel.
        /// </param>
        /// 
        /// <param name="windowWidth">
        /// La largeur de la fenêtre en pixel.
        /// </param>
        /// 
        /// <param name="title">
        /// Titre de la fenêtre.
        /// </param>
        /// 
        /// <param name="style">
        /// Style de la fenêtre.
        /// </param>
        public Application(uint windowHeight = 768, uint windowWidth = 1024, string title = "Tetris", Styles style = Styles.Close)
		{

			window = new RenderWindow(new VideoMode(windowWidth, windowHeight), title, style);

			//Add the keypressed function to the window
			window.KeyPressed += window_KeyPressed;

			//Add the Closed function to the window
			window.Closed += window_Closed;

			window.SetKeyRepeatEnabled(false);
			window.SetMouseCursorVisible(false);

			window.SetIcon(48, 48, IconGenerator.IconToBytes("Art/icon.png"));

			//Inits window height and width global vars
			WINDOW_HEIGHT = (int)windowHeight;
			WINDOW_WIDTH = (int)windowWidth;

		}

		/// <summary>
		/// Main loop of the program
		/// </summary>
		public void Run()
		{

			window.SetVisible(true);

			//Made this function so you can "reset" the game easily
			//Pierre please dont make us lose points because I didn't init my variables at the start
			//some of them wont even init correctly
			//I want my C++ .h file 
			//>.<

			SetUpGlobalVars();

			gameState = GameState.Menu;

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

		void StartNewGame()
		{

			SetUpGlobalVars();

			clock.Restart();

			GC.Collect();
			GC.WaitForPendingFinalizers();

			if (!musicOff)
			{

				soundplayer.PlayLooping();

			}

		}

		void Quit()
		{

			window.Close();

		}

		void DrawActivePieceHardDrop()
		{

			PieceType[,] pieceArray = activePiece.GetPieceArray();

			for (int i = 0; i < pieceArray.GetLength(0); i++)
			{

				for (int j = 0; j < pieceArray.GetLength(1); j++)
				{

					if(pieceArray[i,j] == activePiece.type)
					{

						StaticVars.drawGrid[Math.Max(0,activePiece.position.Y + i - 2), activePiece.position.X + j].Texture = StaticVars.blockTextures[(int)activePiece.type];

					}
					 
				}

			}

		}

		void NewPiece(PieceType type = PieceType.Empty)
		{

			holdManager.canSwap = true;

			if(type == PieceType.Empty)
			{

				activePiece = new Piece(pieceQueue.GrabNext());
				//activePiece = new Piece(PieceType.I);

			}
			else
			{

				activePiece = new Piece(type);

			}

			//if (!grid.CanPlacePiece(activePiece, down) || grid.CheckLose())
			if(grid.CheckLose())
			{

				//I kinda know WHY I did this, but I don't need to
				//grid.KillPiece(activePiece);

				//if (grid.CheckLose())
				//{

				gameState = GameState.End;

				//}

			}
			else
			{

				grid.MovePiece(activePiece, StaticVars.flat);

				stats.AddToCounter(activePiece.type);

			}

		}

		#region Check if you completed rows

		void CheckFullRows()
		{

			idxRowFull = grid.CheckFullRows();

			score.CountScore(idxRowFull, level);

			RemoveRows(idxRowFull);

		}
		
		void RemoveRows(List<int> fullRows)
		{

			foreach (var rowIdx in fullRows)
			{

				grid.RemoveRow(rowIdx);

				PieceType[,] drawableGrid = grid.GetDrawable();

				for (int i = rowIdx - 2; i > 0; i--)
				{

					for (int j = 0; j < StaticVars.drawGrid.GetLength(1); j++)
					{

						//Only move "Dead" textures since the rest gets sorted out
						if (drawableGrid[i, j] == PieceType.Dead)
						{

							StaticVars.drawGrid[i, j].Texture = StaticVars.drawGrid[i - 1, j].Texture;

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
			grid = new Grid(teacher);
			teacher = false;

			holdManager = new Hold();

			for (int i = 0; i < StaticVars.drawGrid.GetLength(0); i++)
			{

				for (int j = 0; j < StaticVars.drawGrid.GetLength(1); j++)
				{

					StaticVars.drawGrid[i, j] = new Sprite();

					//Place each block of the grid at the corresponding position of the array
					StaticVars.drawGrid[i, j].Position = new Vector2f(j * StaticVars.imageSize.X + StaticVars.gridXPos, i * StaticVars.imageSize.Y + StaticVars.gridYPos + StaticVars.imageSize.Y);

					//drawGrid[i, j].Size = new Vector2f(WINDOW_WIDTH / 10, WINDOW_HEIGHT / 20);
					//drawGrid[i, j].Size = new Vector2f(20, 20);

				}

			}

			#region End text
			endText = new Text("You have lost\npress space\nto reset\nthe game", StaticVars.font);

			FloatRect textRect = endText.GetLocalBounds();
			endText.Origin = new Vector2f(textRect.Left + textRect.Width/2.0f, textRect.Top  + textRect.Height/2.0f);
			endText.Position = new Vector2f(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2);
			#endregion

			FloatRect textRectPause = StaticVars.pauseText.GetLocalBounds();
			StaticVars.pauseText.Origin = new Vector2f(textRectPause.Left + textRectPause.Width / 2.0f, textRectPause.Top + textRectPause.Height / 2.0f);
			StaticVars.pauseText.Position = new Vector2f(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2);

			//Make it so the game is playing
			gameState = GameState.Playing;

			pieceQueue = new PieceQueue();

			stats = new Stats();

			//Select a new active piece
			NewPiece();

			//Add the piece to the grid, with a movement of 0,0
			grid.AddPiece(activePiece, new Vector2i(0,0));

			StaticVars.queueSprite.Position = new Vector2f(StaticVars.gridXPos + StaticVars.imageSize.X * (StaticVars.drawGrid.GetLength(1) + 2.25f), StaticVars.gridYPos);
			StaticVars.holdSprite.Position = new Vector2f(StaticVars.gridXPos - StaticVars.holdTexture.Size.X - (StaticVars.imageSize.X * 2.25f), StaticVars.gridYPos);
			StaticVars.drawGridSprite.Position = new Vector2f(StaticVars.gridXPos - StaticVars.imageSize.X * 1.5f, StaticVars.gridYPos - StaticVars.imageSize.Y * 2f);

			#region Queue setup

			queueSpriteArray4x4 = new Sprite[3][,]{	new Sprite[4,4],
													new Sprite[4,4],
													new Sprite[4,4]};

			for (int i = 0; i < queueSpriteArray4x4.Length; i++)
			{

				for (int j = 0; j < queueSpriteArray4x4[0].GetLength(0); j++)
				{

					for (int k = 0; k < queueSpriteArray4x4[0].GetLength(1); k++)
					{

                        queueSpriteArray4x4[i][j, k] = new Sprite();

						queueSpriteArray4x4[i][j, k].Position = new Vector2f(	StaticVars.queueSprite.Position.X + StaticVars.imageSize.X * 1.5f + StaticVars.imageSize.X * k,
																				StaticVars.queueSprite.Position.Y + StaticVars.imageSize.Y * 2.5f + StaticVars.imageSize.Y * j + StaticVars.imageSize.Y * ( queueSpriteArray4x4[0].GetLength(0) - 1) * i - StaticVars.imageSize.Y);

					}

				}

			}

			queueSpriteArray1x4 = new Sprite[3][,]{ new Sprite[1,4],
													new Sprite[1,4],
													new Sprite[1,4]};

			for (int i = 0; i < queueSpriteArray1x4.Length; i++)
			{

				for (int j = 0; j < queueSpriteArray1x4[0].GetLength(0); j++)
				{

					for (int k = 0; k < queueSpriteArray1x4[0].GetLength(1); k++)
					{

						queueSpriteArray1x4[i][j, k] = new Sprite();

						queueSpriteArray1x4[i][j, k].Position = new Vector2f(	StaticVars.queueSprite.Position.X + StaticVars.imageSize.X * 1.5f + StaticVars.imageSize.X * k,
																				StaticVars.queueSprite.Position.Y + StaticVars.imageSize.Y * 2.5f + StaticVars.imageSize.Y * j + StaticVars.imageSize.Y * (queueSpriteArray4x4[0].GetLength(0) - 1) * i + StaticVars.imageSize.Y * 0.5f);

					}

				}

			}


			queueSpriteArray3x3 = new Sprite[3][,]{	new Sprite[3,3],
													new Sprite[3,3],
													new Sprite[3,3]};

			for (int i = 0; i < queueSpriteArray3x3.Length; i++)
			{

				for (int j = 0; j < queueSpriteArray3x3[0].GetLength(0); j++)
				{

					for (int k = 0; k < queueSpriteArray3x3[0].GetLength(1); k++)
					{

						queueSpriteArray3x3[i][j, k] = new Sprite();
						
						queueSpriteArray3x3[i][j, k].Position = new Vector2f(	StaticVars.queueSprite.Position.X + StaticVars.imageSize.X * 1.5f + StaticVars.imageSize.X * k + StaticVars.imageSize.X * 0.5f,
																				StaticVars.queueSprite.Position.Y + StaticVars.imageSize.Y * 2.5f + StaticVars.imageSize.Y * j + StaticVars.imageSize.Y * queueSpriteArray3x3[0].GetLength(0) * i );

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

					holdSprite3x3[i, j].Position = new Vector2f(	StaticVars.holdSprite.Position.X + StaticVars.imageSize.X * 1.5f + StaticVars.imageSize.X * j + StaticVars.imageSize.X * 0.5f,
																	StaticVars.holdSprite.Position.Y + StaticVars.imageSize.Y * 3.5f + StaticVars.imageSize.Y * i);

				}

			}

			holdSprite4x4 = new Sprite[4, 4];

			for (int i = 0; i < holdSprite4x4.GetLength(0); i++)
			{

				for (int j = 0; j < holdSprite4x4.GetLength(1); j++)
				{

					holdSprite4x4[i, j] = new Sprite();

					holdSprite4x4[i, j].Position = new Vector2f(	StaticVars.holdSprite.Position.X + StaticVars.imageSize.X * 1.5f + StaticVars.imageSize.X * j,
																	StaticVars.holdSprite.Position.Y + StaticVars.imageSize.Y * 2.5f + StaticVars.imageSize.Y * i);

				}

			}

			holdSprite1x4 = new Sprite[1, 4];

			for (int i = 0; i < holdSprite1x4.GetLength(0); i++)
			{

				for (int j = 0; j < holdSprite1x4.GetLength(1); j++)
				{

					holdSprite1x4[i, j] = new Sprite();

					holdSprite1x4[i, j].Position = new Vector2f(	StaticVars.holdSprite.Position.X + StaticVars.imageSize.X * 1.5f + StaticVars.imageSize.X * j,
																	StaticVars.holdSprite.Position.Y + StaticVars.imageSize.Y * 2.5f + StaticVars.imageSize.Y * i + StaticVars.imageSize.Y * 1.5f);

				}

			}

			#endregion

			#region Text

			score = new Score();

			level = new Level();

			realTime = new RealTime();

			#endregion

			menu = new Menu("Play", "Options", "Exit");

			optionsMenu = new Options();

			soundplayer = new SoundPlayer(selectedSong);

			StaticVars.statsSprite.Position = new Vector2f(StaticVars.holdSprite.Position.X, realTime.Position.Y + StaticVars.imageSize.Y);

			StaticVars.controlsText.Position = new Vector2f(StaticVars.queueSprite.Position.X, StaticVars.queueSprite.Position.Y + StaticVars.queueTexture.Size.Y + StaticVars.imageSize.Y);

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
						level.LevelUp();
						break;
                    
                    // Rotate active piece clockwise.
					case Keyboard.Key.Up:
                    case Keyboard.Key.W:
                        if (grid.RotatePiece(activePiece, 0))
                        {
							dropTime -= level.sideMoveSpeed;
                        }
                        break;

                    // Rotate active piece counter clockwise.
                    case Keyboard.Key.E:
                        if (grid.RotatePiece(activePiece, 1))
                        {
                            dropTime -= level.sideMoveSpeed;
                        }
                        break;

                    // Move active piece left.
                    case Keyboard.Key.Left:
					case Keyboard.Key.A:
						if (!grid.CanPlacePiece(activePiece, StaticVars.down))
						{

							dropTime -= level.sideMoveSpeed;
							
						}

						grid.MovePiece(activePiece, StaticVars.left);

						break;

                    // Move active piece down.
					case Keyboard.Key.Down:
					case Keyboard.Key.S:
						if (grid.CanPlacePiece(activePiece, StaticVars.down))
						{

							grid.MovePiece(activePiece, StaticVars.down);
							score.AddScore(1);

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

							dropTime -= level.sideMoveSpeed;

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

							if(holdManager.currentPiece == PieceType.Empty)
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
						score.AddScore(2 * spacesMoved);

						grid.MovePiece(activePiece, StaticVars.down * (spacesMoved));

						dropTime = level.dropSpeed;

						DrawActivePieceHardDrop();
                        break;

                    // Opens the menu.
					case Keyboard.Key.Escape:
					case Keyboard.Key.M:
						soundplayer.Stop();
						gameState = GameState.Menu;
						break;

                    // Pause game.
					case Keyboard.Key.P:
						gameState = GameState.Pause;
						break;

                    default:
						break;

				}

			}
			else if (gameState == GameState.End)
			{


				switch (e.Code)
				{

                    // Opens menu.
                    case Keyboard.Key.Escape:
						soundplayer.Stop();
						gameState = GameState.Menu;
						break;

                    // Starts a new game.
					case Keyboard.Key.Space:
						StartNewGame();
						break;

					default:
						break;

				}
				
			}
			else if (gameState == GameState.Pause)
			{
				
				switch (e.Code)
				{

                    // Quits the application.
					case Keyboard.Key.Escape:
						Quit();
						break;

                    // Unpauses the game.
					case Keyboard.Key.P:
						gameState = GameState.Playing;
						break;

					default:
						break;

				}

			}
			else if(gameState == GameState.Menu)
			{

				#region Menu
				switch (e.Code)
				{

					case Keyboard.Key.Escape:
						Quit();
						break;

					case Keyboard.Key.M:
						gameState = GameState.Playing;
						break;

					case Keyboard.Key.Up:
						menu.MoveCursor(Direction.Up);
						break;

					case Keyboard.Key.Down:
						menu.MoveCursor(Direction.Down);
						break;

					case Keyboard.Key.Space:
						switch (menu.cursorPos)
						{

							case 0:
								StartNewGame();
								break;
							case 1:
								gameState = GameState.Options;
								break;
							case 2:
								Quit();
								break;
							default:
								break;

						}
						break;

					default:
						break;

				}
				#endregion

			}
			else if(gameState == GameState.Options)
			{


				switch (e.Code)
				{

					case Keyboard.Key.Escape:
						soundplayer.Stop();
						gameState = GameState.Menu;
						break;

					case Keyboard.Key.Up:
						optionsMenu.UpdateCursor(Direction.Up);
						break;

					case Keyboard.Key.Down:
						optionsMenu.UpdateCursor(Direction.Down);
						break;

					case Keyboard.Key.Left:
						optionsMenu.UpdateCursor(Direction.Left);
						break;
					
					case Keyboard.Key.Right:
						optionsMenu.UpdateCursor(Direction.Right);
						break;

					case Keyboard.Key.Space:
						switch (optionsMenu.cursorPosY)
						{

							case 0:
								switch (optionsMenu.cursorPosX)
								{

									case 0:
										selectedSong = "Music/korobeiniki.wav";
										break;

									case 1:
										selectedSong = "Music/katyusha.wav";
										break;

									case 2:
										selectedSong = "Music/kalinka.wav";
										break;

								}
								musicOff = false;
								gameState = GameState.Menu;
								break;

							case 1:
								musicOff = true;
								gameState = GameState.Menu;
								break;

							case 2:
								teacher = true;
								StartNewGame();
								break;

							case 3:
								gameState = GameState.Menu;
								break;

							default:
								break;

						}
						break;

					default:
						break;

				}

			}

		}

		/// <summary>
		/// Called when the window "X" is clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void window_Closed(object sender, EventArgs e)
		{

			Quit();

		}

		#endregion
		
		/// <summary>
		/// Update code of the program
		/// </summary>
		private void Update()
		{

			gameTime = clock.Restart();

			#region FPS stuff, no touchy

			timeElapsed += gameTime.AsSeconds();

			//If 1 second has passed in the game
			if (timeElapsed > 1)
			{

				realTime.UpdateTimeString();

				Console.WriteLine("FPS: {0}", fps);

				fps = 0;
				timeElapsed = 0;

			}

			fps++;

			#endregion

			if (gameState == GameState.Playing)
			{

				dropTime += gameTime.AsSeconds();

				realTime.realTime += gameTime.AsSeconds();

				if (grid.CheckLose())
				{

					gameState = GameState.End;
					return;

				}

				if (dropTime > level.dropSpeed)
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

		/// <summary>
		/// Draw code of the program
		/// </summary>
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
				for (int i = 0; i < drawArray.GetLength(0); i++)
				{

					for (int j = 0; j < drawArray.GetLength(1); j++)
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
						if (drawArray[i, j] != PieceType.Dead)
						{

							//Associated the blockTextures with the same indexes as the Enum
							//Look at both and you will understand
							StaticVars.drawGrid[i, j].Texture = StaticVars.blockTextures[(int)drawArray[i, j]];

						}

						//Finally draw to the screen the final results
						window.Draw(StaticVars.drawGrid[i, j]);

					}

				}

				#endregion

				//Draw the queue

				#region Queue

				List<PieceType> queueArray = pieceQueue.GetList();

				for (int i = 0; i < queueArray.Count; i++)
				{

					PieceType[,] queuePieceToDraw = StaticVars.GetPieceArray(queueArray[i]);

					if(queueArray[i] == PieceType.I)
					{

						for (int j = 0; j < queueSpriteArray1x4[0].GetLength(0); j++)
						{

							for (int k = 0; k < queuePieceToDraw.GetLength(1); k++)
							{

								if (queuePieceToDraw[j + 1, k] != PieceType.Empty)
								{

									queueSpriteArray1x4[i][j, k].Texture = StaticVars.blockTextures[(int)queuePieceToDraw[j + 1, k]];

									window.Draw(queueSpriteArray1x4[i][j, k]);

								}

							}

						}

					}
					else if (queuePieceToDraw.GetLength(1) == 4)
					{

						for (int j = 0; j < queuePieceToDraw.GetLength(0); j++)
						{

							for (int k = 0; k < queuePieceToDraw.GetLength(1); k++)
							{

								if (queuePieceToDraw[j, k] != PieceType.Empty)
								{

									queueSpriteArray4x4[i][j, k].Texture = StaticVars.blockTextures[(int)queuePieceToDraw[j, k]];

									window.Draw(queueSpriteArray4x4[i][j, k]);

								}



							}

						}
					}
					else
					{


						for (int j = 0; j < queuePieceToDraw.GetLength(0); j++)
						{

							for (int k = 0; k < queuePieceToDraw.GetLength(1); k++)
							{

								if (queuePieceToDraw[j, k] != PieceType.Empty)
								{

									queueSpriteArray3x3[i][j,k].Texture = StaticVars.blockTextures[(int)queuePieceToDraw[j, k]];
									window.Draw(queueSpriteArray3x3[i][j, k]);

								}

							}

						}

					}

				}

				#endregion

				#region Hold
				
				PieceType[,] pieceToDraw = StaticVars.GetPieceArray(holdManager.currentPiece);

				if(holdManager.currentPiece == PieceType.I)
				{

					for (int j = 0; j < holdSprite1x4.GetLength(0); j++)
					{

						for (int k = 0; k < pieceToDraw.GetLength(1); k++)
						{

							if (pieceToDraw[j + 1, k] != PieceType.Empty)
							{

								holdSprite1x4[j, k].Texture = StaticVars.blockTextures[(int)pieceToDraw[j + 1, k]];

								window.Draw(holdSprite1x4[j, k]);

							}



						}

					}

				}
				else if (pieceToDraw.GetLength(1) == 4)
				{

					for (int j = 0; j < pieceToDraw.GetLength(0); j++)
					{

						for (int k = 0; k < pieceToDraw.GetLength(1); k++)
						{

							if (pieceToDraw[j, k] != PieceType.Empty)
							{

								holdSprite4x4[j, k].Texture = StaticVars.blockTextures[(int)pieceToDraw[j, k]];

								window.Draw(holdSprite4x4[j, k]);

							}



						}

					}

				}
				else
				{


					for (int j = 0; j < pieceToDraw.GetLength(0); j++)
					{

						for (int k = 0; k < pieceToDraw.GetLength(1); k++)
						{

							if (pieceToDraw[j, k] != PieceType.Empty)
							{

								holdSprite3x3[j, k].Texture = StaticVars.blockTextures[(int)pieceToDraw[j, k]];
								window.Draw(holdSprite3x3[j, k]);

							}

						}

					}

				}

				#endregion

				window.Draw(score);

				window.Draw(level);

				window.Draw(realTime);

				window.Draw(StaticVars.statsSprite);

				window.Draw(StaticVars.controlsText);

				foreach (var item in stats.GetDrawable())
				{

					window.Draw(item);
					
				}

				if(gameState == GameState.Pause)
				{

					window.Draw(StaticVars.pauseText);

				}

			}
			else if (gameState == GameState.End)
			{
				//Draw the background first, so it's in the back
				window.Draw(StaticVars.backDrop);
				window.Draw(StaticVars.holdSprite);
				window.Draw(StaticVars.queueSprite);
				window.Draw(StaticVars.drawGridSprite);
				window.Draw(endText);
				window.Draw(score);
				window.Draw(level);
				window.Draw(realTime);
				window.Draw(StaticVars.statsSprite);
				window.Draw(StaticVars.controlsText);

				foreach (var item in stats.GetDrawable())
				{

					window.Draw(item);

				}

			}
			else if(gameState == GameState.Menu)
			{

				//window.Draw(StaticVars.menuBackdrop);

				foreach (var item in menu.GetDrawable())
				{

					window.Draw(item);

				}

			}
			else if (gameState == GameState.Options)
			{

				foreach (var item in optionsMenu.GetDrawable())
				{

					window.Draw(item);

				}

			}

			window.Display();

		}

	}
}
