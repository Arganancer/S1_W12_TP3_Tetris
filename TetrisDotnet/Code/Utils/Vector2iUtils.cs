using SFML.System;

namespace TetrisDotnet.Code.Utils
{
	// ReSharper disable once InconsistentNaming
	public static class Vector2iUtils
	{
		public static Vector2i flat { get; } = new Vector2i(0, 0);
		public static Vector2i down { get; } = new Vector2i(0, 1);
		public static Vector2i left { get; } = new Vector2i(-1, 0);
		public static Vector2i right { get; } = new Vector2i(1, 0);
	}
}