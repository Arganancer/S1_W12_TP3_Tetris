using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
	class Score : Text
	{

		private List<int> scoreList = new List<int>();

		public Score()
		{

			DisplayedString = "0";
			Font = StaticVars.font;
			CharacterSize = 16;
			Color = Color.Green;
			Position = new Vector2f(StaticVars.holdSprite.Position.X, StaticVars.holdSprite.Position.Y + StaticVars.holdTexture.Size.Y + 1);

			score = 0;

		}

		public void AddScore(int scoreToAdd)
		{

			score += scoreToAdd;

			DisplayedString = score.ToString();

		}

		public void CountScore(List<int> fullRows, Level level)
		{

			scoreList.Clear();

			int currentScoreLevel = 1;

			int last = -1;

			for (int i = 0; i < fullRows.Count; i++)
			{

				if (last == -1)
				{

					last = fullRows[i];

				}
				else if (last != fullRows[i] - 1)
				{

					scoreList.Add(currentScoreLevel);

					currentScoreLevel = 1;

				}
				else
				{

					last = fullRows[i];

					currentScoreLevel++;

				}

			}

			if (last != -1)
			{

				scoreList.Add(currentScoreLevel);

			}

			for (int i = 0; i < scoreList.Count; i++)
			{

				switch (scoreList[i])
				{

					case 1:
						AddScore(40 * (level.level + 1));
						break;
					case 2:
						AddScore(100 * (level.level + 1));
						break;
					case 3:
						AddScore(300 * (level.level + 1));
						break;
					case 4:
						AddScore(1200 * (level.level + 1));
						level.LevelUp();
						break;
					default:
						break;

				}

			}

		}

		public int score { get; private set; }

	}
}
