using System.Collections.Generic;
using SFML.Graphics;
using TetrisDotnet.Code.UI.Base;

namespace TetrisDotnet.Code.UI.Layouts
{
	public class UiLayout
	{
		protected readonly List<UiElement> Elements;

		public UiLayout()
		{
			Elements = new List<UiElement>();
		}

		public void Update()
		{
			foreach (UiElement element in Elements)
			{
				element.Update();
			}
		}

		public void Draw(RenderWindow window)
		{
			foreach (UiElement element in Elements)
			{
				element.Draw(window);
			}
		}
	}
}