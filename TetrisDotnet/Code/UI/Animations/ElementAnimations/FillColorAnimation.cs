using SFML.Graphics;
using TetrisDotnet.Code.UI.Base.BaseElement;
using TetrisDotnet.Code.Utils;

namespace TetrisDotnet.Code.UI.Animations.ElementAnimations
{
	public class FillColorAnimation : PropertyAnimation
	{
		private readonly DrawableElement targetElement;
		private readonly Color originalColor;
		private readonly Color targetColor;
		
		public FillColorAnimation(float startTime, float duration, DrawableElement targetElement, Color targetColor) : base(startTime, duration)
		{
			this.targetElement = targetElement;
			originalColor = targetElement.FillColor;
			this.targetColor = targetColor;
		}

		protected override void PlayAnimation(float elapsedTime)
		{
			targetElement.FillColor = Interpolation.CosineInterpolate(originalColor, targetColor, elapsedTime / Duration);
		}
	}
}