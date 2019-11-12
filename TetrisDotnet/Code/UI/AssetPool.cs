using SFML.Graphics;
using SFML.System;

namespace TetrisDotnet.Code.UI
{
	public static class AssetPool
	{
		public static Texture[] blockTextures { get; } =
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

		public static Vector2i blockSize { get; } = new Vector2i((int) blockTextures[0].Size.X,
			(int) blockTextures[0].Size.Y);

		public static Texture statsTexture { get; } = new Texture("Art/stats.png");
		private static Texture backDropTexture { get; } = new Texture("Art/background_img.png");
		public static Texture holdTexture { get; } = new Texture("Art/holdbox_Frame.png");
		private static Texture queueTexture { get; } = new Texture("Art/queuebox_Frame.png");
		private static Texture drawGridBackgroundTexture { get; } = new Texture("Art/gridbox_Frame.png");

		public static Sprite backDrop { get; } = new Sprite(backDropTexture);
		public static Sprite queueSprite { get; } = new Sprite(queueTexture);
		public static Sprite holdSprite { get; } = new Sprite(holdTexture);
		public static Sprite drawGridSprite { get; } = new Sprite(drawGridBackgroundTexture);
		public static Sprite statsSprite { get; } = new Sprite(statsTexture);
		
		public static Font font { get; }= new Font("consola.ttf");
	}
}