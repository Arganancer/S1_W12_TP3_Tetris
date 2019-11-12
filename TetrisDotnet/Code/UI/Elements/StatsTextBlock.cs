using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Game;
using TetrisDotnet.Code.Utils;

namespace TetrisDotnet.Code.UI.Elements
{
	class StatsTextBlock
	{
		private readonly int[] counter;
		private int total;
		private readonly List<Text> elementsToDraw = new List<Text>();

		public StatsTextBlock()
		{
			const int charSize = 20;

			counter = new[] {0, 0, 0, 0, 0, 0, 0};

			UpdateTotal();

			for (int i = 0; i < counter.Length; i++)
			{
				Text text = new Text(GenerateString(i), StaticVars.font, charSize)
				{
					FillColor = Color.Green,
					Position = new Vector2f(
						StaticVars.statsSprite.Position.X + StaticVars.statsTexture.Size.X + charSize,
						StaticVars.statsSprite.Position.Y + i * charSize * 2.4f)
				};

				elementsToDraw.Add(text);
			}
		}

		public IEnumerable<Text> GetDrawable()
		{
			return elementsToDraw;
		}

		public void AddToCounter(PieceType type)
		{
			counter[(int) type]++;

			UpdateTotal();

			for (int i = 0; i < elementsToDraw.Count; i++)
			{
				elementsToDraw[i].DisplayedString = GenerateString(i);
			}
		}

		private string GenerateString(int i)
		{
			return $"{counter[i]} {(counter[i] == 0 ? 0 : counter[i] / total * 100)}%";
		}

		private void UpdateTotal()
		{
			// TODO: Not this plz.
			total = counter.Sum();
		}
	}
}