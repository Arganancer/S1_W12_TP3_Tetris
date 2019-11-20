using System.Diagnostics;
using SFML.Graphics;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.UI.Base;
using TetrisDotnet.Code.Utils.Enums;

namespace TetrisDotnet.Code.UI.SealedElements
{
	public sealed class LevelText : UiElement
	{
		private readonly TextElement levelText;

		public LevelText()
		{
			TextElement label = new TextElement
			{
				DisplayedString = "Level:",
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

			levelText = new TextElement
			{
				DisplayedString = "0",
				Font = AssetPool.Font,
				FillColor = Color.Green,
				CharacterSize = 16,
				TopAnchor = 0.0f,
				LeftAnchor = 0.4f,
				RightAnchor = 1.0f,
			};
			AddChild(levelText);


			Application.EventSystem.Subscribe(EventType.LevelUp, OnLevelUp);
		}

		~LevelText()
		{
			Application.EventSystem.Unsubscribe(EventType.LevelUp, OnLevelUp);
		}

		private void OnLevelUp(EventData eventData)
		{
			IntEventData levelUpEventData = eventData as IntEventData;
			Debug.Assert(levelUpEventData != null, nameof(levelUpEventData) + " != null");
			levelText.DisplayedString = $"{levelUpEventData.Value}";
		}
	}
}