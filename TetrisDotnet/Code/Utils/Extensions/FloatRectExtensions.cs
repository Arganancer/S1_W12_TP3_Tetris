using SFML.Graphics;
using SFML.System;

namespace TetrisDotnet.Code.Utils.Extensions
{
	public static class FloatRectExtensions
	{
		public static Vector2f Center(this FloatRect floatRect)
		{
			return new Vector2f(floatRect.Left + floatRect.Width * 0.5f, floatRect.Top + floatRect.Height * 0.5f);
		}

		public static bool Contains(this FloatRect floatRect, Vector2i position)
		{
			return floatRect.Contains(position.X, position.Y);
		}
	}
}