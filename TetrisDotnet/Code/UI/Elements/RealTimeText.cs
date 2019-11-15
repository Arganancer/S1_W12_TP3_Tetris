using System;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Utils;

namespace TetrisDotnet.Code.UI.Elements
{
	class RealTimeText : Text
	{
		private float realTime;
		public float RealTime {
			get => realTime;
			set
			{
				realTime = value;
				UpdateTimeString();
			} }

		public RealTimeText()
		{
			DisplayedString = "Time:  00:00:00";
			Font = AssetPool.font;
			CharacterSize = 16;
			FillColor = Color.Green;
			Position = new Vector2f(AssetPool.holdSprite.Position.X,
				AssetPool.holdSprite.Position.Y + AssetPool.holdTexture.Size.Y + 51);

			realTime = 0;
		}

		public void UpdateTimeString()
		{
			DisplayedString = $"Time:  {TimeSpan.FromSeconds(realTime):hh\\:mm\\:ss}";
		}
	}
}