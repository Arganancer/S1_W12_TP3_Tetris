using System;
using SFML.System;

namespace TetrisDotnet.Code.Utils
{
	public class Vector2I : IComparable<Vector2I>
	{
		private Vector2i position;
		public int Y => position.Y;
		public int X => position.X;

		public static implicit operator Vector2i(Vector2I vec) => vec.position;
		public static implicit operator Vector2I(Vector2i vec) => new Vector2I(vec);

		public Vector2I(Vector2i position)
		{
			this.position = position;
		}

		public Vector2I(int x, int y)
		{
			position = new Vector2i(x, y);
		}

		public static bool operator ==(Vector2I a, Vector2I b)
		{
			return a.position == b.position;
		}
		
		public static bool operator ==(Vector2I a, Vector2i b)
		{
			return a.position == b;
		}

		public static bool operator !=(Vector2I a, Vector2I b)
		{
			return !(a == b);
		}
		
		public static bool operator !=(Vector2I a, Vector2i b)
		{
			return !(a == b);
		}
		
		public static Vector2I operator +(Vector2I a, Vector2I b)
		{
			return new Vector2I(a.position + b.position);
		}

		public int CompareTo(Vector2I other)
		{
			if (position.X > other.position.X)
				return 1;
			if (position.X < other.position.X)
				return -1;
			if (position.Y > other.position.Y)
				return 1;
			if (position.Y < other.position.Y)
				return -1;
			return 0;
		}

		private bool Equals(Vector2I other)
		{
			return position.Equals(other.position);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Vector2I) obj);
		}

		public override int GetHashCode()
		{
			return position.GetHashCode();
		}
	}
}