using System;
using SFML.System;

namespace TetrisDotnet.Code.Utils
{
	public class GameClock
	{
		private readonly Clock clock;
		public Time deltaTime { get; private set; }
		public float timeElapsed { get; private set; }
		public int fps { get; private set; }

		public GameClock()
		{
			clock = new Clock();
			deltaTime = new Time();
			timeElapsed = 0;
			fps = 0;
		}

		public void Update()
		{
			deltaTime = clock.Restart();

			timeElapsed += deltaTime.AsSeconds();

			//If 1 second has passed in the game
			if (timeElapsed > 1)
			{
				Console.WriteLine($"FPS: {fps}");
				fps = 0;
				timeElapsed = 0;
			}

			++fps;
		}
	}
}