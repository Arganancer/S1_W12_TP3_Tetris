using SFML.Window;

namespace TetrisDotnet.Code.Utils.Extensions
{
	public static class KeyExtensions
	{
		public static bool IsNumeric(this Keyboard.Key key)
		{
			switch (key)
			{
				case Keyboard.Key.Num0:
				case Keyboard.Key.Num1:
				case Keyboard.Key.Num2:
				case Keyboard.Key.Num3:
				case Keyboard.Key.Num4:
				case Keyboard.Key.Num5:
				case Keyboard.Key.Num6:
				case Keyboard.Key.Num7:
				case Keyboard.Key.Num8:
				case Keyboard.Key.Num9:
				case Keyboard.Key.Numpad0:
				case Keyboard.Key.Numpad1:
				case Keyboard.Key.Numpad2:
				case Keyboard.Key.Numpad3:
				case Keyboard.Key.Numpad4:
				case Keyboard.Key.Numpad5:
				case Keyboard.Key.Numpad6:
				case Keyboard.Key.Numpad7:
				case Keyboard.Key.Numpad8:
				case Keyboard.Key.Numpad9:
				case Keyboard.Key.Period:
					return true;
				default:
					return false;
			}
		}
	}
}