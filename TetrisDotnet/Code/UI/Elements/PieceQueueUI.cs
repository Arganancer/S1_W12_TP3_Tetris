using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.Game;
using TetrisDotnet.Code.UI.Base;
using TetrisDotnet.Code.Utils.Enums;

namespace TetrisDotnet.Code.UI.Elements
{
	// ReSharper disable once InconsistentNaming
	public sealed class PieceQueueUI : UiElement
	{
		private readonly PieceDisplay[] pieceDisplays;

		public PieceQueueUI()
		{
			pieceDisplays = new PieceDisplay[3];

			Application.EventSystem.Subscribe(EventType.UpdatedPieceQueue, OnUpdatedPieceQueue);

			InitializeChildren();
		}

		private void InitializeChildren()
		{
			SpriteElement background = new SpriteElement(0.0f, 1.0f, 0.0f, 1.0f)
			{
				LeftAnchor = 0.0f,
				RightAnchor = 1.0f,
				Texture = AssetPool.QueueTexture,
				StretchToFit = false,
				SpriteHorizontalAlignment = HorizontalAlignment.Left,
				TopPadding = AssetPool.BlockSize.Y * 1.5f,
				BottomPadding = AssetPool.BlockSize.Y * 1,
				LeftPadding = AssetPool.BlockSize.X * 1,
				RightPadding = AssetPool.BlockSize.X * 1
			};
			AddChild(background);

			ListContainer container = new ListContainer {Orientation = Orientation.Vertical, Spacing = 0};
			background.AddChild(container);

			for (int i = 0; i < pieceDisplays.Length; i++)
			{
				pieceDisplays[i] = new PieceDisplay(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, AssetPool.BlockSize.Y * 3.0f, 0.0f,
					AssetPool.BlockSize.X * 3.0f);
				container.AddChild(pieceDisplays[i]);
			}
		}

		~PieceQueueUI()
		{
			Application.EventSystem.Unsubscribe(EventType.UpdatedPieceQueue, OnUpdatedPieceQueue);
		}

		private void OnUpdatedPieceQueue(EventData eventData)
		{
			UpdatedPieceQueueEventData updatedPieceQueueEventData = eventData as UpdatedPieceQueueEventData;

			Debug.Assert(updatedPieceQueueEventData != null, nameof(updatedPieceQueueEventData) + " != null");

			ReplaceQueuedBlockSprites(updatedPieceQueueEventData.PieceTypes.ToArray());
		}

		private void ReplaceQueuedBlockSprites(IReadOnlyList<PieceType> pieceTypes)
		{
			for (int i = 0; i < pieceDisplays.Length; i++)
			{
				pieceDisplays[i].UpdateDisplayedPiece(pieceTypes[i]);
			}
		}
	}
}