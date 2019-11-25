using TetrisDotnet.Code.AI;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.UI.Base;

namespace TetrisDotnet.Code.UI.Generics
{
	public class AiBumpinessTextFieldElement : TextFieldElement
	{
		public AiBumpinessTextFieldElement()
		{
			TextElement.DisplayedString = Evaluator.BumpinessFactor.ToString();
			CurrentValue = TextElement.DisplayedString;
		}

		protected override void OnNewValue(float value)
		{
			Application.EventSystem.ProcessEvent(EventType.AiBumpinessUpdated, new FloatEventData(value));
		}
	}
}