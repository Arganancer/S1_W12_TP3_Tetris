using TetrisDotnet.Code.AI;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.Scenes;
using TetrisDotnet.Code.UI.Base;

namespace TetrisDotnet.Code.UI.Generics
{
	public class AiTickSpeedTextFieldElement : TextFieldElement
	{
		public AiTickSpeedTextFieldElement()
		{
			TextElement.DisplayedString = GameScene.AiTickInterval.ToString();
			CurrentValue = TextElement.DisplayedString;
		}

		protected override void OnNewValue(float value)
		{
			Application.EventSystem.ProcessEvent(EventType.AiTickSpeedUpdated, new FloatEventData(value));
		}
	}
}