using SFML.Graphics;
using TetrisDotnet.Code.UI.Base;
using TetrisDotnet.Code.UI.Base.BaseElement;
using TetrisDotnet.Code.Utils;

namespace TetrisDotnet.Code.UI.Animations.ElementAnimations
{
	public class OutlineColorAnimation : PropertyAnimation
	{
		private readonly RectangleElement targetElement;
		private readonly Color originalColor;
		private readonly Color targetColor;

		public OutlineColorAnimation(float startTime, float duration, RectangleElement targetElement, Color targetColor) :
			base(startTime, duration)
		{
			this.targetElement = targetElement;
			originalColor = targetElement.OutlineColor;
			this.targetColor = targetColor;
		}

		protected override void PlayAnimation(float elapsedTime)
		{
			targetElement.OutlineColor =
				Interpolation.Lerp(originalColor, targetColor, elapsedTime / Duration);
		}
	}
}