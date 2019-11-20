using System;
using System.Diagnostics;
using SFML.Graphics;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.UI.Base;
using TetrisDotnet.Code.Utils.Enums;

namespace TetrisDotnet.Code.UI.SealedElements
{
	public sealed class RealTimeText : UiElement
	{
		private readonly TextElement realTimeText;

		public RealTimeText()
		{
			TextElement label = new TextElement
			{
				DisplayedString = "Time:",
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
			realTimeText = new TextElement
			{
				DisplayedString = "00:00:00",
				Font = AssetPool.Font,
				CharacterSize = 16,
				FillColor = Color.Green,
				TopAnchor = 0.0f,
				LeftAnchor = 0.4f,
				RightAnchor = 1.0f,
			};
			AddChild(realTimeText);

			Application.EventSystem.Subscribe(EventType.TimeUpdated, OnRealTimeUpdated);
		}

		~RealTimeText()
		{
			Application.EventSystem.Unsubscribe(EventType.TimeUpdated, OnRealTimeUpdated);
		}

		private void OnRealTimeUpdated(EventData eventData)
		{
			FloatEventData currentTimeInSeconds = eventData as FloatEventData;
			Debug.Assert(currentTimeInSeconds != null, nameof(currentTimeInSeconds) + " != null");
			realTimeText.DisplayedString = $"{TimeSpan.FromSeconds(currentTimeInSeconds.Value):hh\\:mm\\:ss}";
		}
	}
}