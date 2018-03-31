using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
	class Stats
	{
		private int[] counter;
		private int total;
		private List<Text> elementsToDraw = new List<Text>();
		private int charSize;

		public Stats()
		{

			charSize = 20;

			counter = new int[] { 0, 0, 0, 0, 0, 0, 0 };

			UpdateTotal();

			for (int i = 0; i < counter.Length; i++)
			{

				Text text = new Text(GenerateString(i),StaticVars.font, (uint)charSize);

				text.Color = Color.Green;

				text.Position = new Vector2f(StaticVars.statsSprite.Position.X + StaticVars.statsTexture.Size.X + charSize, StaticVars.statsSprite.Position.Y + i * charSize * 2.4f);

				elementsToDraw.Add(text);

			}

		}

		public List<Text> GetDrawable()
		{

			return elementsToDraw;

		}

		public void AddToCounter(PieceType type)
		{

			counter[(int)type]++;

			UpdateTotal();

			for (int i = 0; i < elementsToDraw.Count; i++)
			{
				
				elementsToDraw[i].DisplayedString = GenerateString(i);

			}


		}

		private string GenerateString(int i)
		{

			return counter[i] + " " + ((counter[i] == 0) ? 0 : (int)(((double)counter[i] / total) * 100)) + "%";

		}

		private void UpdateTotal()
		{

			total = counter.Sum();

		}

	}
}
