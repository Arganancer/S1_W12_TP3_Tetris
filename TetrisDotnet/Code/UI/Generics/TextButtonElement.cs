using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.UI.Base;
using TetrisDotnet.Code.Utils.Enums;

namespace TetrisDotnet.Code.UI.Generics
{
	public class TextButtonElement : ButtonElement
	{
		private readonly RectangleShape background;

		private Color lightColor = new Color(38, 209, 0);
		private Color mediumColor = new Color(29, 158, 0);
		private Color darkColor = new Color(20, 107, 0);

		private readonly Color lightFadeColor = new Color(169, 204, 161);
		private readonly Color mediumFadeColor = new Color(116, 148, 111);
		private Color darkFadeColor = new Color(81, 99, 77);

		private bool aiIsActive = true;

		protected TextElement TextElement;

		public string DisplayedString
		{
			get => TextElement.DisplayedString;
			set => TextElement.DisplayedString = value;
		}

		public TextButtonElement()
		{
			background = new RectangleShape {FillColor = darkColor};
			background.OutlineColor = mediumColor;
			TextElement = new TextElement
			{
				CharacterSize = 16,
				Font = AssetPool.Font,
				FillColor = mediumColor,
				TextHorizontalAlignment = HorizontalAlignment.Center,
				TextVerticalAlignment = VerticalAlignment.Center,
			};
			AddChild(TextElement);
		}

		protected override void OnButtonDown()
		{			
			TextElement.FillColor = aiIsActive ? mediumColor : mediumFadeColor;
			background.FillColor = aiIsActive ? darkColor : darkFadeColor;
			background.OutlineThickness = 1;
			background.OutlineColor = aiIsActive ? lightColor : lightFadeColor;
			base.OnButtonDown();
		}

		protected override void OnButtonPressed()
		{
			aiIsActive = !aiIsActive;
			Application.EventSystem.ProcessEvent(EventType.ToggleAi, new ToggleEventData(aiIsActive));
			DisplayedString = $"Ai is {(aiIsActive ? "On" : "Off")}";
			base.OnButtonPressed();
		}

		protected override void OnButtonReleased()
		{
			if (MouseInside)
			{
				TextElement.FillColor = aiIsActive ? lightColor : lightFadeColor;
				background.FillColor = aiIsActive ? mediumColor : mediumFadeColor;
				background.OutlineThickness = 1;
				background.OutlineColor = aiIsActive ? lightColor : lightFadeColor;
			}
			else
			{
				TextElement.FillColor = aiIsActive ? mediumColor : mediumFadeColor;
				background.FillColor = aiIsActive ? darkColor : darkFadeColor;
				background.OutlineThickness = -1;
				background.OutlineColor = aiIsActive ? mediumColor : mediumFadeColor;
			}
			base.OnButtonReleased();
		}

		protected override void OnMouseEnter()
		{
			if (!ButtonDown)
			{
				TextElement.FillColor = aiIsActive ? lightColor : lightFadeColor;
				background.FillColor = aiIsActive ? mediumColor : mediumFadeColor;
				background.OutlineThickness = 1;
				background.OutlineColor = aiIsActive ? lightColor : lightFadeColor;
			}

			base.OnMouseEnter();
		}

		protected override void OnMouseExit()
		{			
			if (!ButtonDown)
			{
				TextElement.FillColor = aiIsActive ? mediumColor : mediumFadeColor;
				background.FillColor = aiIsActive ? darkColor : darkFadeColor;
				background.OutlineThickness = -1;
				background.OutlineColor = aiIsActive ? mediumColor : mediumFadeColor;
			}
			base.OnMouseExit();
		}

		protected override void Refresh()
		{
			base.Refresh();
			background.Position = new Vector2f(Rectangle.Left, Rectangle.Top);
			background.Size = new Vector2f(Rectangle.Width, Rectangle.Height);
			background.OutlineThickness = -1;
		}

		public override void Draw(RenderWindow window)
		{
			window.Draw(background);
			base.Draw(window);
		}
	}
}