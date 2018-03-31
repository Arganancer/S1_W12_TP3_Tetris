using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
	class Level : Text
	{

		public Level()
		{

			DisplayedString = "0";
			Font = StaticVars.font;
			Position = new Vector2f(StaticVars.holdSprite.Position.X, StaticVars.holdSprite.Position.Y + StaticVars.holdTexture.Size.Y + 26);
			Color = Color.Green;
			CharacterSize = 16;

			level = 0;
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


		public float dropSpeed { get; set; }

		public float sideMoveSpeed { get; set; }

		public int level { get; set; }

	}
}
