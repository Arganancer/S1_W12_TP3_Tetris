using TetrisDotnet.Code.AI;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.UI.Base;

namespace TetrisDotnet.Code.UI.Generics
{
	public class AiNbOfHolesTextFieldElement : TextFieldElement
	{
		public AiNbOfHolesTextFieldElement()
		{
			TextElement.DisplayedString = Evaluator.NbOfHolesFactor.ToString();
			CurrentValue = TextElement.DisplayedString;
		}

		protected override void OnNewValue(float value)
		{
			Application.EventSystem.ProcessEvent(EventType.AiNbOfHolesUpdated, new FloatEventData(value));
		}
	}
}