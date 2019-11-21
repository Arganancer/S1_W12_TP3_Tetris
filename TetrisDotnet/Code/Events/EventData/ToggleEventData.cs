namespace TetrisDotnet.Code.Events.EventData
{
	public class ToggleEventData : EventData
	{
		public bool IsOn;

		public ToggleEventData(bool isOn)
		{
			IsOn = isOn;
		}
	}
}