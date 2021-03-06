﻿using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Utils;

namespace TetrisDotnet.Code.UI.Elements
{
	class LevelText : Text
	{
		public LevelText()
		{
			DisplayedString = "Level: 0";
			Font = AssetPool.font;
			Position = new Vector2f(AssetPool.holdSprite.Position.X,
				AssetPool.holdSprite.Position.Y + AssetPool.holdTexture.Size.Y + 26);
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