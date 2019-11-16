using System.Collections.Generic;
using SFML.System;

namespace TetrisDotnet.Code.AI.Pathfinding.Lists
{
	class ClosedHashSet
	{
		private readonly HashSet<Vector2i> closedSet = new HashSet<Vector2i>();

		public void Add(PathNode pathNode)
		{
			closedSet.Add(pathNode.Position);
		}

		public bool Contains(PathNode pathNode)
		{
			return closedSet.Contains(pathNode.Position);
		}

		public bool Contains(Vector2i position)
		{
			return closedSet.Contains(position);
		}

		public void Clear()
		{
			closedSet.Clear();
		}
	}
}