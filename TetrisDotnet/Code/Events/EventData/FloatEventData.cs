namespace TetrisDotnet.Code.Events.EventData
{
	public class FloatEventData : EventData
	{
		public float Value;

		public FloatEventData(float value)
		{
			Value = value;
		}
	}
}