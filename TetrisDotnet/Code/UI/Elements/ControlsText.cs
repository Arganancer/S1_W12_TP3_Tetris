using SFML.Graphics;

namespace TetrisDotnet.Code.UI.Elements
{
	public class ControlsText : Text
	{
		public ControlsText()
		{
			DisplayedString =
				"←/→ - Move piece\n↑ - Rotate piece\n↓ - Soft Drop\nSpace - Hard Drop\nC - Hold piece\nP - Pause\nEsc/M - Menu";
			Font = AssetPool.font;
			CharacterSize = 20;
			FillColor = Color.Green;
		}
	}
}