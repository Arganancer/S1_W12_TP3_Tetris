using SFML.System;
using TetrisDotnet.Code.AI.Pathfinding;
using TetrisDotnet.Code.Events;

namespace TetrisDotnet.Code.AI
{
	class Controller
	{
		private Action currentAction;
		private PathNode currentPathNodeAction;

		public void PlanPath(Action action)
		{
			currentAction = action;
			if (currentAction.actionType == ActionType.Place)
			{
				currentPathNodeAction = action.path.Pop();
			}
		}

		public void RunCommands(State state, int nbOfTicks)
		{
			for (int i = 0; i < nbOfTicks; i++)
			{
				if (currentAction.actionType == ActionType.Place)
				{
					if (state.currentPiece.rotationIndex != currentPathNodeAction.Rotation)
					{
						Application.eventSystem.ProcessEvent(EventType.InputRotateClockwise);
						continue;
					}

					Vector2i currentMove = currentPathNodeAction.Position - state.currentPiece.position;

					if (currentMove.X == 0 && currentMove.Y <= 0)
					{
						if (currentAction.path.Count == 0)
						{
							if (state.currentPiece.rotationIndex == currentPathNodeAction.Rotation)
							{
								Application.eventSystem.ProcessEvent(EventType.InputHardDrop);
							}

							return;
						}

						currentPathNodeAction = currentAction.path.Pop();
					}

					if (currentMove.X != 0)
					{
						if (currentMove.X < 0)
						{
							Application.eventSystem.ProcessEvent(EventType.InputLeft);
						}
						else
						{
							Application.eventSystem.ProcessEvent(EventType.InputRight);
						}

						continue;
					}

					if (currentMove.Y > 0)
					{
						Application.eventSystem.ProcessEvent(EventType.InputDown);
					}
				}
				else if (currentAction.actionType == ActionType.Hold)
				{
					Application.eventSystem.ProcessEvent(EventType.InputHold);
				}
			}
		}
	}
}