using System;
using SFML.Graphics;
using SFML.Window;
using TetrisDotnet.Code.Controls;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.Game;
using TetrisDotnet.Code.Scenes;
using TetrisDotnet.Code.Utils;
using EventType = TetrisDotnet.Code.Events.EventType;

namespace TetrisDotnet.Code
{
	class Application
	{
		public static uint WindowHeight { get; private set; } = 768;
		public static uint WindowWidth { get; private set; } = 1024;

		public static EventSystem EventSystem { get; } = new EventSystem();
		private static GameClock GameClock { get; } = new GameClock();

		private InputManager InputManager { get; } = new InputManager();

		private readonly RenderWindow window;
		private readonly SceneManager sceneManager;

		public Application(string title = "Tetris", Styles style = Styles.Default)
		{
			window = new RenderWindow(new VideoMode(WindowWidth, WindowHeight), title, style);

			sceneManager = new SceneManager();

			window.KeyPressed += InputManager.OnKeyPressed;
			window.MouseMoved += InputManager.OnMouseMoved;
			window.MouseButtonPressed += InputManager.OnMouseButtonPressed;
			window.MouseButtonReleased += InputManager.OnMouseButtonReleased;
			window.TextEntered += InputManager.OnTextEntered;
			
			EventSystem.Subscribe(EventType.InputEscape, OnQuit);
			window.Closed += OnWindowClosed;
			window.Resized += OnWindowResized;

			window.SetKeyRepeatEnabled(false);
			window.SetMouseCursorVisible(true);

			window.SetIcon(48, 48, IconGenerator.IconToBytes("Assets/Art/icon.png"));
		}

		private void OnWindowResized(object sender, SizeEventArgs e)
		{
			WindowHeight = e.Height;
			WindowWidth = e.Width;
			window.SetView(new View(new FloatRect(0, 0, WindowWidth, WindowHeight)));
			EventSystem.ProcessEvent(EventType.WindowResized);
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
				sceneManager.Update(GameClock.DeltaTime.AsSeconds());
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