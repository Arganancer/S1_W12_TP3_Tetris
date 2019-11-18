using System.Collections.Generic;
using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.Game;
using TetrisDotnet.Code.Game.World;

namespace TetrisDotnet.Code.UI.Elements
{
	// ReSharper disable once InconsistentNaming
	public class PieceQueueUI
	{
		private List<Sprite> queuedPieceBlocks = new List<Sprite>();
		
		public PieceQueueUI()
		{
			AssetPool.QueueSprite.Position =
				new Vector2f(GridUI.Position.X + AssetPool.BlockSize.X * (Grid.GridWidth + 2.25f), GridUI.Position.Y);
			Application.EventSystem.Subscribe(EventType.UpdatedPieceQueue, OnUpdatedPieceQueue);
		}

		~PieceQueueUI()
		{
			Application.EventSystem.Unsubscribe(EventType.UpdatedPieceQueue, OnUpdatedPieceQueue);
		}

		private void OnUpdatedPieceQueue(EventData eventData)
		{
			UpdatedPieceQueueEventData updatedPieceQueueEventData = eventData as UpdatedPieceQueueEventData;
			
			Debug.Assert(updatedPieceQueueEventData != null, nameof(updatedPieceQueueEventData) + " != null");
			
			ReplaceQueuedBlockSprites(updatedPieceQueueEventData.PieceTypes);
		}

		private void ReplaceQueuedBlockSprites(IEnumerable<PieceType> pieceTypes)
		{
			float yHeightModifier = 0;
			
			queuedPieceBlocks = new List<Sprite>();
			
			foreach (PieceType pieceType in pieceTypes)
			{
				float xOffset = pieceType == PieceType.I ? 1.5f : pieceType == PieceType.O ? 1.5f : 2.0f;
				float yOffset = pieceType == PieceType.I ? 3.0f : 2.5f;
				
				foreach (Vector2i position in PieceTypeUtils.GetPieceTypeBlocks(pieceType))
				{
					Sprite sprite = new Sprite
					{
						Texture = AssetPool.BlockTextures[(int) pieceType],
						Position = new Vector2f(
							AssetPool.QueueSprite.Position.X + AssetPool.BlockSize.X * (position.X + xOffset),
							AssetPool.QueueSprite.Position.Y + AssetPool.BlockSize.Y * (position.Y + yOffset) +
							yHeightModifier)
					};
					queuedPieceBlocks.Add(sprite);
				}

				yHeightModifier += AssetPool.BlockSize.Y * 3;
			}
		}

		public void Draw(RenderWindow window)
		{
			foreach (Sprite queuedPieceBlock in queuedPieceBlocks)
			{
				window.Draw(queuedPieceBlock);
			}
		}
	}
}