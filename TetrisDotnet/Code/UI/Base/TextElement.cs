using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Utils.Enums;
using TetrisDotnet.Code.Utils.Extensions;

namespace TetrisDotnet.Code.UI.Base
{
	public class TextElement : UiElement
	{
		protected List<Text> Texts;

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
		public Color FillColor
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
		
		private HorizontalAlignment textHorizontalAlignment;
		public HorizontalAlignment TextHorizontalAlignment
		{
			get => textHorizontalAlignment;
			set
			{
				textHorizontalAlignment = value;
				SetDirty();
			}
		}

		private VerticalAlignment textVerticalAlignment;
		public VerticalAlignment TextVerticalAlignment
		{
			get => textVerticalAlignment;
			set
			{
				textVerticalAlignment = value;
				SetDirty();
			}
		}

		public TextElement() : base()
		{
			InitializeTextElementDefaults();
		}

		public TextElement(float topAnchor, float bottomAnchor, float leftAnchor, float rightAnchor) :
			base(topAnchor, bottomAnchor, leftAnchor, rightAnchor)
		{
			InitializeTextElementDefaults();
		}

		public TextElement(float topAnchor, float bottomAnchor, float leftAnchor, float rightAnchor, float topHeight,
			float bottomHeight, float leftWidth, float rightWidth) :
			base(topAnchor, bottomAnchor, leftAnchor, rightAnchor, topHeight, bottomHeight, leftWidth, rightWidth)
		{
			InitializeTextElementDefaults();
		}

		private void InitializeTextElementDefaults()
		{
			Texts = new List<Text>();
			TextHorizontalAlignment = HorizontalAlignment.Left;
			TextVerticalAlignment = VerticalAlignment.Top;
			FillColor = Color.Green;
			spacingBetweenLines = 2;
			Wrap = false;
		}

		public override void Draw(RenderWindow window)
		{
			if (!Hidden)
			{
				foreach (Text text in Texts)
				{
					window.Draw(text);
				}
			}

			base.Draw(window);
		}

		protected override void Refresh()
		{			
			RecalculateRectangleLeft();
			RecalculateRectangleWidth();
			
			Texts = new List<Text>();

			Text defaultText = new Text
			{
				DisplayedString = displayedString,
				Font = font,
				CharacterSize = characterSize,
				FillColor = fillColor
			};

			FloatRect localRect = defaultText.GetLocalBounds();

			if (Wrap && localRect.Width > Rectangle.Width)
			{
				Texts = GetWrappedTextElements();
			}
			else
			{
				Texts.Add(defaultText);
			}

			if (textVerticalAlignment == VerticalAlignment.Top)
			{
				BottomHeight = Texts.Count * (characterSize + spacingBetweenLines) - spacingBetweenLines;
			}

			RecalculateRectangleTop();
			RecalculateRectangleHeight();

			Dirty = false;
			
			AlignTextElements();
		}

		private void AlignTextElements()
		{
			int textCount = Texts.Count;
			switch (TextVerticalAlignment)
			{
				case VerticalAlignment.Top:
					for (int i = 0; i < textCount; i++)
					{
						Texts[i].Origin = new Vector2f(0, i * -(characterSize + spacingBetweenLines));
						Texts[i].Position = new Vector2f(0, Rectangle.Top);
					}
					break;
				case VerticalAlignment.Center:
					for (int i = 0; i < textCount; i++)
					{
						float offset = 1 + i - textCount * 0.5f;
						Texts[i].Origin = new Vector2f(0, offset * (characterSize + spacingBetweenLines));
						Texts[i].Position = new Vector2f(0, Rectangle.Center().Y);
					}
					break;
				case VerticalAlignment.Bottom:
					int posOffset = 0;
					for (int i = textCount - 1; i >= 0; i--)
					{
						Texts[i].Origin = new Vector2f(0, ++posOffset * (characterSize + spacingBetweenLines));
						Texts[i].Position = new Vector2f(0, Rectangle.Top + Rectangle.Height);
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			switch (TextHorizontalAlignment)
			{
				case HorizontalAlignment.Left:
					for (int i = 0; i < textCount; i++)
					{
						Texts[i].Origin = new Vector2f(0, Texts[i].Origin.Y);
						Texts[i].Position = new Vector2f(Rectangle.Left, Texts[i].Position.Y);
					}
					break;
				case HorizontalAlignment.Center:
					for (int i = 0; i < textCount; i++)
					{
						FloatRect rect = Texts[i].GetLocalBounds();
						Texts[i].Origin = new Vector2f(rect.Center().X, Texts[i].Origin.Y);
						Texts[i].Position = new Vector2f(Rectangle.Center().X, Texts[i].Position.Y);
					}
					break;
				case HorizontalAlignment.Right:
					for (int i = 0; i < textCount; i++)
					{
						FloatRect rect = Texts[i].GetLocalBounds();
						Texts[i].Origin = new Vector2f(rect.Width, Texts[i].Origin.Y);
						Texts[i].Position = new Vector2f(Rectangle.Left + Rectangle.Width, Texts[i].Position.Y);
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
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

				if (tmpTextElement.GetLocalBounds().Width > Rectangle.Width)
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
	}
}