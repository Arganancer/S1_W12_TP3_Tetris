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
					Main.eventSystem.ProcessEvent(EventType.InputRotateClockwise);
					break;
				case Keyboard.Key.E:
					Main.eventSystem.ProcessEvent(EventType.InputRotateCounterClockwise);
					break;
				case Keyboard.Key.Left:
				case Keyboard.Key.A:
					Main.eventSystem.ProcessEvent(EventType.InputLeft);
					break;
				case Keyboard.Key.Down:
				case Keyboard.Key.S:
					Main.eventSystem.ProcessEvent(EventType.InputDown);
					break;
				case Keyboard.Key.Right:
				case Keyboard.Key.D:
					Main.eventSystem.ProcessEvent(EventType.InputRight);
					break;
				case Keyboard.Key.LShift:
				case Keyboard.Key.C:
					Main.eventSystem.ProcessEvent(EventType.InputHold);
					break;
				case Keyboard.Key.Space:
					Main.eventSystem.ProcessEvent(EventType.InputHardDrop);
					break;
				case Keyboard.Key.Escape:
					Main.eventSystem.ProcessEvent(EventType.InputEscape);
					break;
				case Keyboard.Key.P:
					Main.eventSystem.ProcessEvent(EventType.InputPause);
					break;
			}
		}
	}
}