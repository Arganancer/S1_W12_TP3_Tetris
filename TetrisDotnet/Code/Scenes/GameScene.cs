using System.Collections.Generic;
using System.Diagnostics;
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
		private Grid grid = new Grid();
		private readonly PieceQueue pieceQueue = new PieceQueue();
		private Piece activePiece;
		private readonly Hold holdManager = new Hold();
		private SceneType nextScene;
		private bool isPaused;
		private Statistics statistics;

		private float timeUntilNextDrop;
		private const float PieceLockDelay = 0.5f;
		private float timeUntilPieceLock;
		
		// AI Elements
		private readonly Evaluator evaluator = new Evaluator();
		private readonly Controller controller = new Controller();
		private const float AiTickInterval = 0.05f;
		private float lastAiTick;
		private bool aiPlaying = true;

		public GameScene() : base(SceneType.Game, new GameLayout())
		{
			nextScene = SceneType;
			Application.EventSystem.Subscribe(EventType.InputPause, OnInputPause);
			SubscribeToInputs();
			statistics = new Statistics();
			StartNewGame();

			Application.EventSystem.Subscribe(EventType.ToggleAi, OnAiToggled);
		}

		~GameScene()
		{
			UnsubscribeFromInputs();
			Application.EventSystem.Unsubscribe(EventType.InputPause, OnInputPause);
			Application.EventSystem.Unsubscribe(EventType.ToggleAi, OnAiToggled);
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

			if (timeUntilNextDrop >= statistics.DropSpeed)
			{
				timeUntilNextDrop = 0.0f;

				if (grid.CanPlacePiece(activePiece, Vector2iUtils.down))
				{
					grid.MovePiece(activePiece, Vector2iUtils.down);
				}
			}
			
			if (!grid.CanPlacePiece(activePiece, Vector2iUtils.down))
			{
				timeUntilPieceLock += deltaTime;
				if (timeUntilPieceLock >= PieceLockDelay)
				{
					timeUntilPieceLock = 0.0f;
					grid.KillPiece(activePiece);
					CheckFullRows();
					NewPiece();
				}
			}
			else
			{
				timeUntilPieceLock = 0.0f;
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
		
		private void OnAiToggled(EventData eventData)
		{
			ToggleEventData toggleEventData = eventData as ToggleEventData;
			Debug.Assert(toggleEventData != null, nameof(toggleEventData) + " != null");
			aiPlaying = toggleEventData.IsOn;
		}

		private void OnInputRotateClockwise(EventData eventData)
		{
			if (grid.RotatePiece(activePiece, Rotation.Clockwise))
			{
				timeUntilPieceLock = 0.0f;
			}
		}

		private void OnInputRotateCounterClockwise(EventData eventData)
		{
			if (grid.RotatePiece(activePiece, Rotation.CounterClockwise))
			{
				timeUntilPieceLock = 0.0f;
			}
		}

		private void OnInputDown(EventData eventData)
		{
			if (grid.CanPlacePiece(activePiece, Vector2iUtils.down))
			{
				grid.MovePiece(activePiece, Vector2iUtils.down);
				Application.EventSystem.ProcessEvent(EventType.PieceDropped, new PieceDroppedEventData(Drop.SoftDrop, 1));
				timeUntilNextDrop = 0.0f;
			}
		}

		private void OnInputLeft(EventData eventData)
		{
			if (grid.CanPlacePiece(activePiece, Vector2iUtils.left))
			{
				timeUntilPieceLock = 0.0f;
				grid.MovePiece(activePiece, Vector2iUtils.left);
			}
		}

		private void OnInputRight(EventData eventData)
		{
			if (grid.CanPlacePiece(activePiece, Vector2iUtils.right))
			{
				timeUntilPieceLock = 0.0f;
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

		private void StartNewGame()
		{
			grid = new Grid();
			statistics.Reset();
			NewPiece();
		}

		private void NewPiece(PieceType type = PieceType.Empty)
		{
			timeUntilPieceLock = 0.0f;
			timeUntilNextDrop = 0.0f;
			
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

			List<Move> moves = new List<Move> {MoveUtils.GetLinesCleared(idxRowFull.Count)};

			// TODO: Add TSpin modifier.
			// TODO: This should not be called here.
			Application.EventSystem.ProcessEvent(EventType.PiecePlaced, new PiecePlacedEventData(moves));

			RemoveRows(idxRowFull);
		}

		private void RemoveRows(IEnumerable<int> fullRows)
		{
			foreach (int rowIdx in fullRows)
			{
				grid.RemoveRow(rowIdx);
			}
		}
	}
}