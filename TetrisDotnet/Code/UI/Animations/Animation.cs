using System.Collections.Generic;
using System.Linq;
using TetrisDotnet.Code.UI.Animations.ElementAnimations;

namespace TetrisDotnet.Code.UI.Animations
{
	public class Animation
	{
		private readonly List<PropertyAnimation> elementAnimations;

		private float elapsedTime;
		private float Duration => elementAnimations.Max(eAnim => eAnim.EndTime);

		public bool Finished => elapsedTime > Duration;

		public Animation()
		{
			elementAnimations = new List<PropertyAnimation>();
		}

		public void Update(float deltaTime)
		{
			elapsedTime += deltaTime;
			foreach (PropertyAnimation elementAnimation in elementAnimations)
			{
				elementAnimation.Update(elapsedTime);
			}
		}

		public void AddElementAnimation(PropertyAnimation propertyAnimation)
		{
			elementAnimations.Add(propertyAnimation);
		}
	}
}