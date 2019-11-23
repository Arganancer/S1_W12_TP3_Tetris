using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.Game;
using TetrisDotnet.Code.Game.World;
using TetrisDotnet.Code.UI.Base;
using TetrisDotnet.Code.Utils.Enums;
using TetrisDotnet.Code.Utils.Extensions;

namespace TetrisDotnet.Code.UI.SealedElements
{
	// ReSharper disable once InconsistentNaming
	public sealed class GridUI : SpriteElement
	{
		private Sprite[,] drawableGrid;

		public GridUI()
		{
			Texture = AssetPool.DrawGridBackgroundTexture;
			HorizontalAlignment = HorizontalAlignment.Center;
			VerticalAlignment = VerticalAlignment.Center;
			
			Application.EventSystem.Subscribe(EventType.GridUpdated, OnGridUpdated);
		}

		~GridUI()
		{
			Application.EventSystem.Unsubscribe(EventType.GridUpdated, OnGridUpdated);
		}

		protected override void SelfDraw(RenderWindow window)
		{
			base.SelfDraw(window);
			
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

		protected override void Refresh()
		{
			base.Refresh();
			UpdateGridLayout();
		}

		private void UpdateGridLayout()
		{
			drawableGrid = new Sprite[Grid.GridWidth, Grid.VisibleGridHeight];

			for (int x = 0; x < Grid.GridWidth; x++)
			{
				for (int y = 0; y < Grid.VisibleGridHeight; y++)
				{
					drawableGrid[x, y] = new Sprite
					{
						Position = (Vector2f) new Vector2i((int) ((x - Grid.GridWidth * 0.5f) * AssetPool.BlockSize.X + Rectangle.Center().X),
							(int) ((y - Grid.GridHeight * 0.5f + 1) * AssetPool.BlockSize.Y + Rectangle.Center().Y + AssetPool.BlockSize.Y)),
						Texture = AssetPool.BlockTextures[(int) PieceType.Empty]
					};
				}
			}
		}
	}
}