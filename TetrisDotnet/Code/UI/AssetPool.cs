using SFML.Graphics;

namespace TetrisDotnet.Code.UI
{
	public static class AssetPool
	{
		public static Texture[] blockTextures { get; } = new[]
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
	}
}