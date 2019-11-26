using SFML.System;
using TetrisDotnet.Code.AI.Pathfinding;
using TetrisDotnet.Code.Events;

namespace TetrisDotnet.Code.AI
{
	class Controller
	{
		private Action currentAction = null;
		private PathNode currentPathNodeAction;

		public void PlanPath(Action action)
		{
			currentAction = action;
			if (currentAction != null && currentAction.actionType == ActionType.Place)
			{
				currentPathNodeAction = action.path.Pop();
			}
		}

		public bool RunCommands(State state, int nbOfTicks)
		{
			if (currentAction == null) return false;
			
			if (currentAction.actionType == ActionType.Place)
			{
				for (int i = 0; i < nbOfTicks; i++)
				{
					if (state.CurrentPiece.RotationIndex != currentPathNodeAction.Rotation)
					{
						Application.EventSystem.ProcessEvent(EventType.InputRotateClockwise);
						continue;
					}

					Vector2i currentMove = currentPathNodeAction.Position - state.CurrentPiece.Position;

					if (currentMove.X == 0 && currentMove.Y <= 0)
					{
						if (currentAction.path.Count == 0)
						{
							if (state.CurrentPiece.RotationIndex == currentPathNodeAction.Rotation)
							{
								Application.EventSystem.ProcessEvent(EventType.InputHardDrop);
							}

							currentAction = null;
							return true;
						}

						currentPathNodeAction = currentAction.path.Pop();
					}

					if (currentMove.X != 0)
					{
						Application.EventSystem.ProcessEvent(currentMove.X < 0
							? EventType.InputLeft
							: EventType.InputRight);

						continue;
					}

					if (currentMove.Y > 0)
					{
						Application.EventSystem.ProcessEvent(EventType.InputDown);
					}
				}
			}
			else if (currentAction.actionType == ActionType.Hold)
			{
				Application.EventSystem.ProcessEvent(EventType.InputHold);
			}

			return true;
		}
	}
}