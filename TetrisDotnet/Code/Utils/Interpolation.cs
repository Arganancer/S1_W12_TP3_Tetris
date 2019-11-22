using System;
using SFML.Graphics;
using SFML.System;

namespace TetrisDotnet.Code.Utils
{
	public static class Interpolation
	{
		public static float Lerp(float origin, float target, float mu)
		{
			return origin * (1 - mu) + target * mu;
		}
		
		public static Vector2f Lerp(Vector2f origin, Vector2f target, float mu)
		{
			return origin * (1 - mu) + target * mu;
		}

		public static float CosineInterpolate(float origin, float target, float mu)
		{
			float mu2 = (float) (1.0f - Math.Cos(mu * Math.PI)) * 0.5f;
			return Lerp(origin, target, mu2);
		}
		
		public static Vector2f CosineInterpolate(Vector2f origin, Vector2f target, float mu)
		{
			float mu2 = (float) (1.0f - Math.Cos(mu * Math.PI)) * 0.5f;
			return Lerp(origin, target, mu2);
		}
		
		public static Color Lerp(Color origin, Color target, float mu)
		{
			return new Color((byte) Lerp(origin.R, target.R, mu), (byte) Lerp(origin.G, target.G, mu), (byte) Lerp(origin.B, target.B, mu));
		}
		
		public static Color CosineInterpolate(Color origin, Color target, float mu)
		{
			float mu2 = (float) (1.0f - Math.Cos(mu * Math.PI)) * 0.5f;
			return new Color((byte) Lerp(origin.R, target.R, mu2), (byte) Lerp(origin.G, target.G, mu2), (byte) Lerp(origin.B, target.B, mu2));
		}
	}
}