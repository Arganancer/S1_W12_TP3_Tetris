namespace TetrisDotnet.Code.Events.EventData
{
	public class TextEnteredEventData : EventData
	{
		public string Text;

		public TextEnteredEventData(string text)
		{
			Text = text;
		}
	}
}