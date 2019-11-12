using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Game.World;

namespace TetrisDotnet.Code.UI.Elements
{
	public class PauseText : Text
	{
		public PauseText()
		{
			DisplayedString = "Paused";
			Font = AssetPool.font;
			CharacterSize = 40;
			
			FloatRect localRect = GetLocalBounds();
			Origin = new Vector2f(localRect.Left + localRect.Width *0.5f,
				localRect.Top + localRect.Height / 0.5f);
			Position = new Vector2f(Main.WindowWidth * 0.5f, Main.WindowHeight * 0.5f);
		}
	}
}