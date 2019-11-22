using System.Collections.Generic;
using TetrisDotnet.Code.UI.Animations;

namespace TetrisDotnet.Code.UI.Base.BaseElement
{
	public partial class UiElement
	{
		private readonly Queue<Animation> animations = new Queue<Animation>();
		private Animation currentAnimation;

		/// <summary>
		/// Adds the given animation to the end of the animations queue.
		/// </summary>
		protected void QueueAnimation(Animation animation)
		{
			animations.Enqueue(animation);
		}

		/// <summary>
		/// Squashes all previously enqueued animations and plays the given one instead.
		/// </summary>
		protected void PlayAnimation(Animation animation)
		{
			animations.Clear();
			
			currentAnimation = animation;
		}

		private void UpdateAnimations(float deltaTime)
		{
			if (animations.Count > 0)
			{
				if (currentAnimation == null || currentAnimation.Finished)
				{
					currentAnimation = animations.Dequeue();
				}
			}
			else if (currentAnimation != null && currentAnimation.Finished)
			{
				currentAnimation = null;
			}

			currentAnimation?.Update(deltaTime);
		}
	}
}