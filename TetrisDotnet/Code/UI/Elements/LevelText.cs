using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Utils;

namespace TetrisDotnet.Code.UI.Elements
{
	class LevelText : Text
	{
		public LevelText()
		{
			DisplayedString = "Level: 0";
			Font = AssetPool.Font;
			Position = new Vector2f(AssetPool.HoldSprite.Position.X,
				AssetPool.HoldSprite.Position.Y + AssetPool.HoldTexture.Size.Y + 26);
			FillColor = Color.Green;
			CharacterSize = 16;

			level = 0;
			dropSpeed = 1f;
			sideMoveSpeed = 0.05f;
		}

		public void LevelUp()
		{
			level++;

			DisplayedString = $"Level: {level.ToString()}";

			dropSpeed -= dropSpeed * 0.1f;

			sideMoveSpeed -= sideMoveSpeed / 3;
		}
		
		public float dropSpeed { get; set; }

		public float sideMoveSpeed { get; set; }

		public int level { get; set; }
	}
}