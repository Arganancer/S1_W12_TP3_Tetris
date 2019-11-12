using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Game.World;

namespace TetrisDotnet.Code.UI.Elements
{
	// ReSharper disable once InconsistentNaming
	public class GridUI
	{
		public Vector2f position { get; }

		public Sprite[,] DrawableGrid;

		public GridUI()
		{
			position = new Vector2f(Main.WindowWidth * 0.5f - Grid.GridWidth * AssetPool.blockSize.X * 0.5f,
				Main.WindowHeight * 0.5f - Grid.GridHeight * AssetPool.blockSize.Y * 0.5f);
			
			InitializeGrid();
		}

		private void InitializeGrid()
		{
			DrawableGrid = new Sprite[Grid.GridWidth, Grid.GridHeight - 2];

			for (int x = 0; x < Grid.GridWidth; x++)
			{
				for (int y = 0; y < Grid.GridHeight - 2; y++)
				{
					DrawableGrid[x, y] = new Sprite
					{
						Position = new Vector2f(x * AssetPool.blockSize.X + position.X,
							y * AssetPool.blockSize.Y + position.Y + AssetPool.blockSize.Y)
					};
				}
			}
		}
	}
}