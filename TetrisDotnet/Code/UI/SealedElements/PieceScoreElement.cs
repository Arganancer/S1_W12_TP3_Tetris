using System.Diagnostics;
using SFML.Graphics;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.Game.Stats;
using TetrisDotnet.Code.UI.Base;
using TetrisDotnet.Code.Utils.Enums;

namespace TetrisDotnet.Code.UI.SealedElements
{
	public class PieceScoreElement : ListContainer
	{
		private TextElement softDropScore;
		private TextElement hardDropScore;
		private TextElement comboScore;
		private TextElement linesClearedScore;
		private TextElement difficultyMultiplierScore;
		private TextElement totalScore;

		public PieceScoreElement()
		{
			Application.EventSystem.Subscribe(EventType.PieceScore, OnPieceScoreUpdated);
			InitializeElements();
			Orientation = Orientation.Vertical;
			AutoHeight = true;
		}

		private void InitializeElements()
		{
			totalScore = new TextElement{DisplayedString = "0"};
			AddChild(CreateLabelScoreElement("Total Score:", totalScore));
			linesClearedScore = new TextElement{DisplayedString = "0"};
			AddChild(CreateLabelScoreElement("Move Score:", linesClearedScore));
			softDropScore = new TextElement{DisplayedString = "0"};
			AddChild(CreateLabelScoreElement("Soft Drop:", softDropScore));
			hardDropScore = new TextElement{DisplayedString = "0"};
			AddChild(CreateLabelScoreElement("Hard Drop:", hardDropScore));
			comboScore = new TextElement{DisplayedString = "0"};
			AddChild(CreateLabelScoreElement("Combo Score:", comboScore));
			difficultyMultiplierScore = new TextElement{DisplayedString = "0"};
			AddChild(CreateLabelScoreElement("B2B Multiplier:", difficultyMultiplierScore));
		}

		private ListContainer CreateLabelScoreElement(string labelText, TextElement scoreElement)
		{
			ListContainer listContainer = new ListContainer
			{
				Orientation = Orientation.Horizontal,
				TopAnchor = BottomAnchor = LeftAnchor = 0.0f,
				RightAnchor = 1.0f,
				AutoHeight = true,
			};
			
			listContainer.AddChild(new TextElement
			{
				DisplayedString = labelText,
				Font = AssetPool.Font,
				CharacterSize = 16,
				FillColor = Color.Green,
				TextHorizontalAlignment = HorizontalAlignment.Right,
				TextVerticalAlignment = VerticalAlignment.Center,
				AutoHeight = true,
			});
			
			scoreElement.Font = AssetPool.Font;
			scoreElement.CharacterSize = 16;
			scoreElement.FillColor = Color.Green;
			scoreElement.TextHorizontalAlignment = HorizontalAlignment.Left;
			scoreElement.TextVerticalAlignment = VerticalAlignment.Center;
			scoreElement.AutoHeight = true;
			
			listContainer.AddChild(scoreElement);

			return listContainer;
		}

		~PieceScoreElement()
		{
			Application.EventSystem.Unsubscribe(EventType.PieceScore, OnPieceScoreUpdated);
		}

		private void OnPieceScoreUpdated(EventData eventData)
		{
			PieceScoreEventData pieceScoreEventData = eventData as PieceScoreEventData;
			Debug.Assert(pieceScoreEventData != null, nameof(pieceScoreEventData) + " != null");
			Statistics.PieceScore pieceScore = pieceScoreEventData.PieceScore;

			softDropScore.DisplayedString = $"{pieceScore.SoftDropScore}";
			hardDropScore.DisplayedString = $"{pieceScore.HardDropScore}";
			comboScore.DisplayedString = $"{pieceScore.ComboScore}";
			linesClearedScore.DisplayedString = $"{pieceScore.LinesClearedScore}";
			difficultyMultiplierScore.DisplayedString = $"{pieceScore.DifficultyScoreMultiplier}";
			totalScore.DisplayedString = $"{pieceScore.GetScore()}";
		}
	}
}