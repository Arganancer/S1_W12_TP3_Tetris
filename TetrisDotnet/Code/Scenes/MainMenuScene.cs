using SFML.Graphics;
using TetrisDotnet.Code.UI;
using TetrisDotnet.Code.UI.Layouts;

namespace TetrisDotnet.Code.Scenes
{
	public class MainMenuScene : Scene
	{
		private readonly UiLayout uiLayout;
		public MainMenuScene() : base(SceneType.MainMenu, new MenuLayout())
		{
			uiLayout = new UiLayout();
		}
		
		public override SceneType Update(float deltaTime)
		{
			uiLayout.Update(deltaTime);
			return SceneType.MainMenu;
		}

		public override void Draw(RenderWindow window)
		{
			uiLayout.Draw(window);
		}
	}
}