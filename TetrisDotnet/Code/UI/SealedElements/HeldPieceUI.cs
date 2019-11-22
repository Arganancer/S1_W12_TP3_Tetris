using System.Diagnostics;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.UI.Base;
using TetrisDotnet.Code.UI.Base.BaseElement;
using TetrisDotnet.Code.UI.Generics;
using TetrisDotnet.Code.Utils.Enums;

namespace TetrisDotnet.Code.UI.SealedElements
{
	// ReSharper disable once InconsistentNaming
	public sealed class HeldPieceUI : UiElement
	{
		private PieceDisplay heldPieceDisplay;

		public HeldPieceUI()
		{
			Application.EventSystem.Subscribe(EventType.NewHeldPiece, OnNewHeldPiece);

			InitializeUiElements();
		}

		private void InitializeUiElements()
		{
			SpriteElement background = new SpriteElement
			{
				LeftAnchor = 0.0f,
				RightAnchor = 1.0f,
				Texture = AssetPool.HoldTexture,
				StretchToFit = false,
				TopPadding = AssetPool.BlockSize.Y * 2.5f,
				BottomPadding = AssetPool.BlockSize.Y * 2.5f,
				LeftPadding = AssetPool.BlockSize.X * 1.0f,
				RightPadding = AssetPool.BlockSize.X * 1.0f,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
			};

			AddChild(background);

			heldPieceDisplay = new PieceDisplay
			{				
				TopAnchor = 0.0f,
				BottomAnchor = 1.0f,
				LeftAnchor = 0.0f,
				RightAnchor = 1.0f,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
			};
			background.AddChild(heldPieceDisplay);
		}

		~HeldPieceUI()
		{
			Application.EventSystem.Unsubscribe(EventType.NewHeldPiece, OnNewHeldPiece);
		}

		private void OnNewHeldPiece(EventData eventData)
		{
			PieceTypeEventData pieceTypeEventData = eventData as PieceTypeEventData;
			Debug.Assert(pieceTypeEventData != null, nameof(pieceTypeEventData) + " != null");
			heldPieceDisplay.UpdateDisplayedPiece(pieceTypeEventData.PieceType);
		}
	}
}