using System;

namespace TetrisDotnet.Code.Utils
{
	public static class Rand
	{
		private static readonly Random Random;

		static Rand()
		{
			Random = new Random();
		}

		public static int Next()
		{
			return Random.Next();
		}

		public static int Next(int maxValue)
		{
			return Random.Next(maxValue);
		}

		public static int Next(int minValue, int maxValue)
		{
			return Random.Next(minValue, maxValue);
		}

		public static double NextDouble()
		{
			return Random.NextDouble();
		}
	}
}