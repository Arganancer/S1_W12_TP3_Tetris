using SFML.System;
using SFML.Window;

namespace TetrisDotnet.Code.Events.EventData
{
	public class MouseButtonEventData : EventData
	{
		public bool IsPressed;
		public Vector2i MousePosition;
		public Mouse.Button MouseButton;

		public MouseButtonEventData(bool isPressed, Vector2i mousePosition, Mouse.Button mouseButton)
		{
			IsPressed = isPressed;
			MousePosition = mousePosition;
			MouseButton = mouseButton;
		}
	}
}