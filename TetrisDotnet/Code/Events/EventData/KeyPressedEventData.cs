using SFML.Window;

namespace TetrisDotnet.Code.Events.EventData
{
	public class KeyPressedEventData : EventData
	{
		public Keyboard.Key Key;

		public KeyPressedEventData(Keyboard.Key key)
		{
			Key = key;
		}
	}
}