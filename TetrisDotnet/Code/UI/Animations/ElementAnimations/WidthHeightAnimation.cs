using System;
using TetrisDotnet.Code.UI.Base.BaseElement;
using TetrisDotnet.Code.Utils;

namespace TetrisDotnet.Code.UI.Animations.ElementAnimations
{
	public class WidthHeightAnimation : PropertyAnimation
	{
		private readonly UiElement targetElement;
		
		private readonly float originalTopHeight;
		private readonly float originalBottomHeight;
		private readonly float originalLeftWidth;
		private readonly float originalRightWidth;
		
		private readonly float newTopHeight;
		private readonly float newBottomHeight;
		private readonly float newLeftWidth;
		private readonly float newRightWidth;

		public WidthHeightAnimation(float startTime, float duration, UiElement targetElement, float newTopHeight, float newBottomHeight, float newLeftWidth, float newRightWidth) : base(startTime, duration)
		{
			this.targetElement = targetElement;
			
			originalTopHeight = targetElement.TopHeight;
			originalBottomHeight = targetElement.BottomHeight;
			originalLeftWidth = targetElement.LeftWidth;
			originalRightWidth = targetElement.RightWidth;
			
			this.newTopHeight = newTopHeight;
			this.newBottomHeight = newBottomHeight;
			this.newLeftWidth = newLeftWidth;
			this.newRightWidth = newRightWidth;
		}

		protected override void PlayAnimation(float mu)
		{
			targetElement.TopHeight = Interpolation.Lerp(originalTopHeight, newTopHeight, mu);
			targetElement.BottomHeight = Interpolation.Lerp(originalBottomHeight, newBottomHeight, mu);
			targetElement.LeftWidth = Interpolation.Lerp(originalLeftWidth, newLeftWidth, mu);
			targetElement.RightWidth = Interpolation.Lerp(originalRightWidth, newRightWidth, mu);
		}
	}
}