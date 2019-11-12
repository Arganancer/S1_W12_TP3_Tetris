using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Utils;

namespace TetrisDotnet.Code.UI.Elements
{
	// TODO: Separate UI from Logic.
	class ScoreText : Text
	{
		private readonly List<int> scoreList = new List<int>();

		public ScoreText()
		{
			DisplayedString = "0";
			Font = StaticVars.font;
			CharacterSize = 16;
			FillColor = Color.Green;
			Position = new Vector2f(StaticVars.holdSprite.Position.X,
				StaticVars.holdSprite.Position.Y + StaticVars.holdTexture.Size.Y + 1);

			score = 0;
		}

		public void AddScore(int scoreToAdd)
		{
			score += scoreToAdd;

			DisplayedString = score.ToString();
		}

		public void CountScore(List<int> fullRows, LevelText levelText)
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
						AddScore(40 * (levelText.level + 1));
						break;
					case 2:
						AddScore(100 * (levelText.level + 1));
						break;
					case 3:
						AddScore(300 * (levelText.level + 1));
						break;
					case 4:
						AddScore(1200 * (levelText.level + 1));
						levelText.LevelUp();
						break;
				}
			}
		}

		public int score { get; private set; }
	}
}