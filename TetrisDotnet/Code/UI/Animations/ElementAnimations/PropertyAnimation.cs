namespace TetrisDotnet.Code.UI.Animations.ElementAnimations
{
	public abstract class PropertyAnimation
	{
		protected readonly float StartTime;
		protected readonly float Duration;
		private bool playedFinalFrame;

		public float EndTime => StartTime + Duration;

		protected PropertyAnimation(float startTime, float duration)
		{
			StartTime = startTime;
			Duration = duration;
		}

		public void Update(float totalElapsedTime)
		{
			float mu =  Duration > 0.0000000001f ? (totalElapsedTime - StartTime) / Duration : 1.0f;
			if (totalElapsedTime <= EndTime && totalElapsedTime > StartTime)
			{
				PlayAnimation(mu);
			}
			else if (totalElapsedTime > EndTime && !playedFinalFrame)
			{
				PlayAnimation(mu);
				playedFinalFrame = true;
			}
		}

		protected abstract void PlayAnimation(float mu);

		public void ResetAnimation()
		{
			playedFinalFrame = false;
		}
	}
}