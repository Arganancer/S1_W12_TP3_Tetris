using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet;
using TetrisDotnet.Code;
using TetrisDotnet.Code.UI;
using TetrisDotnet.Code.Utils;

namespace Tetris
{
	class Options
	{
		private string[][] menuItems;
		private List<Text> elementsToDraw = new List<Text>();

		const uint CharSize = 60;
		public int cursorPosX { get; private set; }
		public int cursorPosY { get; private set; }

		public Options()
		{
			int charSizeBuffer = (int) (CharSize * 1.4);

			menuItems = new[]
			{
				new[] {"Music A", "Music B", "Music C"},
				new[] {"Off"},
				new[] {"Change grid size"},
				new[] {"Back"}
			};

			int[] longestWordsLens = new int[menuItems.Length];

			for (int i = 0; i < longestWordsLens.Length; i++)
			{
				for (int j = 0; j < menuItems[i].Length; j++)
				{
					if (longestWordsLens[i] < menuItems[i][j].Length)
					{
						longestWordsLens[i] = menuItems[i][j].Length;
					}
				}
			}

			for (int i = 0; i < menuItems.Length; i++)
			{
				for (int j = 0; j < menuItems[i].Length; j++)
				{
					Text text = new Text(menuItems[i][j], StaticVars.font, CharSize);
					text.Color = Color.Green;

					FloatRect textRect = text.GetLocalBounds();
					text.Origin = new Vector2f(textRect.Left + textRect.Width / 2.0f,
						textRect.Top + textRect.Height / 2.0f);
					text.Position = new Vector2f(
						Application.WINDOW_WIDTH / 2 -
						((menuItems[i].Length - 1) / 2 * (charSizeBuffer / 1.8f * longestWordsLens[i])) +
						(charSizeBuffer / 1.8f * longestWordsLens[i]) * j,
						Application.WINDOW_HEIGHT / 2 - ((menuItems.Length * 0.5f) / 2 * charSizeBuffer) +
						(i * charSizeBuffer));

					elementsToDraw.Add(text);
				}
			}

			cursorPosX = 0;
			cursorPosY = 0;

			SetCursor();
		}

		public void UpdateCursor(Direction direction)
		{
			RemoveCursor();

			if (direction == Direction.Up || direction == Direction.Down)
			{
				cursorPosY += (int) direction;
			}
			else if (direction == Direction.Left || direction == Direction.Right)
			{
				if (direction == Direction.Left)
				{
					cursorPosX += -1;
				}
				else
				{
					cursorPosX += 1;
				}
			}

			LimitCursor();

			SetCursor();
		}

		private void SetCursor()
		{
			int flatArrayLength = 0;

			for (int i = 0; i < cursorPosY; i++)
			{
				flatArrayLength += menuItems[i].Length;
			}

			elementsToDraw[flatArrayLength + cursorPosX].DisplayedString =
				">" + elementsToDraw[flatArrayLength + cursorPosX].DisplayedString;
		}

		private void RemoveCursor()
		{
			int flatArrayLength = 0;

			for (int i = 0; i < cursorPosY; i++)
			{
				flatArrayLength += menuItems[i].Length;
			}

			elementsToDraw[flatArrayLength + cursorPosX].DisplayedString =
				elementsToDraw[flatArrayLength + cursorPosX].DisplayedString.Substring(1);
		}

		private void LimitCursor()
		{
			if (cursorPosY < 0)
			{
				cursorPosY = menuItems.Length - 1;
			}
			else if (cursorPosY > menuItems.Length - 1)
			{
				cursorPosY = 0;
			}

			if (cursorPosX < 0)
			{
				cursorPosX = menuItems[cursorPosY].Length - 1;
			}
			else if (cursorPosX > menuItems[cursorPosY].Length - 1)
			{
				cursorPosX = 0;
			}
		}

		public List<Text> GetDrawable()
		{
			return elementsToDraw;
		}
	}
}