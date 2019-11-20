using SFML.Graphics;
using SFML.System;

namespace TetrisDotnet.Code.UI.SealedElements
{
	public class EndText : Text
	{
		public EndText()
		{
			DisplayedString = "You have lost\npress space\nto reset\nthe game";
			Font = AssetPool.Font;

			FloatRect localRect = GetLocalBounds();
			Origin = new Vector2f(localRect.Left + localRect.Width * 0.5f, localRect.Top + localRect.Height * 0.5f);
			Position = new Vector2f(Application.WindowWidth * 0.5f, Application.WindowHeight * 0.5f);
		}
	}
}