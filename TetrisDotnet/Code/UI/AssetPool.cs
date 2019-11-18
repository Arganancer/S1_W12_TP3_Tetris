using SFML.Graphics;
using SFML.System;

namespace TetrisDotnet.Code.UI
{
	public static class AssetPool
	{
		public static Texture[] BlockTextures { get; } =
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

		public static Vector2i BlockSize { get; } = new Vector2i((int) BlockTextures[0].Size.X, (int) BlockTextures[0].Size.Y);

		public static Texture StatsTexture { get; } = new Texture("Art/stats.png");
		private static Texture BackDropTexture { get; } = new Texture("Art/background_img.png");
		public static Texture HoldTexture { get; } = new Texture("Art/holdbox_Frame.png");
		public static Texture QueueTexture { get; } = new Texture("Art/queuebox_Frame.png");
		private static Texture DrawGridBackgroundTexture { get; } = new Texture("Art/gridbox_Frame.png");

		public static Sprite BackDrop { get; } = new Sprite(BackDropTexture);
		public static Sprite QueueSprite { get; } = new Sprite(QueueTexture);
		public static Sprite HoldSprite { get; } = new Sprite(HoldTexture);
		public static Sprite DrawGridSprite { get; } = new Sprite(DrawGridBackgroundTexture);
		public static Sprite StatsSprite { get; } = new Sprite(StatsTexture);
		
		public static Font Font { get; }= new Font("consola.ttf");
	}
}