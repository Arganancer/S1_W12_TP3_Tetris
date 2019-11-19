using System.Collections.Generic;
using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.Game;
using TetrisDotnet.Code.UI.Base;

namespace TetrisDotnet.Code.UI.Elements
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
				BottomPadding = AssetPool.BlockSize.Y * 1.0f,
				LeftPadding = AssetPool.BlockSize.X * 1.0f,
				RightPadding = AssetPool.BlockSize.X * 1.0f
			};
			
			AddChild(background);
			
			heldPieceDisplay = new PieceDisplay(0.0f, 1.0f, 0.0f, 1.0f);
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