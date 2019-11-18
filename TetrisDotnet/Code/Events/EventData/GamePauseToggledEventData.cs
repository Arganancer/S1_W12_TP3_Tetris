namespace TetrisDotnet.Code.Events.EventData
{
	public class GamePauseToggledEventData : EventData
	{
		public bool IsPaused;

		public GamePauseToggledEventData(bool isPaused)
		{
			IsPaused = isPaused;
		}
	}
}