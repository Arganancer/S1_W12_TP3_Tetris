using SFML.Graphics;

namespace TetrisDotnet.Code.UI.Elements
{
	public class PauseText : Text
	{
		public PauseText()
		{
			DisplayedString = "Paused";
			Font = AssetPool.font;
			CharacterSize = 40;
		}
	}
}