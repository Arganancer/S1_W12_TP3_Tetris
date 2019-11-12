using SFML.Graphics;
using SFML.System;

namespace TetrisDotnet.Code.UI.Elements
{
	public class EndText : Text
	{
		public EndText()
		{
			DisplayedString = "You have lost\npress space\nto reset\nthe game";
			Font = AssetPool.font;

			FloatRect localRect = GetLocalBounds();
			Origin = new Vector2f(localRect.Left + localRect.Width * 0.5f, localRect.Top + localRect.Height * 0.5f);
			Position = new Vector2f(Main.WindowWidth * 0.5f, Main.WindowHeight * 0.5f);
		}
	}
}