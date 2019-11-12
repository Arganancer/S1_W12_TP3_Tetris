using System.Collections.Generic;
using SFML.System;
using TetrisDotnet.Code.Utils;

namespace TetrisDotnet.Code.Game.World
{
	class Grid
	{
		public const int GridHeight = 22;
		public const int GridWidth = 10;

		private readonly PieceType[,] grid;

		public Grid()
		{
			grid = new PieceType[GridWidth, GridHeight];
			InitializeGrid();
		}

		// TODO: Fuck this function.
		public PieceType[,] GetDrawable()
		{
			PieceType[,] visibleArray = new PieceType[GridWidth, GridHeight - 2];

			for (int x = 0; x < GridWidth; x++)
			{
				for (int y = 0; y < GridHeight - 2; y++)
				{
					//i + 2 since we only want the grid where we actually see the pieces
					visibleArray[x, y] = grid[x, y + 2];
				}
			}

			return visibleArray;
		}

		public List<int> GetFullRows()
		{
			List<int> fullRows = new List<int>();

			for (int y = 0; y < GridHeight; y++)
			{
				bool isFull = true;

				for (int x = 0; x < GridWidth; x++)
				{
					if (grid[x, y] == PieceType.Dead) continue;
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
					if (grid[x, y] == PieceType.Dead)
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
					grid[x, y] = PieceType.Empty;
				}
			}
		}

		public void KillPiece(Piece piece)
		{
			for (int x = 0; x < 4; x++)
			{
				for (int y = 0; y < 4; y++)
				{
					if (!OutOfRange(new Vector2i(piece.position.X + x, piece.position.Y + y)) &&
					    grid[piece.position.X + x, piece.position.Y + y] == piece.type)
					{
						grid[piece.position.X + x, piece.position.Y + y] = PieceType.Dead;
					}
				}
			}
		}

		public void RemovePiece(Piece piece)
		{
			for (int x = 0; x < 4; x++)
			{
				for (int y = 0; y < 4; y++)
				{
					if (!OutOfRange(new Vector2i(piece.position.X + x, piece.position.Y + y)) &&
					    grid[piece.position.X + x, piece.position.Y + y] == piece.type)
					{
						grid[piece.position.X + x, piece.position.Y + y] = PieceType.Empty;
					}
				}
			}
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
					if (grid[x, y] == PieceType.Dead || grid[x, y] == PieceType.Empty)
					{
						grid[x, y] = grid[x, y - 1];
					}
				}
			}
		}

		public bool CanPlacePiece(Piece piece, Vector2i position)
		{
			foreach (Vector2i block in piece.getGlobalBlocks)
			{
				Vector2i newPosition = block + position;

				if (OutOfRange(newPosition) || grid[newPosition.X, newPosition.Y] == PieceType.Dead)
				{
					return false;
				}
			}

			return true;
		}

		public void AddPiece(Piece piece, Vector2i position)
		{
			piece.position += position;
			foreach (Vector2i block in piece.getGlobalBlocks)
			{
				grid[block.X, block.Y] = piece.type;
			}
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
				return true;
			}

			return false;
		}

		public void AddRotatedPiece(Piece piece, Vector2i backPosition, Rotation rotation)
		{
			piece.Rotate(rotation);
			piece.position += backPosition;
			foreach (Vector2i block in piece.getGlobalBlocks)
			{
				grid[block.X, block.Y] = piece.type;
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
			else if (piece.type == PieceType.T && CheckPiecePositionValid(piece, blocks, DiagLeft))
			{
				backPosition = DiagLeft;
			}
			else if (piece.type == PieceType.T && CheckPiecePositionValid(piece, blocks, DiagRight))
			{
				backPosition = DiagRight;
			}
			else if (CheckPiecePositionValid(piece, blocks, BackUp))
			{
				backPosition = BackUp;
			}
			else if (piece.type == PieceType.I && CheckPiecePositionValid(piece, blocks, LineToRight))
			{
				backPosition = LineToRight;
			}
			else if (piece.type == PieceType.I && CheckPiecePositionValid(piece, blocks, LineToLeft))
			{
				backPosition = LineToLeft;
			}
			else if (piece.type == PieceType.I && CheckPiecePositionValid(piece, blocks, LineDown))
			{
				backPosition = LineDown;
			}
			else if (piece.type == PieceType.I && CheckPiecePositionValid(piece, blocks, LineUp))
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
				Vector2i finalPosition = piece.position + block + backPosition;
				if (OutOfRange(finalPosition) || grid[finalPosition.X, finalPosition.Y] == PieceType.Dead)
				{
					return false;
				}
			}

			return true;
		}
		
		public int CheckLowestPossiblePosition(Piece piece)
		{
			int lowestDownPosition = 0;

			for (; CanPlacePiece(piece, StaticVars.down * lowestDownPosition); lowestDownPosition++)
			{
			}

			return lowestDownPosition - 1;
		}
		
		public void PlaceGhostPiece(Piece piece)
		{
			int ghostPiecePos = CheckLowestPossiblePosition(piece);
			foreach (Vector2i block in piece.getGlobalBlocks)
			{
				Vector2i finalPosition = new Vector2i(block.X, block.Y + ghostPiecePos);
				if (grid[finalPosition.X, finalPosition.Y] == PieceType.Empty)
				{
					grid[finalPosition.X, finalPosition.Y] = PieceType.Ghost;
				}
			}
		}
		
		public int DetermineDropdownPosition(Piece piece)
		{
			RemoveGhostPiece(piece);
			RemovePiece(piece);
			int dropDownPos = CheckLowestPossiblePosition(piece);
			return dropDownPos;
		}
		
		public void RemoveGhostPiece(Piece piece)
		{
			for (int x = 0; x < GridWidth; x++)
			{
				for (int y = 0; y < GridHeight; y++)
				{
					if (grid[x, y] == PieceType.Ghost)
					{
						grid[x, y] = PieceType.Empty;
					}
				}
			}
		}
	}
}