using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.Game;
using TetrisDotnet.Code.Game.World;

namespace TetrisDotnet.Code.UI.SealedElements
{
	// ReSharper disable once InconsistentNaming
	public class GridUI
	{
		public static Vector2f Position { get; private set; }

		private Sprite[,] drawableGrid;

		public GridUI()
		{
			Position = new Vector2f(Application.WindowWidth * 0.5f - Grid.GridWidth * AssetPool.BlockSize.X * 0.5f,
				Application.WindowHeight * 0.5f - Grid.GridHeight * AssetPool.BlockSize.Y * 0.5f);
			
			InitializeGrid();
			
			Application.EventSystem.Subscribe(EventType.GridUpdated, OnGridUpdated);
		}

		~GridUI()
		{
			Application.EventSystem.Unsubscribe(EventType.GridUpdated, OnGridUpdated);
		}

		public void Draw(RenderWindow window)
		{
			for (int x = 0; x < Grid.GridWidth; x++)
			{
				for (int y = 0; y < Grid.VisibleGridHeight; y++)
				{
					window.Draw(drawableGrid[x, y]);
				}
			}
		}
		
		private void OnGridUpdated(EventData eventData)
		{
			GridUpdatedEventData gridUpdatedEventData = eventData as GridUpdatedEventData;

			Debug.Assert(gridUpdatedEventData != null, nameof(gridUpdatedEventData) + " != null");
			
			Grid grid = gridUpdatedEventData.Grid;
			
			for (int x = 0; x < Grid.GridWidth; x++)
			{
				for (int y = 0; y < Grid.VisibleGridHeight; y++)
				{
					PieceType block = grid.GetBlock(x, y + 2).PieceType;
					drawableGrid[x, y].Texture = AssetPool.BlockTextures[(int) block];
				}
			}
		}

		private void InitializeGrid()
		{
			drawableGrid = new Sprite[Grid.GridWidth, Grid.VisibleGridHeight];

			for (int x = 0; x < Grid.GridWidth; x++)
			{
				for (int y = 0; y < Grid.VisibleGridHeight; y++)
				{
					drawableGrid[x, y] = new Sprite
					{
						Position = new Vector2f(x * AssetPool.BlockSize.X + Position.X,
							y * AssetPool.BlockSize.Y + Position.Y + AssetPool.BlockSize.Y),
						Texture = AssetPool.BlockTextures[(int) PieceType.Empty]
					};
				}
			}
		}
	}
}