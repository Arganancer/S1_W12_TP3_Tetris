using System;
using SFML.Graphics;
using SFML.System;

namespace TetrisDotnet.Code.Utils
{
	public static class Interpolation
	{
		private static float CalculateCosineMu(float mu)
		{
			return (float) (1.0f - Math.Cos(mu * Math.PI)) * 0.5f;
		}
		
		public static float Lerp(float origin, float target, float mu)
		{
			return origin * (1.0f - mu) + target * mu;
		}

		public static float CosineInterpolate(float origin, float target, float mu)
		{
			return Lerp(origin, target, CalculateCosineMu(mu));
		}
		
		public static Vector2f Lerp(Vector2f origin, Vector2f target, float mu)
		{
			return origin * (1 - mu) + target * mu;
		}
		
		public static Vector2f CosineInterpolate(Vector2f origin, Vector2f target, float mu)
		{
			return Lerp(origin, target, CalculateCosineMu(mu));
		}
		
		public static Color Lerp(Color origin, Color target, float mu)
		{
			return new Color((byte) Lerp(origin.R, target.R, mu), (byte) Lerp(origin.G, target.G, mu), (byte) Lerp(origin.B, target.B, mu));
		}
		
		public static Color CosineInterpolate(Color origin, Color target, float mu)
		{
			float mu2 = CalculateCosineMu(mu);
			return Lerp(origin, target, mu2);
		}
	}
}