using System.Collections.Generic;
using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.UI.Base;
using TetrisDotnet.Code.Utils;

namespace TetrisDotnet.Code.UI.Elements
{
	class ScoreText : TextElement
	{
		public ScoreText()
		{
			DisplayedString = "Score: 0";
			Font = AssetPool.Font;
			CharacterSize = 16;
			FillColor = Color.Green;
			Hidden = false;

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
			
			DisplayedString = $"Score: {scoreUpdatedEventData.Value.ToString()}";
		}
	}
}