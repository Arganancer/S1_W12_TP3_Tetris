namespace TetrisDotnet.Code.UI.Animations.ElementAnimations
{
	public abstract class PropertyAnimation
	{
		protected readonly float StartTime;
		protected readonly float Duration;

		public float EndTime => StartTime + Duration;

		protected PropertyAnimation(float startTime, float duration)
		{
			StartTime = startTime;
			Duration = duration;
		}

		public void Update(float totalElapsedTime)
		{
			float elapsedTime = totalElapsedTime - StartTime;
			if (totalElapsedTime < EndTime && elapsedTime > 0)
			{
				PlayAnimation(elapsedTime);
			}
		}

		protected abstract void PlayAnimation(float elapsedTime);
	}
}