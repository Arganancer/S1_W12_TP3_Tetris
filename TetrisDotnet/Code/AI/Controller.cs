﻿using System.Drawing;
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
		private Action currentAction;

		public void PlanPath(Action action)
		{
			currentAction = action;
			if (currentAction.actionType == ActionType.Place)
			{
				rotation = action.destinationPiece.rotationIndex;
			}
		}

		public void RunCommands(State state)
		{
			if (currentAction.actionType == ActionType.Place)
			{
				if (rotation-- > 0)
				{
					Application.eventSystem.ProcessEvent(EventType.InputRotateClockwise);
					return;
				}

				int moves = currentAction.destinationPiece.getGlobalBlocks.First().X -
				            state.currentPiece.getGlobalBlocks.First().X;
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
			else if (currentAction.actionType == ActionType.Hold)
			{
				Application.eventSystem.ProcessEvent(EventType.InputHold);
			}
		}
	}
}