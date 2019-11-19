using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace TetrisDotnet.Code.UI
{
	public enum FontFamily
	{
		Audiowide,
		Baloo_Bhaijaan,
		Bowlby_One_Sc,
		Bungee,
		Do_Hyeon,
		Electrolize,
		Graduate,
		Gruppo,
		Kanit,
		Lalezar,
		Odibee_Sans,
		Russo_One,
		Squada_One,
	}
	
	public static class AssetPool
	{
		public static Texture[] BlockTextures { get; } =
		{
			new Texture("Assets/Art/Blocks/placeholder_I.png"),
			new Texture("Assets/Art/Blocks/placeholder_O.png"),
			new Texture("Assets/Art/Blocks/placeholder_T.png"),
			new Texture("Assets/Art/Blocks/placeholder_S.png"),
			new Texture("Assets/Art/Blocks/placeholder_Z.png"),
			new Texture("Assets/Art/Blocks/placeholder_J.png"),
			new Texture("Assets/Art/Blocks/placeholder_L.png"),
			new Texture("Assets/Art/Blocks/placeholder_empty.png"),
			new Texture("Assets/Art/Blocks/placeholder_ghost.png")
		};

		public static Vector2i BlockSize { get; } =
			new Vector2i((int) BlockTextures[0].Size.X, (int) BlockTextures[0].Size.Y);

		public static Texture StatsTexture { get; } = new Texture("Assets/Art/stats.png");
		private static Texture BackDropTexture { get; } = new Texture("Assets/Art/background_img.png");
		public static Texture HoldTexture { get; } = new Texture("Assets/Art/holdbox_Frame.png");
		public static Texture QueueTexture { get; } = new Texture("Assets/Art/queuebox_Frame.png");
		private static Texture DrawGridBackgroundTexture { get; } = new Texture("Assets/Art/gridbox_Frame.png");

		public static Sprite BackDrop { get; } = new Sprite(BackDropTexture);
		public static Sprite QueueSprite { get; } = new Sprite(QueueTexture);
		public static Sprite HoldSprite { get; } = new Sprite(HoldTexture);
		public static Sprite DrawGridSprite { get; } = new Sprite(DrawGridBackgroundTexture);
		public static Sprite StatsSprite { get; } = new Sprite(StatsTexture);

		public static SortedDictionary<FontFamily, Font> Fonts = new SortedDictionary<FontFamily, Font>
		{
			{FontFamily.Audiowide, new Font("Assets/Fonts/Audiowide-Regular.ttf")},
			{FontFamily.Baloo_Bhaijaan, new Font("Assets/Fonts/BalooBhaijaan-Regular.ttf")},
			{FontFamily.Bowlby_One_Sc, new Font("Assets/Fonts/BowlbyOneSc-Regular.ttf")},
			{FontFamily.Bungee, new Font("Assets/Fonts/Bungee-Regular.ttf")},
			{FontFamily.Do_Hyeon, new Font("Assets/Fonts/DoHyeon-Regular.ttf")},
			{FontFamily.Electrolize, new Font("Assets/Fonts/Electrolize-Regular.ttf")},
			{FontFamily.Graduate, new Font("Assets/Fonts/Graduate-Regular.ttf")},
			{FontFamily.Gruppo, new Font("Assets/Fonts/Gruppo-Regular.ttf")},
			{FontFamily.Kanit, new Font("Assets/Fonts/Kanit-Regular.ttf")},
			{FontFamily.Lalezar, new Font("Assets/Fonts/Lalezar-Regular.ttf")},
			{FontFamily.Odibee_Sans, new Font("Assets/Fonts/OdibeeSans-Regular.ttf")},
			{FontFamily.Russo_One, new Font("Assets/Fonts/RussoOne-Regular.ttf")},
			{FontFamily.Squada_One, new Font("Assets/Fonts/SquadaOne-Regular.ttf")},
		};

		public static Font Font { get; } = Fonts[FontFamily.Do_Hyeon];
	}
}