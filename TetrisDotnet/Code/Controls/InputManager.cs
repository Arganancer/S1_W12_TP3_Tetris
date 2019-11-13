using SFML.Window;
using EventType = TetrisDotnet.Code.Events.EventType;

namespace TetrisDotnet.Code.Controls
{
	public class InputManager
	{
		public void OnKeyPressed(object sender, KeyEventArgs e)
		{
			switch (e.Code)
			{
				case Keyboard.Key.Up:
				case Keyboard.Key.W:
					Application.eventSystem.ProcessEvent(EventType.InputRotateClockwise);
					break;
				case Keyboard.Key.E:
					Application.eventSystem.ProcessEvent(EventType.InputRotateCounterClockwise);
					break;
				case Keyboard.Key.Left:
				case Keyboard.Key.A:
					Application.eventSystem.ProcessEvent(EventType.InputLeft);
					break;
				case Keyboard.Key.Down:
				case Keyboard.Key.S:
					Application.eventSystem.ProcessEvent(EventType.InputDown);
					break;
				case Keyboard.Key.Right:
				case Keyboard.Key.D:
					Application.eventSystem.ProcessEvent(EventType.InputRight);
					break;
				case Keyboard.Key.LShift:
				case Keyboard.Key.C:
					Application.eventSystem.ProcessEvent(EventType.InputHold);
					break;
				case Keyboard.Key.Space:
					Application.eventSystem.ProcessEvent(EventType.InputHardDrop);
					break;
				case Keyboard.Key.Escape:
					Application.eventSystem.ProcessEvent(EventType.InputEscape);
					break;
				case Keyboard.Key.P:
					Application.eventSystem.ProcessEvent(EventType.InputPause);
					break;
			}
		}
	}
}