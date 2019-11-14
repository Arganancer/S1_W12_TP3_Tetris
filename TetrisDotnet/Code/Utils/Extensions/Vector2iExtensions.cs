using System;
using SFML.System;

namespace TetrisDotnet.Code.Utils.Extensions
{
	// ReSharper disable once InconsistentNaming
	static class Vector2iExtensions
	{
		public static float Distance(this Vector2i origin, Vector2i destination)
		{
			float x = origin.X - destination.X;
			float z = origin.Y - destination.Y;

			return (float) Math.Sqrt(x * x + z * z);
		}
	}
}