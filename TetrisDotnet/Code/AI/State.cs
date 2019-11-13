using SFML.System;
using TetrisDotnet.Code.Game;

namespace TetrisDotnet.Code.AI
{
	class State
	{
		public Piece currentPiece { get; }
		public bool[,] grid { get; }

		public State(Piece currentPiece, bool[,] grid)
		{
			this.currentPiece = currentPiece;
			this.grid = grid;
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