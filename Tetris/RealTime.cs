using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
	class RealTime : Text
	{

		public RealTime()
		{

			DisplayedString = "00:00:00";
			Font = StaticVars.font;
			CharacterSize = 16;
			Color = Color.Green;
			Position = new Vector2f(StaticVars.holdSprite.Position.X, StaticVars.holdSprite.Position.Y + StaticVars.holdTexture.Size.Y + 51);

			realTime = 0;

		}

		public void UpdateTimeString()
		{

			DisplayedString = TimeSpan.FromSeconds(realTime).ToString(@"hh\:mm\:ss");

		}

		public float realTime { get; set; }

	}
}
