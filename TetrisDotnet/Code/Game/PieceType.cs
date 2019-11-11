using System.Collections.Generic;

namespace TetrisDotnet.Code.Game
{
	public enum PieceType
	{
		I = 0,
		O = 1,
		T = 2,
		S = 3,
		Z = 4,
		J = 5,
		L = 6,
		Empty = 7,
		Ghost = 8,
		Dead = 9
	}

	public static class PieceTypeUtils
	{
		public static IEnumerable<PieceType> GetRealPieces()
		{
			return new[] {PieceType.I, PieceType.O, PieceType.T, PieceType.S, PieceType.Z, PieceType.J, PieceType.L};
		}
	}

	public static class PieceTypeExtensions
	{
		public static bool IsRealPiece(this PieceType pieceType)
		{
			return pieceType <= PieceType.L;
		}
	}
}