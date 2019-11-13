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
		private bool isPaused;
		private float dropTime;

		// UI Elements
		private readonly GridUI gridUi = new GridUI();
		private ScoreText scoreText;
		private LevelText levelText;
		private RealTimeText realTimeText;
		private StatsTextBlock statsTextBlock;
		private readonly ControlsText controlsText;
		private readonly PauseText pauseText = new PauseText();

		public GameScene() : base(SceneType.Game)
		{
			isPaused = false;
			nextScene = SceneType;
			
			SubscribeToInputs();

			scoreText = new ScoreText();
			levelText = new LevelText();
			realTimeText = new RealTimeText();
			controlsText = new ControlsText();

			HeldPieceUI heldPieceUi = new HeldPieceUI();
			QueuedPiecesUI queuedPiecesUi = new QueuedPiecesUI();

			StartNewGame();
		}

		~GameScene()
		{
			UnsubscribeFromInputs();
		}

		private void SubscribeToInputs()
		{
			Application.eventSystem.Subscribe(EventType.InputRotateClockwise, OnInputRotateClockwise);
			Application.eventSystem.Subscribe(EventType.InputRotateCounterClockwise, OnInputRotateCounterClockwise);
			Application.eventSystem.Subscribe(EventType.InputDown, OnInputDown);
			Application.eventSystem.Subscribe(EventType.InputLeft, OnInputLeft);
			Application.eventSystem.Subscribe(EventType.InputRight, OnInputRight);
			Application.eventSystem.Subscribe(EventType.InputHold, OnInputHold);
			Application.eventSystem.Subscribe(EventType.InputHardDrop, OnInputHardDrop);
			Application.eventSystem.Subscribe(EventType.InputPause, OnInputPause);
		}

		private void UnsubscribeFromInputs()
		{
			Application.eventSystem.Unsubscribe(EventType.InputRotateClockwise, OnInputRotateClockwise);
			Application.eventSystem.Unsubscribe(EventType.InputRotateCounterClockwise, OnInputRotateCounterClockwise);
			Application.eventSystem.Unsubscribe(EventType.InputDown, OnInputDown);
			Application.eventSystem.Unsubscribe(EventType.InputLeft, OnInputLeft);
			Application.eventSystem.Unsubscribe(EventType.InputRight, OnInputRight);
			Application.eventSystem.Unsubscribe(EventType.InputHold, OnInputHold);
			Application.eventSystem.Unsubscribe(EventType.InputHardDrop, OnInputHardDrop);
			Application.eventSystem.Unsubscribe(EventType.InputPause, OnInputPause);
		}

		public override void Resume()
		{
			nextScene = SceneType;
		}

		public override SceneType Update(float deltaTime)
		{
			if (isPaused)
				return nextScene;

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
			window.Draw(AssetPool.backDrop);
			window.Draw(AssetPool.holdSprite);
			window.Draw(AssetPool.queueSprite);
			window.Draw(AssetPool.drawGridSprite);

			// TODO: This whole bit of code should go.
			for (int x = 0; x < Grid.GridWidth; x++)
			{
				for (int y = 0; y < Grid.VisibleGridHeight; y++)
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

			if (isPaused)
			{
				window.Draw(pauseText);
			}
		}

		private void OnInputRotateClockwise(EventData eventData)
		{
			if (grid.RotatePiece(activePiece, Rotation.Clockwise))
			{
				dropTime -= levelText.sideMoveSpeed;
			}
		}

		private void OnInputRotateCounterClockwise(EventData eventData)
		{
			if (grid.RotatePiece(activePiece, Rotation.CounterClockwise))
			{
				dropTime -= levelText.sideMoveSpeed;
			}
		}

		private void OnInputDown(EventData eventData)
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

		private void OnInputLeft(EventData eventData)
		{
			if (!grid.CanPlacePiece(activePiece, Vector2iUtils.down))
			{
				dropTime -= levelText.sideMoveSpeed;
			}

			grid.MovePiece(activePiece, Vector2iUtils.left);
		}

		private void OnInputRight(EventData eventData)
		{
			if (!grid.CanPlacePiece(activePiece, Vector2iUtils.down))
			{
				dropTime -= levelText.sideMoveSpeed;
			}

			grid.MovePiece(activePiece, Vector2iUtils.right);
		}

		private void OnInputHold(EventData eventData)
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

		private void OnInputHardDrop(EventData eventData)
		{
			int spacesMoved = grid.DetermineDropdownPosition(activePiece);
			scoreText.AddScore(2 * spacesMoved);

			grid.MovePiece(activePiece, Vector2iUtils.down * (spacesMoved));

			dropTime = levelText.dropSpeed;

			DrawActivePieceHardDrop();
		}

		private void OnInputPause(EventData eventData)
		{
			isPaused = !isPaused;
			if (isPaused)
			{
				UnsubscribeFromInputs();
			}
			else
			{
				SubscribeToInputs();;
			}
		}

		private void InitializeGame()
		{
			scoreText = new ScoreText();

			levelText = new LevelText();

			realTimeText = new RealTimeText();

			AssetPool.statsSprite.Position = new Vector2f(AssetPool.holdSprite.Position.X,
				realTimeText.Position.Y + AssetPool.blockSize.Y);

			statsTextBlock = new StatsTextBlock();

			controlsText.Position = new Vector2f(AssetPool.queueSprite.Position.X,
				AssetPool.queueSprite.Position.Y + AssetPool.queueTexture.Size.Y + AssetPool.blockSize.Y);

			//Select a new active piece
			NewPiece();

			//Add the piece to the grid, with a movement of 0,0
			grid.AddPiece(activePiece, new Vector2i(0, 0));

			AssetPool.drawGridSprite.Position = new Vector2f(GridUI.position.X - AssetPool.blockSize.X * 1.5f,
				GridUI.position.Y - AssetPool.blockSize.Y * 2f);
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
			List<int> idxRowFull = grid.GetFullRows();

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