using System.Drawing;
using System.Linq;
using SFML.System;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Game;
using TetrisDotnet.Code.Game.World;

namespace TetrisDotnet.Code.AI
{
	class Controller
	{
		private int rotation;
		private Vector2i move = new Vector2i(0, 0);

		public void PlanPath(Piece desiredPlacement)
		{
			rotation = desiredPlacement.rotationIndex;

			move = desiredPlacement.position;
		}

		public void RunCommands(State state)
		{
			if (rotation > 0)
			{
				rotation--;
				Application.eventSystem.ProcessEvent(EventType.InputRotateClockwise);
				return;
			}

			int currentX = state.currentPiece.position.X;

			int moves = move.X - currentX;
			if (moves != 0)
			{
				if (moves < 0)
				{
					Application.eventSystem.ProcessEvent(EventType.InputLeft);
				}
				else
				{
					Application.eventSystem.ProcessEvent(EventType.InputRight);
				}

				return;
			}

			Application.eventSystem.ProcessEvent(EventType.InputHardDrop);
		}
	}
}