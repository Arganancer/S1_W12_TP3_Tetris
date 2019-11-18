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
			Font = AssetPool.Font;
			CharacterSize = 16;
			FillColor = Color.Green;
			Position = new Vector2f(AssetPool.HoldSprite.Position.X,
				AssetPool.HoldSprite.Position.Y + AssetPool.HoldTexture.Size.Y + 51);

			realTime = 0;
		}

		public void UpdateTimeString()
		{
			DisplayedString = $"Time:  {TimeSpan.FromSeconds(realTime):hh\\:mm\\:ss}";
		}
	}
}