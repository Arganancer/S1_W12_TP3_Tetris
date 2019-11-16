using System.Collections.Generic;
using TetrisDotnet.Code.AI.Pathfinding;
using TetrisDotnet.Code.Game;

namespace TetrisDotnet.Code.AI
{
	class Action
	{
		public ActionType actionType { get; }
		public Stack<PathNode> path { get; }

		public Action(ActionType actionType, Stack<PathNode> path)
		{
			this.actionType = actionType;
			this.path = path;
		}
	}
}