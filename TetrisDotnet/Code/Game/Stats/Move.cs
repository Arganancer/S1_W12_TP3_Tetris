using System.Linq;

namespace TetrisDotnet.Code.Game.Stats
{
	public enum Move
	{
		None = 0,
		Single = 1,
		Double = 2,
		Triple = 3,
		Tetris = 4,
		TSpinMini,
		TSpin,
	}

	public enum Drop
	{
		SoftDrop,
		HardDrop,
	}

	public static class MoveUtils
	{
		public static Move GetLinesCleared(int linesCleared)
		{
			return (Move) linesCleared;
		}
	}

	public static class MoveExtensions
	{
		public static bool IsDifficult(this Move[] moves)
		{
			return moves.LinesCleared() == Move.Tetris || moves.ContainsTSpin() && moves.LinesCleared() != Move.None;
		}
		
		public static bool ContainsTSpin(this Move[] moves)
		{
			return moves.Contains(Move.TSpin) || moves.Contains(Move.TSpinMini);
		}

		public static Move GetTSpin(this Move[] moves)
		{
			foreach (Move move in moves)
			{
				switch (move)
				{
					case Move.TSpinMini:
					case Move.TSpin:
						return move;
				}
			}

			return Move.None;
		}
		
		public static Move LinesCleared(this Move[] moves)
		{
			foreach (Move move in moves)
			{
				switch (move)
				{
					case Move.Single:
					case Move.Double:
					case Move.Triple:
					case Move.Tetris:
						return move;
				}
			}

			return Move.None;
		}

		public static bool BreaksBackToBackChain(this Move[] moves)
		{
			// Only a Single, Double, or Triple line clear can break a Back-to-Back chain, T-Spin with no lines will not break the chain.
			return !moves.ContainsTSpin() && moves.LinesCleared() > 0 && !moves.Contains(Move.Tetris);
		}

		public static int CountPoints(this Move[] moves, int level, int combo, int backToBackChain)
		{
			int points = 0;
			float backToBackMultiplier = backToBackChain > 0 && !moves.BreaksBackToBackChain() ? 1.5f : 1.0f;

			Move TSpin = moves.GetTSpin();
			Move LinesCleared = moves.LinesCleared();

			switch (TSpin)
			{
				case Move.TSpinMini:
					switch (LinesCleared)
					{
						case Move.None:
							points = 100;
							break;
						case Move.Single:
							points = 200;
							break;
						case Move.Double:
							points = 400;
							break;
					}
					break;
				case Move.TSpin:
					switch (LinesCleared)
					{
						case Move.None:
							points = 400;
							break;
						case Move.Single:
							points = 800;
							break;
						case Move.Double:
							points = 1200;
							break;
						case Move.Triple:
							points = 1600;
							break;
					}
					break;
				case Move.None:
					switch (LinesCleared)
					{
						case Move.Single:
							points = 100;
							break;
						case Move.Double:
							points = 300;
							break;
						case Move.Triple:
							points = 500;
							break;
						case Move.Tetris:
							points = 800;
							break;
					}
					break;
			}

			return (int) (points * backToBackMultiplier) + 50 * combo * level;
		}
	}
}