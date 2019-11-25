using TetrisDotnet.Code.AI;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.UI.Base;

namespace TetrisDotnet.Code.UI.Generics
{
	public class AiTopHeightTextFieldElement : TextFieldElement
	{
		public AiTopHeightTextFieldElement()
		{
			TextElement.DisplayedString = Evaluator.TopHeightFactor.ToString();
			CurrentValue = TextElement.DisplayedString;
		}
		
		protected override void OnNewValue(float value)
		{
			Application.EventSystem.ProcessEvent(EventType.AiTopHeightUpdated, new FloatEventData(value));
		}
	}
}