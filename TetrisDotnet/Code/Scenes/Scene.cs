using SFML.Graphics;

namespace TetrisDotnet.Code.Scenes
{
	public abstract class Scene
	{
		public SceneType SceneType;

		protected Scene(SceneType sceneType)
		{
			SceneType = sceneType;
		}

		public virtual void Resume()
		{
		}

		public virtual void Pause()
		{
		}

		public abstract SceneType Update(float deltaTime);
		public abstract void Draw(RenderWindow window);
	}
}