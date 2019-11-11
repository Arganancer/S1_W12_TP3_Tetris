using System;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Utils;

namespace TetrisDotnet.Code.UI
{
	class RealTimeText : Text
	{
		public RealTimeText()
		{
			DisplayedString = "00:00:00";
			Font = StaticVars.font;
			CharacterSize = 16;
			FillColor = Color.Green;
			Position = new Vector2f(StaticVars.holdSprite.Position.X,
				StaticVars.holdSprite.Position.Y + StaticVars.holdTexture.Size.Y + 51);

			realTime = 0;
		}

		public void UpdateTimeString()
		{
			DisplayedString = TimeSpan.FromSeconds(realTime).ToString(@"hh\:mm\:ss");
		}

		public float realTime { get; set; }
	}
}