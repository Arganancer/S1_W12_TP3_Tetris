using SFML.System;

namespace TetrisDotnet.Code.Events.EventData
{
	public class MouseMovedEventData : EventData
	{
		public Vector2i MousePosition;

		public MouseMovedEventData(Vector2i mousePosition)
		{
			MousePosition = mousePosition;
		}
	}
}