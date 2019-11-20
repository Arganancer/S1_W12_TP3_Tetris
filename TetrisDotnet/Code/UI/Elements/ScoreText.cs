using System.Diagnostics;
using SFML.Graphics;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.UI.Base;
using TetrisDotnet.Code.Utils.Enums;

namespace TetrisDotnet.Code.UI.Elements
{
	public sealed class ScoreText : UiElement
	{
		private readonly TextElement scoreText;

		public ScoreText()
		{
			TextElement label = new TextElement
			{
				DisplayedString = "Score:",
				Font = AssetPool.Font,
				CharacterSize = 16,
				FillColor = Color.Green,
				TopAnchor = 0.0f,
				LeftAnchor = 0.0f,
				RightAnchor = 0.4f,
				RightWidth = -10.0f,
				TextHorizontalAlignment = HorizontalAlignment.Right
			};
			AddChild(label);
			scoreText = new TextElement
			{
				DisplayedString = "0",
				Font = AssetPool.Font,
				CharacterSize = 16,
				FillColor = Color.Green,
				TopAnchor = 0.0f,
				LeftAnchor = 0.4f,
				RightAnchor = 1.0f,
			};
			AddChild(scoreText);

			Application.EventSystem.Subscribe(EventType.ScoreUpdated, OnScoreUpdated);
		}

		~ScoreText()
		{
			Application.EventSystem.Unsubscribe(EventType.ScoreUpdated, OnScoreUpdated);
		}

		private void OnScoreUpdated(EventData eventData)
		{
			IntEventData scoreUpdatedEventData = eventData as IntEventData;
			Debug.Assert(scoreUpdatedEventData != null, nameof(scoreUpdatedEventData) + " != null");
			scoreText.DisplayedString = $"{scoreUpdatedEventData.Value.ToString()}";
		}
	}
}