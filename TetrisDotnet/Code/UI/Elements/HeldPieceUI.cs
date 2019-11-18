using System.Collections.Generic;
using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.Game;

namespace TetrisDotnet.Code.UI.Elements
{
	// ReSharper disable once InconsistentNaming
	class HeldPieceUI
	{
		List<Sprite> heldPieceBlocks = new List<Sprite>();
		
		public HeldPieceUI()
		{
			AssetPool.HoldSprite.Position =
				new Vector2f(GridUI.Position.X - AssetPool.HoldTexture.Size.X - (AssetPool.BlockSize.X * 2.25f),
					GridUI.Position.Y);
			Application.EventSystem.Subscribe(EventType.NewHeldPiece, OnNewHeldPiece);
		}

		~HeldPieceUI()
		{
			Application.EventSystem.Unsubscribe(EventType.NewHeldPiece, OnNewHeldPiece);
		}

		private void OnNewHeldPiece(EventData eventData)
		{
			PieceTypeEventData pieceTypeEventData = eventData as PieceTypeEventData;
			
			Debug.Assert(pieceTypeEventData != null, nameof(pieceTypeEventData) + " != null");
			
			ReplaceHeldPieceSprites(pieceTypeEventData.PieceType);
		}

		private void ReplaceHeldPieceSprites(PieceType newPieceType)
		{
			heldPieceBlocks = new List<Sprite>();
			
			List<Vector2i> pieceToDraw = PieceTypeUtils.GetPieceTypeBlocks(newPieceType);

			float xOffset = newPieceType == PieceType.I ? 1.5f : newPieceType == PieceType.O ? 1.5f : 2.0f;
			float yOffset = newPieceType == PieceType.I ? 4.0f : 3.5f;

			foreach (Vector2i position in pieceToDraw)
			{
				heldPieceBlocks.Add(new Sprite
				{
					Texture = AssetPool.BlockTextures[(int) newPieceType],
					Position = new Vector2f(
						AssetPool.HoldSprite.Position.X + AssetPool.BlockSize.X * (xOffset + position.X),
						AssetPool.HoldSprite.Position.Y + AssetPool.BlockSize.Y * (yOffset + position.Y))
				});
			}
		}

		public void Draw(RenderWindow window)
		{
			foreach (Sprite heldPieceBlock in heldPieceBlocks)
			{
				window.Draw(heldPieceBlock);
			}
		}
	}
}