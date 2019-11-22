using SFML.System;
using TetrisDotnet.Code.Game;

namespace TetrisDotnet.Code.AI
{
	class State
	{
		public Piece CurrentPiece { get; }
		public PieceType CurrentHeldPiece { get; }
		public bool CanSwap { get; }
		public bool[,] Grid { get; }

		public State(Piece currentPiece, bool[,] grid, PieceType currentHeldPiece, bool canSwap)
		{
			CurrentPiece = currentPiece;
			Grid = grid;
			CurrentHeldPiece = currentHeldPiece;
			CanSwap = canSwap;
		}

		public bool GetBlock(int x, int y)
		{
			return Grid[x, y];
		}

		public bool GetBlock(Vector2i pos)
		{
			return Grid[pos.X, pos.Y];
		}
	}
}