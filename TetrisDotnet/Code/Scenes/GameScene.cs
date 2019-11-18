using System;
using System.Collections.Generic;
using SFML.System;
using TetrisDotnet.Code.AI;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.Game;
using TetrisDotnet.Code.Game.Stats;
using TetrisDotnet.Code.Game.World;
using TetrisDotnet.Code.UI.Layouts;
using TetrisDotnet.Code.Utils;
using TetrisDotnet.Code.Utils.Enums;
using Action = TetrisDotnet.Code.AI.Action;

namespace TetrisDotnet.Code.Scenes
{
	public class GameScene : Scene
	{
		// Logic Elements
		private readonly Grid grid = new Grid();
		private readonly PieceQueue pieceQueue = new PieceQueue();
		private Piece activePiece;
		private readonly Hold holdManager = new Hold();
		private SceneType nextScene;
		private bool isPaused = false;
		private readonly Statistics statistics = new Statistics();

		private float timeUntilNextDrop = 0;
		private const float PieceLockDelay = 0.5f;
		private float timeUntilPieceLock = PieceLockDelay;
		
		// AI Elements
		private readonly Evaluator evaluator = new Evaluator();
		private readonly Controller controller = new Controller();
		private const float AiTickInterval = 0.0001f;
		private float lastAiTick;
		private bool aiPlaying = false;

		public GameScene() : base(SceneType.Game, new GameLayout())
		{
			nextScene = SceneType;
			Application.EventSystem.Subscribe(EventType.InputPause, OnInputPause);
			SubscribeToInputs();
			StartNewGame();
		}

		~GameScene()
		{
			UnsubscribeFromInputs();
			Application.EventSystem.Unsubscribe(EventType.InputPause, OnInputPause);
		}

		private void SubscribeToInputs()
		{
			Application.EventSystem.Subscribe(EventType.InputRotateClockwise, OnInputRotateClockwise);
			Application.EventSystem.Subscribe(EventType.InputRotateCounterClockwise, OnInputRotateCounterClockwise);
			Application.EventSystem.Subscribe(EventType.InputDown, OnInputDown);
			Application.EventSystem.Subscribe(EventType.InputLeft, OnInputLeft);
			Application.EventSystem.Subscribe(EventType.InputRight, OnInputRight);
			Application.EventSystem.Subscribe(EventType.InputHold, OnInputHold);
			Application.EventSystem.Subscribe(EventType.InputHardDrop, OnInputHardDrop);
		}

		private void UnsubscribeFromInputs()
		{
			Application.EventSystem.Unsubscribe(EventType.InputRotateClockwise, OnInputRotateClockwise);
			Application.EventSystem.Unsubscribe(EventType.InputRotateCounterClockwise, OnInputRotateCounterClockwise);
			Application.EventSystem.Unsubscribe(EventType.InputDown, OnInputDown);
			Application.EventSystem.Unsubscribe(EventType.InputLeft, OnInputLeft);
			Application.EventSystem.Unsubscribe(EventType.InputRight, OnInputRight);
			Application.EventSystem.Unsubscribe(EventType.InputHold, OnInputHold);
			Application.EventSystem.Unsubscribe(EventType.InputHardDrop, OnInputHardDrop);
		}

		public override void Resume()
		{
			nextScene = SceneType;
		}

		public override SceneType Update(float deltaTime)
		{
			if (isPaused)
				return nextScene;

			timeUntilNextDrop += deltaTime;

			if (timeUntilNextDrop > statistics.DropSpeed)
			{
				timeUntilNextDrop = 0;

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

			if (aiPlaying)
			{
				lastAiTick += deltaTime;
				if (lastAiTick >= AiTickInterval)
				{
					int nbOfTicks = (int) (lastAiTick / AiTickInterval);
					lastAiTick = 0;
					controller.RunCommands(new State(activePiece, grid.GetBoolGrid(), holdManager.CurrentPiece),
						nbOfTicks);
				}
			}

			base.Update(deltaTime);
			return nextScene;
		}

		private void OnInputRotateClockwise(EventData eventData)
		{
			if (grid.RotatePiece(activePiece, Rotation.Clockwise))
			{
				timeUntilPieceLock = PieceLockDelay;
			}
		}

		private void OnInputRotateCounterClockwise(EventData eventData)
		{
			if (grid.RotatePiece(activePiece, Rotation.CounterClockwise))
			{
				timeUntilPieceLock = PieceLockDelay;
			}
		}

		private void OnInputDown(EventData eventData)
		{
			if (grid.CanPlacePiece(activePiece, Vector2iUtils.down))
			{
				grid.MovePiece(activePiece, Vector2iUtils.down);
				Application.EventSystem.ProcessEvent(EventType.PieceDropped, new PieceDroppedEventData(Drop.SoftDrop, 1));
			}
			else
			{
				timeUntilNextDrop = statistics.DropSpeed;
			}
		}

		private void OnInputLeft(EventData eventData)
		{
			if (grid.CanPlacePiece(activePiece, Vector2iUtils.left))
			{
				timeUntilPieceLock = PieceLockDelay;
				grid.MovePiece(activePiece, Vector2iUtils.left);
			}
		}

		private void OnInputRight(EventData eventData)
		{
			if (grid.CanPlacePiece(activePiece, Vector2iUtils.right))
			{
				timeUntilPieceLock = PieceLockDelay;
				grid.MovePiece(activePiece, Vector2iUtils.right);
			}
		}

		private void OnInputHold(EventData eventData)
		{
			if (holdManager.CanSwap)
			{
				PieceType oldPiece = activePiece.Type;

				grid.RemovePiece(activePiece);

				if (holdManager.CurrentPiece == PieceType.Empty)
				{
					NewPiece();
				}
				else
				{
					NewPiece(holdManager.CurrentPiece);
				}

				holdManager.CurrentPiece = oldPiece;
				holdManager.CanSwap = false;
			}
		}

		private void OnInputHardDrop(EventData eventData)
		{
			int spacesMoved = grid.DetermineDropdownPosition(activePiece);
			Application.EventSystem.ProcessEvent(EventType.PieceDropped, new PieceDroppedEventData(Drop.HardDrop, spacesMoved));

			grid.MovePiece(activePiece, Vector2iUtils.down * spacesMoved);
			
			grid.KillPiece(activePiece);
			CheckFullRows();
			NewPiece();
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
			
			Application.EventSystem.ProcessEvent(EventType.GamePauseToggled, new GamePauseToggledEventData(isPaused));
		}

		private void InitializeGame()
		{
			//Select a new active piece
			NewPiece();
		}

		private void StartNewGame()
		{
			InitializeGame();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		private void NewPiece(PieceType type = PieceType.Empty)
		{
			timeUntilPieceLock = PieceLockDelay;
			timeUntilNextDrop = statistics.DropSpeed;
			
			holdManager.CanSwap = true;

			activePiece = type == PieceType.Empty ? new Piece(pieceQueue.GrabNext()) : new Piece(type);

			if (grid.CheckLose())
			{
				StartNewGame();
			}
			else
			{
				grid.MovePiece(activePiece, Vector2iUtils.flat);

				if (aiPlaying)
				{
					State currentState = new State(activePiece, grid.GetBoolGrid(), holdManager.CurrentPiece);
					Action action = evaluator.GetBestPlacement(currentState);
					controller.PlanPath(action);
				}
			}
		}

		private void CheckFullRows()
		{
			List<int> idxRowFull = grid.GetFullRows();
			
			List<Move> moves = new List<Move>();
			moves.Add(MoveUtils.GetLinesCleared(idxRowFull.Count));
			
			// TODO: Add TSpin modifier.
			// TODO: This should not be called here.
			Application.EventSystem.ProcessEvent(EventType.PiecePlaced, new PiecePlacedEventData(moves));

			RemoveRows(idxRowFull);
		}

		private void RemoveRows(List<int> fullRows)
		{
			foreach (int rowIdx in fullRows)
			{
				grid.RemoveRow(rowIdx);
			}
		}
	}
}