using System.Diagnostics;
using SFML.Graphics;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.UI.Base;
using TetrisDotnet.Code.Utils.Enums;

namespace TetrisDotnet.Code.UI.Elements
{
	public sealed class PiecesPlacedText : UiElement
	{
		private readonly TextElement piecesPlayedText;

		public PiecesPlacedText()
		{
			TextElement label = new TextElement
			{
				DisplayedString = "Pieces:",
				Font = AssetPool.Font,
				FillColor = Color.Green,
				CharacterSize = 16,
				TopAnchor = 0.0f,
				LeftAnchor = 0.0f,
				RightAnchor = 0.4f,
				RightWidth = -10.0f,
				TextHorizontalAlignment = HorizontalAlignment.Right
			};
			AddChild(label);

			piecesPlayedText = new TextElement
			{
				DisplayedString = "0",
				Font = AssetPool.Font,
				FillColor = Color.Green,
				CharacterSize = 16,
				TopAnchor = 0.0f,
				LeftAnchor = 0.4f,
				RightAnchor = 1.0f,
			};
			AddChild(piecesPlayedText);


			Application.EventSystem.Subscribe(EventType.PiecesPlacedUpdated, OnPiecesPlacedUpdated);
		}

		~PiecesPlacedText()
		{
			Application.EventSystem.Unsubscribe(EventType.PiecesPlacedUpdated, OnPiecesPlacedUpdated);
		}

		private void OnPiecesPlacedUpdated(EventData eventData)
		{
			IntEventData piecesPlacedEventData = eventData as IntEventData;
			Debug.Assert(piecesPlacedEventData != null, nameof(piecesPlacedEventData) + " != null");
			piecesPlayedText.DisplayedString = $"{piecesPlacedEventData.Value}";
		}
	}
}