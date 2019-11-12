using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Game;

namespace TetrisDotnet.Code.Utils
{
	internal static class StaticVars
	{
		static StaticVars()
		{
			blockTextures = new[]
			{
				new Texture("Art/blocks/placeholder_I.png"),
				new Texture("Art/blocks/placeholder_O.png"),
				new Texture("Art/blocks/placeholder_T.png"),
				new Texture("Art/blocks/placeholder_S.png"),
				new Texture("Art/blocks/placeholder_Z.png"),
				new Texture("Art/blocks/placeholder_J.png"),
				new Texture("Art/blocks/placeholder_L.png"),
				new Texture("Art/blocks/placeholder_empty.png"),
				new Texture("Art/blocks/placeholder_ghost.png")
			};

			//Create the VISIBLE grid of sprites for the game
			drawGrid = new Sprite[10, 20];

			//Basically takes the resolution of the block sprite and uses it for calculations
			imageSize = new Vector2i((int) blockTextures[0].Size.X,
				(int) blockTextures[0].Size.Y);

			gridXPos = Application.WINDOW_WIDTH / 2 -
			           drawGrid.GetLength(0) * imageSize.X / 2; //(int)drawGrid[0,0].Size.X / 2;
			gridYPos = Application.WINDOW_HEIGHT / 2 -
			           drawGrid.GetLength(1) * imageSize.Y / 2; //* (int)drawGrid[0, 0].Size.Y / 2;

			backDropTexture = new Texture("Art/background_img.png");
			holdTexture = new Texture("Art/holdbox_Frame.png");
			queueTexture = new Texture("Art/queuebox_Frame.png");
			drawGridBackgroundTexture = new Texture("Art/gridbox_Frame.png");
			statsTexture = new Texture("Art/stats.png");

			backDrop = new Sprite(backDropTexture);
			queueSprite = new Sprite(queueTexture);
			holdSprite = new Sprite(holdTexture);
			drawGridSprite = new Sprite(drawGridBackgroundTexture);
			statsSprite = new Sprite(statsTexture);

			flat = new Vector2i(0, 0);
			down = new Vector2i(0, 1);
			left = new Vector2i(-1, 0);
			right = new Vector2i(1, 0);

			font = new Font("consola.ttf");

			pauseText = new Text("Paused", font, 40);

			controlsText =
				new Text(
					"←/→ - Move piece\n↑ - Rotate piece\n↓ - Soft Drop\nSpace - Hard Drop\nC - Hold piece\nP - Pause\nEsc/M - Menu",
					font, 20)
				{
					FillColor = Color.Green
				};
		}


		public static int gridXPos { get; }

		public static int gridYPos { get; }

		public static Texture[] blockTextures { get; }

		private static Texture backDropTexture { get; }

		public static Texture holdTexture { get; }

		public static Texture queueTexture { get; }

		private static Texture drawGridBackgroundTexture { get; }

		public static Sprite backDrop { get; }

		public static Sprite queueSprite { get; }

		public static Sprite holdSprite { get; }

		public static Sprite drawGridSprite { get; }

		public static Vector2i flat { get; }

		public static Vector2i down { get; }

		public static Vector2i left { get; }

		public static Vector2i right { get; }

		public static Font font { get; }

		public static Text pauseText { get; }

		public static Sprite[,] drawGrid { get; }

		public static Vector2i imageSize { get; }

		public static Texture statsTexture { get; }

		public static Sprite statsSprite { get; }

		public static Text controlsText { get; }
	}
}