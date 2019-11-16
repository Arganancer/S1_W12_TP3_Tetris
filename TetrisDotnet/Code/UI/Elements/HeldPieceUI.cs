using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Game;

namespace TetrisDotnet.Code.UI.Elements
{
	// ReSharper disable once InconsistentNaming
	class HeldPieceUI
	{
		public HeldPieceUI()
		{
			AssetPool.holdSprite.Position =
				new Vector2f(GridUI.position.X - AssetPool.holdTexture.Size.X - (AssetPool.blockSize.X * 2.25f),
					GridUI.position.Y);
		}

		public void Draw(RenderWindow window, Hold hold)
		{
			if (hold.currentPiece != PieceType.Empty)
			{
				List<Vector2i> pieceToDraw = PieceTypeUtils.GetPieceTypeBlocks(hold.currentPiece);

				float xOffset = hold.currentPiece == PieceType.I ? 1.5f : hold.currentPiece == PieceType.O ? 1.5f : 2.0f;
				float yOffset = hold.currentPiece == PieceType.I ? 4.0f : 3.5f;

				foreach (Vector2i position in pieceToDraw)
				{
					Sprite sprite = new Sprite();
					sprite.Texture = AssetPool.blockTextures[(int) hold.currentPiece];
					sprite.Position = new Vector2f(
						AssetPool.holdSprite.Position.X + AssetPool.blockSize.X * (xOffset + position.X),
						AssetPool.holdSprite.Position.Y + AssetPool.blockSize.Y * (yOffset + position.Y));

					window.Draw(sprite);
				}
			}
		}
	}
}