using System.Collections.Generic;
using SFML.Graphics;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.UI.Base;
using TetrisDotnet.Code.UI.Base.BaseElement;

namespace TetrisDotnet.Code.UI.Layouts
{
	public class UiLayout
	{
		protected readonly List<UiElement> Elements;

		public UiLayout()
		{
			Elements = new List<UiElement>();
			
			Application.EventSystem.Subscribe(EventType.WindowResized, OnWindowResized);
		}
		
		private void OnWindowResized(EventData eventData)
		{
			foreach (UiElement uiElement in Elements)
			{
				uiElement.SetDirty();
			}
		}

		public virtual void Update(float deltaTime)
		{
			foreach (UiElement element in Elements)
			{
				element.Update(deltaTime);
			}
		}

		public virtual void Draw(RenderWindow window)
		{
			foreach (UiElement element in Elements)
			{
				element.Draw(window);
			}
		}
	}
}