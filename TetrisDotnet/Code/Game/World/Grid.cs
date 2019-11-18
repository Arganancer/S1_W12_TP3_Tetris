using System.Collections.Generic;
using SFML.System;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;
using TetrisDotnet.Code.Utils;
using TetrisDotnet.Code.Utils.Enums;

namespace TetrisDotnet.Code.Game.World
{
	public class Grid
	{
		public const int GridHeight = 22;
		public const int GridWidth = 10;
		public const int VisibleGridHeight = GridHeight - 2;

		public class BlockInfo
		{
			public PieceType PieceType;
			public PieceState PieceState;

			public BlockInfo(PieceType pieceType, PieceState pieceState)
			{
				PieceState = pieceState;
				PieceType = pieceType;
			}

			public static BlockInfo Empty()
			{
				return new BlockInfo(PieceType.Empty, PieceState.None);
			}
		}

		private readonly BlockInfo[,] grid;

		public BlockInfo GetBlock(int x, int y)
		{
			return grid[x, y];
		}

		public Grid()
		{
			grid = new BlockInfo[GridWidth, GridHeight];
			InitializeGrid();
		}

		public bool[,] GetBoolGrid()
		{
			bool[,] boolGrid = new bool[GridWidth, GridHeight];
			
			for (int x = 0; x < GridWidth; x++)
			{
				for (int y = 0; y < GridHeight; y++)
				{
					boolGrid[x, y] = grid[x, y].PieceState == PieceState.Dead;
				}
			}

			return boolGrid;
		}

		public List<int> GetFullRows()
		{
			List<int> fullRows = new List<int>();

			for (int y = 0; y < GridHeight; y++)
			{
				bool isFull = true;

				for (int x = 0; x < GridWidth; x++)
				{
					if (grid[x, y].PieceState == PieceState.Dead) continue;
					isFull = false;
					break;
				}

				if (isFull)
				{
					fullRows.Add(y);
				}
			}

			return fullRows;
		}

		public bool CheckLose()
		{
			for (int y = 0; y < 2; y++)
			{
				for (int x = 0; x < GridWidth; x++)
				{
					if (grid[x, y].PieceState == PieceState.Dead)
					{
						return true;
					}
				}
			}

			return false;
		}

		private void InitializeGrid()
		{
			for (int x = 0; x < GridWidth; x++)
			{
				for (int y = 0; y < GridHeight; y++)
				{
					grid[x, y] = BlockInfo.Empty();
				}
			}
			
			Application.EventSystem.QueueEvent(EventType.GridUpdated, true, new GridUpdatedEventData(this));
		}

		public void KillPiece(Piece piece)
		{
			foreach (Vector2i pos in piece.GetGlobalBlocks)
			{
				grid[pos.X, pos.Y].PieceState = PieceState.Dead;
			}
		}

		public void RemovePiece(Piece piece)
		{
			foreach (Vector2i pos in piece.GetGlobalBlocks)
			{
				grid[pos.X, pos.Y] = BlockInfo.Empty();
			}
			
			Application.EventSystem.QueueEvent(EventType.GridUpdated, true, new GridUpdatedEventData(this));
		}

		private static bool OutOfRange(Vector2i position)
		{
			return position.Y >= GridHeight || position.Y < 0 ||
			       position.X >= GridWidth || position.X < 0;
		}

		public void RemoveRow(int rowIdx)
		{
			for (int y = rowIdx; y > 0; y--)
			{
				for (int x = 0; x < GridWidth; x++)
				{
					if (grid[x, y].PieceState == PieceState.Dead || grid[x, y].PieceType == PieceType.Empty)
					{
						grid[x, y] = grid[x, y - 1];
					}
				}
			}
			
			Application.EventSystem.QueueEvent(EventType.GridUpdated, true, new GridUpdatedEventData(this));
		}

		public bool CanPlacePiece(Piece piece, Vector2i position)
		{
			foreach (Vector2i block in piece.GetGlobalBlocks)
			{
				Vector2i newPosition = block + position;

				if (OutOfRange(newPosition) || grid[newPosition.X, newPosition.Y].PieceState == PieceState.Dead)
				{
					return false;
				}
			}

			return true;
		}

		private void AddPiece(Piece piece, Vector2i position)
		{
			piece.Position += position;
			foreach (Vector2i block in piece.GetGlobalBlocks)
			{
				grid[block.X, block.Y] = new BlockInfo(piece.Type, PieceState.Active);
			}
			
			Application.EventSystem.QueueEvent(EventType.GridUpdated, true, new GridUpdatedEventData(this));
		}

		public void MovePiece(Piece piece, Vector2i newPosition)
		{
			if (CanPlacePiece(piece, newPosition))
			{
				RemoveGhostPiece(piece);
				RemovePiece(piece);

				AddPiece(piece, newPosition);
				PlaceGhostPiece(piece);
			}
		}

		// All of the possible kickback positions. Used in the function "CanRotatePiece".
		private static readonly Vector2i NoKickBack = new Vector2i(0, 0);
		private static readonly Vector2i BackToRight = new Vector2i(1, 0);
		private static readonly Vector2i BackToLeft = new Vector2i(-1, 0);
		private static readonly Vector2i BackDown = new Vector2i(0, 1);
		private static readonly Vector2i DiagLeft = new Vector2i(-1, 1);
		private static readonly Vector2i DiagRight = new Vector2i(1, 1);
		private static readonly Vector2i BackUp = new Vector2i(0, -1);
		private static readonly Vector2i LineToRight = new Vector2i(2, 0);
		private static readonly Vector2i LineToLeft = new Vector2i(-2, 0);
		private static readonly Vector2i LineDown = new Vector2i(0, 2);
		private static readonly Vector2i LineUp = new Vector2i(0, -2);

		public bool RotatePiece(Piece piece, Rotation rotation)
		{
			if (CanRotatePiece(piece, out Vector2i backPosition, rotation))
			{
				RemoveGhostPiece(piece);
				RemovePiece(piece);
				AddRotatedPiece(piece, backPosition, rotation);
				PlaceGhostPiece(piece);
				
				Application.EventSystem.QueueEvent(EventType.GridUpdated, true, new GridUpdatedEventData(this));
				
				return true;
			}

			return false;
		}

		private void AddRotatedPiece(Piece piece, Vector2i backPosition, Rotation rotation)
		{
			piece.Rotate(rotation);
			piece.Position += backPosition;
			foreach (Vector2i block in piece.GetGlobalBlocks)
			{
				grid[block.X, block.Y] = new BlockInfo(piece.Type, PieceState.Active);
			}
		}

		private bool CanRotatePiece(Piece piece, out Vector2i backPosition, Rotation rotation)
		{
			List<Vector2i> blocks = piece.SimulateRotation(rotation);
			if (CheckPiecePositionValid(piece, blocks, NoKickBack))
			{
				backPosition = NoKickBack;
			}
			else if (CheckPiecePositionValid(piece, blocks, BackToRight))
			{
				backPosition = BackToRight;
			}
			else if (CheckPiecePositionValid(piece, blocks, BackToLeft))
			{
				backPosition = BackToLeft;
			}
			else if (CheckPiecePositionValid(piece, blocks, BackDown))
			{
				backPosition = BackDown;
			}
			else if (piece.Type == PieceType.T && CheckPiecePositionValid(piece, blocks, DiagLeft))
			{
				backPosition = DiagLeft;
			}
			else if (piece.Type == PieceType.T && CheckPiecePositionValid(piece, blocks, DiagRight))
			{
				backPosition = DiagRight;
			}
			else if (CheckPiecePositionValid(piece, blocks, BackUp))
			{
				backPosition = BackUp;
			}
			else if (piece.Type == PieceType.I && CheckPiecePositionValid(piece, blocks, LineToRight))
			{
				backPosition = LineToRight;
			}
			else if (piece.Type == PieceType.I && CheckPiecePositionValid(piece, blocks, LineToLeft))
			{
				backPosition = LineToLeft;
			}
			else if (piece.Type == PieceType.I && CheckPiecePositionValid(piece, blocks, LineDown))
			{
				backPosition = LineDown;
			}
			else if (piece.Type == PieceType.I && CheckPiecePositionValid(piece, blocks, LineUp))
			{
				backPosition = LineUp;
			}
			else
			{
				backPosition = NoKickBack;
				return false;
			}

			return true;
		}

		private bool CheckPiecePositionValid(Piece piece, List<Vector2i> blocks, Vector2i backPosition)
		{
			foreach (Vector2i block in blocks)
			{
				Vector2i finalPosition = piece.Position + block + backPosition;
				if (OutOfRange(finalPosition) || grid[finalPosition.X, finalPosition.Y].PieceState == PieceState.Dead)
				{
					return false;
				}
			}

			return true;
		}

		private int FindLowestValidVerticalOffset(Piece piece)
		{
			int lowestDownPosition = 0;

			for (; CanPlacePiece(piece, Vector2iUtils.down * lowestDownPosition); lowestDownPosition++)
			{
			}

			return lowestDownPosition - 1;
		}

		private void PlaceGhostPiece(Piece piece)
		{
			int ghostPiecePos = FindLowestValidVerticalOffset(piece);
			foreach (Vector2i block in piece.GetGlobalBlocks)
			{
				Vector2i finalPosition = new Vector2i(block.X, block.Y + ghostPiecePos);
				if (grid[finalPosition.X, finalPosition.Y].PieceType == PieceType.Empty)
				{
					grid[finalPosition.X, finalPosition.Y].PieceType = PieceType.Ghost;
				}
			}
			
			Application.EventSystem.QueueEvent(EventType.GridUpdated, true, new GridUpdatedEventData(this));
		}
		
		public int DetermineDropdownPosition(Piece piece)
		{
			RemoveGhostPiece(piece);
			RemovePiece(piece);
			int dropDownPos = FindLowestValidVerticalOffset(piece);
			return dropDownPos;
		}

		private void RemoveGhostPiece(Piece piece)
		{
			for (int x = 0; x < GridWidth; x++)
			{
				for (int y = 0; y < GridHeight; y++)
				{
					if (grid[x, y].PieceType == PieceType.Ghost)
					{
						grid[x, y].PieceType = PieceType.Empty;
					}
				}
			}
			
			Application.EventSystem.QueueEvent(EventType.GridUpdated, true, new GridUpdatedEventData(this));
		}
	}
}