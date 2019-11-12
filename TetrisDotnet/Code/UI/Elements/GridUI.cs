using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Game.World;

namespace TetrisDotnet.Code.UI.Elements
{
	// ReSharper disable once InconsistentNaming
	public class GridUI
	{
		private Vector2f position;

		public static Sprite[,] DrawableGrid;
		
		public GridUI()
		{
			DrawableGrid = new Sprite[Grid.GridWidth,Grid.GridHeight - 2];
			position = new Vector2f(Main.WindowWidth *0.5f - Grid.GridWidth * AssetPool.blockSize.X * 0.5f,
				Main.WindowHeight *0.5f - Grid.GridHeight * AssetPool.blockSize.Y * 0.5f);
		}
	}
}