using System;
using SFML.Graphics;
using SFML.Window;
using TetrisDotnet.Code.Controls;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.Scenes;
using TetrisDotnet.Code.Utils;
using EventType = TetrisDotnet.Code.Events.EventType;

namespace TetrisDotnet.Code
{
	class Application
	{
		public const int WindowHeight = 768;
		public const int WindowWidth = 1024;

		public static EventSystem EventSystem { get; } = new EventSystem();
		public static GameClock GameClock { get; } = new GameClock();
		
		private InputManager InputManager { get; } = new InputManager();
		
		private readonly RenderWindow window;
		private readonly SceneManager sceneManager;

		public Application(string title = "Tetris", Styles style = Styles.Close)
		{
			window = new RenderWindow(new VideoMode(WindowWidth, WindowHeight), title, style);

			sceneManager = new SceneManager();
			
			window.KeyPressed += InputManager.OnKeyPressed;
			EventSystem.Subscribe(EventType.InputEscape, OnQuit);
			window.Closed += OnWindowClosed;

			window.SetKeyRepeatEnabled(false);
			window.SetMouseCursorVisible(false);

			window.SetIcon(48, 48, IconGenerator.IconToBytes("Assets/Art/icon.png"));
		}

		~Application()
		{
			EventSystem.Unsubscribe(EventType.InputEscape, OnQuit);
		}
		
		public void Run()
		{
			window.SetVisible(true);

			while (window.IsOpen)
			{
				GameClock.Update();
				window.DispatchEvents();
				sceneManager.Update(GameClock.deltaTime.AsSeconds());
				EventSystem.ProcessQueuedEvents();
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

		private void OnQuit(EventData eventData)
		{
			Quit();
		}

		private void OnWindowClosed(object sender, EventArgs e)
		{
			Quit();
		}
	}
}