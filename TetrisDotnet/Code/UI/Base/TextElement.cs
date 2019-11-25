using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.UI.Base.BaseElement;
using TetrisDotnet.Code.Utils.Enums;
using TetrisDotnet.Code.Utils.Extensions;

namespace TetrisDotnet.Code.UI.Base
{
	public class TextElement : AlignableElement
	{
		private List<Text> Texts;

		private string displayedString;

		public string DisplayedString
		{
			get => displayedString;
			set
			{
				displayedString = value;
				SetDirty();
			}
		}

		private Font font;

		public Font Font
		{
			get => font;
			set
			{
				font = value;
				SetDirty();
			}
		}

		private uint characterSize;

		public uint CharacterSize
		{
			get => characterSize;
			set
			{
				characterSize = value;
				SetDirty();
			}
		}

		private uint spacingBetweenLines;

		public uint SpacingBetweenLines
		{
			get => spacingBetweenLines;
			set
			{
				spacingBetweenLines = value;
				SetDirty();
			}
		}

		private Color fillColor;

		public override Color FillColor
		{
			get => fillColor;
			set
			{
				fillColor = value;
				foreach (Text text in Texts)
				{
					text.FillColor = fillColor;
				}
			}
		}

		private bool wrap;

		public bool Wrap
		{
			get => wrap;
			set
			{
				if (wrap != value)
				{
					wrap = value;
					SetDirty();
				}
			}
		}

		public Vector2f FindCharacterPos(uint index)
		{
			int currentLength = 0;
			foreach (Text text in Texts)
			{
				currentLength += text.DisplayedString.Length;
				if (index < currentLength)
				{
					return text.FindCharacterPos(index) + text.Position;
				}

				if (index >= currentLength)
				{
					FloatRect bounds = text.GetGlobalBounds();
					return new Vector2f(bounds.Left + bounds.Width, text.FindCharacterPos(0).Y + text.Position.Y);
				}
			}

			return new Vector2f();
		}

		public TextElement()
		{
			InitializeTextElementDefaults();
		}

		private void InitializeTextElementDefaults()
		{
			Texts = new List<Text>();
			HorizontalAlignment = HorizontalAlignment.Left;
			VerticalAlignment = VerticalAlignment.Top;
			FillColor = Color.Green;
			spacingBetweenLines = 2;
			Wrap = false;
		}

		protected override void SelfDraw(RenderWindow window)
		{
			foreach (Text text in Texts)
			{
				window.Draw(text);
			}

			base.SelfDraw(window);
		}

		protected override void Refresh()
		{
			RecalculateRectangleLeft();
			RecalculateRectangleWidth();

			Texts = new List<Text>
			{
				new Text
				{
					DisplayedString = displayedString,
					Font = font,
					CharacterSize = characterSize,
					FillColor = fillColor
				}
			};

			FloatRect localRect = Texts.First().GetLocalBounds();

			if (Wrap && localRect.Width > Width)
			{
				Texts = GetWrappedTextElements();
			}

			if (VerticalAlignment == VerticalAlignment.Top)
			{
				BottomHeight = Texts.Count * (characterSize + spacingBetweenLines) - spacingBetweenLines;
			}

			RecalculateRectangleTop();
			RecalculateRectangleHeight();

			Dirty = false;

			AlignElement();
		}

		protected override float GetMaxChildHeight()
		{
			return Math.Max(base.GetMaxChildHeight(),
				Texts.Select(text => text.GetGlobalBounds().Top + text.GetGlobalBounds().Height).Max());
		}

		protected override float GetMaxChildWidth()
		{
			return Math.Max(base.GetMaxChildWidth(),
				Texts.Select(text => text.GetGlobalBounds().Left + text.GetGlobalBounds().Width).Max());
		}

		private List<Text> GetWrappedTextElements()
		{
			List<Text> texts = new List<Text>();
			Text currentTextElement = new Text();

			bool isFirstWord = true;

			string[] words = displayedString.Split(' ');

			for (int i = 0; i < words.Length; i++)
			{
				Text tmpTextElement = new Text
				{
					FillColor = fillColor,
					Font = font,
					CharacterSize = characterSize
				};

				if (isFirstWord)
				{
					isFirstWord = false;
					tmpTextElement.DisplayedString = words[i];
				}
				else
				{
					tmpTextElement.DisplayedString = $"{currentTextElement.DisplayedString} {words[i]}";
				}

				if (tmpTextElement.GetLocalBounds().Width > Width)
				{
					isFirstWord = true;
					--i;
					texts.Add(currentTextElement);
				}
				else
				{
					currentTextElement = tmpTextElement;
				}
			}

			return texts;
		}

		protected override void AlignVerticalTop()
		{
			for (int i = 0; i < Texts.Count; i++)
			{
				Texts[i].Origin = new Vector2f(0, i * -(characterSize + spacingBetweenLines));
				Texts[i].Position = new Vector2f(0, Top);
			}
		}

		protected override void AlignVerticalCenter()
		{
			for (int i = 0; i < Texts.Count; i++)
			{
				float offset = 1 + i - Texts.Count * 0.5f;
				Texts[i].Origin = new Vector2f(0, offset * (characterSize + spacingBetweenLines));
				Texts[i].Position = new Vector2f(0, Rectangle.Center().Y);
			}
		}

		protected override void AlignVerticalBottom()
		{
			int posOffset = 0;
			for (int i = Texts.Count - 1; i >= 0; i--)
			{
				Texts[i].Origin = new Vector2f(0, ++posOffset * (characterSize + spacingBetweenLines));
				Texts[i].Position = new Vector2f(0, Top + Height);
			}
		}

		protected override void AlignHorizontalLeft()
		{
			foreach (Text text in Texts)
			{
				text.Origin = new Vector2f(0, text.Origin.Y);
				text.Position = new Vector2f(Left, text.Position.Y);
			}
		}

		protected override void AlignHorizontalCenter()
		{
			foreach (Text text in Texts)
			{
				FloatRect rect = text.GetLocalBounds();
				text.Origin = new Vector2f(rect.Center().X, text.Origin.Y);
				text.Position = new Vector2f(Rectangle.Center().X, text.Position.Y);
			}
		}

		protected override void AlignHorizontalRight()
		{
			foreach (Text text in Texts)
			{
				FloatRect rect = text.GetLocalBounds();
				text.Origin = new Vector2f(rect.Width, text.Origin.Y);
				text.Position = new Vector2f(Left + Width, text.Position.Y);
			}
		}
	}
}