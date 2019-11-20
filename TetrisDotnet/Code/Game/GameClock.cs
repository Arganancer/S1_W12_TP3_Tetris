using System;
using SFML.System;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;

namespace TetrisDotnet.Code.Game
{
	public class GameClock
	{
		public Time DeltaTime { get; private set; }
		public float TimeElapsed { get; private set; }
		public int Fps { get; private set; }
		
		private readonly Clock clock;
		
		private float timeSinceLastUpdate;
		private const float TimeUpdateInterval = 1.0f;

		public GameClock()
		{
			clock = new Clock();
			DeltaTime = new Time();
			TimeElapsed = 0;
			Fps = 0;
		}

		public void Update()
		{
			DeltaTime = clock.Restart();
			timeSinceLastUpdate += DeltaTime.AsSeconds();
			
			if (timeSinceLastUpdate > TimeUpdateInterval)
			{
				Console.WriteLine($"FPS: {Fps}");
				Fps = 0;
				TimeElapsed += timeSinceLastUpdate;
				timeSinceLastUpdate = 0;
				
				Application.EventSystem.ProcessEvent(EventType.TimeUpdated, new FloatEventData(TimeElapsed));
			}

			++Fps;
		}
	}
}