using System.Collections.Generic;
using SFML.Graphics;

namespace TetrisDotnet.Code.Scenes
{
	public class SceneManager
	{
		private Scene currentScene;
		private Dictionary<SceneType, Scene> scenes;

		public SceneManager()
		{
			scenes = new Dictionary<SceneType, Scene>
			{
				{SceneType.MainMenu, new MainMenuScene()},
				{SceneType.Game, new GameScene()},
				{SceneType.PauseMenu, new PauseMenuScene()}
			};
			currentScene = scenes[SceneType.MainMenu];
		}

		public void Update(float deltaTime)
		{
			SceneType nextScene = currentScene.Update(deltaTime);
			if (nextScene != currentScene.SceneType)
			{
				currentScene.Pause();
				currentScene = scenes[nextScene];
				currentScene.Resume();
			}
		}

		public void Draw(RenderWindow window)
		{
			currentScene.Draw(window);
		}
	}
}