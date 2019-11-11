using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Utils;

namespace TetrisDotnet.Code.UI
{
	class LevelText : Text
	{
		public LevelText()
		{
			DisplayedString = "0";
			Font = StaticVars.font;
			Position = new Vector2f(StaticVars.holdSprite.Position.X,
				StaticVars.holdSprite.Position.Y + StaticVars.holdTexture.Size.Y + 26);
			FillColor = Color.Green;
			CharacterSize = 16;

			level = 0;
			// TODO: FUCKING TODO: ULTIMATE TODO:
			dropSpeed = 1f;
			sideMoveSpeed = 0.05f;
		}

		public void LevelUp()
		{
			level++;

			DisplayedString = level.ToString();

			dropSpeed -= dropSpeed / 3;

			sideMoveSpeed -= sideMoveSpeed / 3;
		}


		// TODO: NO.
		public float dropSpeed { get; set; }

		// TODO: DON'T.
		public float sideMoveSpeed { get; set; }

		public int level { get; set; }
	}
}