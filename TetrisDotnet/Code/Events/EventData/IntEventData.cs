namespace TetrisDotnet.Code.Events.EventData
{
	public class IntEventData : EventData
	{
		public int Value;
		
		public IntEventData(int value)
		{
			Value = value;
		}
	}
}