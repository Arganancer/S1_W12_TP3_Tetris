using System.Diagnostics;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.UI.Base;

namespace TetrisDotnet.Code.UI.Elements
{
	public class PauseText : TextElement
	{
		public PauseText() : base(0.5f, 0.5f, 0.5f, 0.5f)
		{
			DisplayedString = "Paused";
			Font = AssetPool.Font;
			CharacterSize = 40;
			Hidden = true;

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