using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Utils.Extensions;

namespace TetrisDotnet.Code.UI.Base
{
	public class TextElement : UiElement
	{
		private Text text;

		public string DisplayedString
		{
			get => text.DisplayedString;
			set
			{
				text.DisplayedString = value;
				SetDirty();
			}
		}

		public Font Font
		{
			get => text.Font;
			set
			{
				text.Font = value;
				SetDirty();
			}
		}

		public uint CharacterSize
		{
			get => text.CharacterSize;
			set
			{
				text.CharacterSize = value;
				SetDirty();
			}
		}

		public TextElement() : base()
		{
			InitializeTextElement();
		}

		public TextElement(float topAnchor, float bottomAnchor, float leftAnchor, float rightAnchor) : 
			base(topAnchor, bottomAnchor, leftAnchor, rightAnchor)
		{
			InitializeTextElement();
		}

		public TextElement(float topAnchor, float bottomAnchor, float leftAnchor, float rightAnchor, float topHeight, float bottomHeight, float leftWidth, float rightWidth) : 
			base(topAnchor, bottomAnchor, leftAnchor, rightAnchor, topHeight, bottomHeight, leftWidth, rightWidth)
		{
			InitializeTextElement();
		}

		public override void Draw(RenderWindow window)
		{
			if (!Hidden)
			{
				window.Draw(text);
			}

			base.Draw(window);
		}

		private void InitializeTextElement()
		{
			text = new Text();
			SetDirty();
		}

		protected override void Refresh()
		{
			base.Refresh();
			FloatRect localRect = text.GetLocalBounds();
			text.Origin = new Vector2f(localRect.Left + localRect.Width *0.5f,
				localRect.Top + localRect.Height / 0.5f);
			text.Position = Rectangle.Center();
		}
	}
}