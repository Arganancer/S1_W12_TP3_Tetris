using SFML.Graphics;
using TetrisDotnet.Code.UI.Layouts;

namespace TetrisDotnet.Code.Scenes
{
	public class OptionsMenuScene : Scene
	{
		public OptionsMenuScene() : base(SceneType.OptionsMenu, new OptionsLayout())
		{
			
		}

		public override SceneType Update(float deltaTime)
		{
			throw new System.NotImplementedException();
		}

		public override void Draw(RenderWindow window)
		{
			throw new System.NotImplementedException();
		}
	}
}