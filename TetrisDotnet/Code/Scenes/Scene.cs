using SFML.Graphics;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.UI.Layouts;

namespace TetrisDotnet.Code.Scenes
{
	public abstract class Scene
	{
		public readonly SceneType SceneType;
		protected readonly UiLayout Layout;

		protected Scene(SceneType sceneType, UiLayout layout)
		{
			SceneType = sceneType;
			Layout = layout;
		}

		public virtual void Resume()
		{
		}

		public virtual void Pause()
		{
		}

		public virtual SceneType Update(float deltaTime)
		{
			Layout.Update(deltaTime);
			return SceneType;
		}

		public virtual void Draw(RenderWindow window)
		{
			Layout.Draw(window);
		}
	}
}