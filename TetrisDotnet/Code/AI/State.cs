using SFML.System;
using TetrisDotnet.Code.Game;

namespace TetrisDotnet.Code.AI
{
	class State
	{
		public Piece currentPiece { get; }
		public PieceType currentHeldPiece { get; }
		public bool[,] grid { get; }

		public State(Piece currentPiece, bool[,] grid, PieceType currentHeldPiece)
		{
			this.currentPiece = currentPiece;
			this.grid = grid;
			this.currentHeldPiece = currentHeldPiece;
		}

		public bool GetBlock(int x, int y)
		{
			return grid[x, y];
		}

		public bool GetBlock(Vector2i pos)
		{
			return grid[pos.X, pos.Y];
		}
	}
}