using TetrisDotnet.Code.AI;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.UI.Base;

namespace TetrisDotnet.Code.UI.Generics
{
	public class AiAggregateHeightTextFieldElement : TextFieldElement
	{
		public AiAggregateHeightTextFieldElement()
		{
			TextElement.DisplayedString = Evaluator.AggregateHeightFactor.ToString();
			CurrentValue = TextElement.DisplayedString;
		}
		
		protected override void OnNewValue(float value)
		{
			Application.EventSystem.ProcessEvent(EventType.AiAggregateHeightUpdated, new FloatEventData(value));
		}
	}
}