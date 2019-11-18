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
					Application.EventSystem.ProcessEvent(EventType.InputRotateClockwise);
					break;
				case Keyboard.Key.E:
					Application.EventSystem.ProcessEvent(EventType.InputRotateCounterClockwise);
					break;
				case Keyboard.Key.Left:
				case Keyboard.Key.A:
					Application.EventSystem.ProcessEvent(EventType.InputLeft);
					break;
				case Keyboard.Key.Down:
				case Keyboard.Key.S:
					Application.EventSystem.ProcessEvent(EventType.InputDown);
					break;
				case Keyboard.Key.Right:
				case Keyboard.Key.D:
					Application.EventSystem.ProcessEvent(EventType.InputRight);
					break;
				case Keyboard.Key.LShift:
				case Keyboard.Key.C:
					Application.EventSystem.ProcessEvent(EventType.InputHold);
					break;
				case Keyboard.Key.Space:
					Application.EventSystem.ProcessEvent(EventType.InputHardDrop);
					break;
				case Keyboard.Key.Escape:
					Application.EventSystem.ProcessEvent(EventType.InputEscape);
					break;
				case Keyboard.Key.P:
					Application.EventSystem.ProcessEvent(EventType.InputPause);
					break;
			}
		}
	}
}