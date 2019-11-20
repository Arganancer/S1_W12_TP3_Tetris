using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Game;
using TetrisDotnet.Code.UI.Base;

namespace TetrisDotnet.Code.UI.Elements
{
	public class PieceDisplay : UiElement
	{
		private List<Sprite> pieceBlocks;
		private PieceType pieceType;
		
		public PieceDisplay()
		{
			InitializePieceDisplay();
		}

		public PieceDisplay(float topAnchor, float bottomAnchor, float leftAnchor, float rightAnchor) : base(topAnchor, bottomAnchor, leftAnchor, rightAnchor)
		{
			InitializePieceDisplay();
		}

		public PieceDisplay(float topAnchor, float bottomAnchor, float leftAnchor, float rightAnchor, float topHeight, float bottomHeight, float leftWidth, float rightWidth) : base(topAnchor, bottomAnchor, leftAnchor, rightAnchor, topHeight, bottomHeight, leftWidth, rightWidth)
		{
			InitializePieceDisplay();
		}

		private void InitializePieceDisplay()
		{
			pieceBlocks = new List<Sprite>();
			pieceType = PieceType.Empty;
		}

		protected override void Refresh()
		{
			base.Refresh();
			UpdateBlocks();
		}

		public void UpdateDisplayedPiece(PieceType pieceType)
		{
			this.pieceType = pieceType;
			UpdateBlocks();
		}

		private void UpdateBlocks()
		{
			pieceBlocks = new List<Sprite>();
			
			float xOffset = pieceType == PieceType.I ? 0.5f : pieceType == PieceType.O ? 0.5f : 1.0f;
			float yOffset = pieceType == PieceType.I ? 1.5f : 1.0f;
				
			foreach (Vector2i position in PieceTypeUtils.GetPieceTypeBlocks(pieceType))
			{
				Sprite sprite = new Sprite
				{
					Texture = AssetPool.BlockTextures[(int) pieceType],
					Position = new Vector2f(
						Rectangle.Left + AssetPool.BlockSize.X * (position.X + xOffset),
						Rectangle.Top + AssetPool.BlockSize.Y * (position.Y + yOffset))
				};
				pieceBlocks.Add(sprite);
			}
		}

		public override void Draw(RenderWindow window)
		{
			foreach (Sprite pieceBlock in pieceBlocks)
			{
				window.Draw(pieceBlock);
			}
			base.Draw(window);
		}
	}
}