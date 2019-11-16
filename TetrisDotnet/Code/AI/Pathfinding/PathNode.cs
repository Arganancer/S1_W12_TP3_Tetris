using System;
using SFML.System;
using TetrisDotnet.Code.Utils;

namespace TetrisDotnet.Code.AI.Pathfinding
{
	class PathNode : IComparable<PathNode>
	{
		public Vector2I Position;
		public int Rotation;
		public PathNode Parent;
		public float f;
		public float g;
		public float h;

		public PathNode(Vector2i position, float f = 0, float g = 0, float h = 0)
		{
			Position = position;
			this.f = f;
			this.g = g;
			this.h = h;
		}
		
		public int CompareTo(PathNode other)
		{
			return Position.CompareTo(other.Position);
		}
	}
}