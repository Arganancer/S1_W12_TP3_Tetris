using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{

	public enum Direction
	{
		Up = -1,
		Down = 1,
		Left,
		Right
	}

	class Menu
	{

		//private static Texture menuBackdropTexture = new Texture("Art/menu_backdrop.png");
		//private static Sprite menuBackdrop = new Sprite(menuBackdropTexture);

		private List<Drawable> elementsToDraw = new List<Drawable>();

		private uint charSize = 60;
		private int menuLength;
		private float charSizeBuffer;
		private Text cursor;
		private string longestWord;
		public int cursorPos { get; private set; }

		public Menu(params string[] menuItems)
		{

			//elementsToDraw.Add(menuBackdrop);

			longestWord = "";

			charSizeBuffer = (float)charSize + (float)((double)charSize * 0.1);

			for (int i = 0; i < menuItems.Length; i++)
			{

				if (menuItems[i].Length > longestWord.Length)
				{

					longestWord = menuItems[i];

				}

				Text text = new Text(menuItems[i], StaticVars.font);

				text.CharacterSize = charSize;

				FloatRect textRect = text.GetLocalBounds();
				text.Origin = new Vector2f(textRect.Left + textRect.Width / 2.0f, textRect.Top + textRect.Height / 2.0f);
				text.Position = new Vector2f(Application.WINDOW_WIDTH / 2, Application.WINDOW_HEIGHT / 2 + (i * charSizeBuffer) - ((menuItems.Length - 1) / 2 * charSizeBuffer));
				text.Color = Color.Green;

				elementsToDraw.Add(text);

			}

			menuLength = menuItems.Length;

			cursorPos = 0;

			cursor = new Text(">", StaticVars.font);

			cursor.CharacterSize = charSize;

			FloatRect cursorRect = cursor.GetLocalBounds();
			cursor.Origin = new Vector2f(cursorRect.Left + cursorRect.Width / 2.0f, cursorRect.Top + cursorRect.Height / 2.0f);
			cursor.Color = Color.Green;

			elementsToDraw.Add(cursor);

			UpdateCursorPos();

		}

		private void UpdateCursorPos()
		{

			cursor.Position = new Vector2f(Application.WINDOW_WIDTH / 2 - longestWord.Length * (charSize / 3), Application.WINDOW_HEIGHT / 2 + (cursorPos * charSizeBuffer) - ((menuLength - 1) / 2 * charSizeBuffer));

			elementsToDraw.RemoveAt(elementsToDraw.Count - 1);
			elementsToDraw.Add(cursor);

		}

		private void LimitCursor()
		{

			if (cursorPos >= menuLength)
			{

				cursorPos = 0;

			}
			else if (cursorPos < 0)
			{

				cursorPos = menuLength - 1;

			}

		}

		public void MoveCursor(Direction direction)
		{

			if (direction == Direction.Up)
			{

				cursorPos += (int)direction;

			}
			else if(direction == Direction.Down)
			{

				cursorPos += (int)direction;

			}

			LimitCursor();

			UpdateCursorPos();

		}

		public List<Drawable> GetDrawable()
		{

			return elementsToDraw;

		}

	}
}