using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Game;
using TetrisDotnet.Code.Game.World;

namespace TetrisDotnet.Code.UI.Elements
{
	// ReSharper disable once InconsistentNaming
	class QueuedPiecesUI
	{
		public QueuedPiecesUI()
		{
			AssetPool.queueSprite.Position =
				new Vector2f(GridUI.position.X + AssetPool.blockSize.X * (Grid.GridWidth + 2.25f), GridUI.position.Y);
		}

		public void Draw(RenderWindow window, PieceQueue pieceQueue)
		{
			float yHeightModifier = 0;
			foreach (PieceType pieceType in pieceQueue.Get())
			{
				float xOffset = pieceType == PieceType.I ? 1.5f : pieceType == PieceType.O ? 1.5f : 2.0f;
				float yOffset = pieceType == PieceType.I ? 3.0f : 2.5f;
				
				foreach (Vector2i position in PieceTypeUtils.GetPieceTypeBlocks(pieceType))
				{
					Sprite sprite = new Sprite();
					sprite.Texture = AssetPool.blockTextures[(int) pieceType];
					sprite.Position = new Vector2f(
						AssetPool.queueSprite.Position.X + AssetPool.blockSize.X * (position.X + xOffset),
						AssetPool.queueSprite.Position.Y + AssetPool.blockSize.Y * (position.Y + yOffset) + yHeightModifier);
					
					window.Draw(sprite);
				}

				yHeightModifier += AssetPool.blockSize.Y * 3;
			}
		}
	}
}