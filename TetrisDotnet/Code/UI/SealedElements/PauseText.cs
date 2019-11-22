using System.Diagnostics;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.UI.Base;
using TetrisDotnet.Code.Utils.Enums;

namespace TetrisDotnet.Code.UI.SealedElements
{
	public class PauseText : TextElement
	{
		public PauseText()
		{
			TopAnchor = 0.5f;
			BottomAnchor = 0.5f;
			LeftAnchor = 0.5f;
			RightAnchor = 0.5f;
			DisplayedString = "Paused";
			Font = AssetPool.Font;
			CharacterSize = 40;
			Hidden = true;
			HorizontalAlignment = HorizontalAlignment.Center;
			VerticalAlignment = VerticalAlignment.Center;

			Application.EventSystem.Subscribe(EventType.GamePauseToggled, OnGamePauseToggled);
		}

		~PauseText()
		{
			
			Application.EventSystem.Unsubscribe(EventType.GamePauseToggled, OnGamePauseToggled);
		}

		private void OnGamePauseToggled(EventData eventData)
		{
			GamePauseToggledEventData gamePauseToggledEventData = eventData as GamePauseToggledEventData;
			
			Debug.Assert(gamePauseToggledEventData != null, nameof(gamePauseToggledEventData) + " != null");
			
			Hidden = !gamePauseToggledEventData.IsPaused;
		}
	}
}