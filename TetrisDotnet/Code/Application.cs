using System;
using SFML.Graphics;
using SFML.Window;
using TetrisDotnet.Code.Controls;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Scenes;
using TetrisDotnet.Code.Utils;
using EventType = TetrisDotnet.Code.Events.EventType;

namespace TetrisDotnet.Code
{
	class Application
	{
		public const int WindowHeight = 768;
		public const int WindowWidth = 1024;

		public static EventSystem eventSystem { get; } = new EventSystem();
		public static GameClock gameClock { get; } = new GameClock();
		
		private InputManager inputManager { get; } = new InputManager();
		
		private readonly RenderWindow window;
		private readonly SceneManager sceneManager;

		public Application(string title = "Tetris", Styles style = Styles.Close)
		{
			window = new RenderWindow(new VideoMode(WindowWidth, WindowHeight), title, style);

			sceneManager = new SceneManager();
			
			window.KeyPressed += inputManager.OnKeyPressed;
			eventSystem.Subscribe(EventType.InputEscape, Quit);
			window.Closed += OnWindowClosed;

			window.SetKeyRepeatEnabled(false);
			window.SetMouseCursorVisible(false);

			window.SetIcon(48, 48, IconGenerator.IconToBytes("Art/icon.png"));
		}

		~Application()
		{
			eventSystem.Unsubscribe(EventType.InputEscape, Quit);
		}
		
		public void Run()
		{
			window.SetVisible(true);

			while (window.IsOpen)
			{
				gameClock.Update();
				window.DispatchEvents();
				sceneManager.Update(gameClock.deltaTime.AsSeconds());
				Draw();
			}
		}
		
		private void Draw()
		{
			window.Clear();
			sceneManager.Draw(window);
			window.Display();
		}

		private void Quit()
		{
			window.Close();
		}

		private void OnWindowClosed(object sender, EventArgs e)
		{
			Quit();
		}
	}
}