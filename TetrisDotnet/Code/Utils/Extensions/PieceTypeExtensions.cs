using TetrisDotnet.Code.Game;

namespace TetrisDotnet.Code.Utils.Extensions
{
	static class PieceTypeExtensions
	{
		public static int PossibleRotations(this PieceType pieceType)
		{
			switch (pieceType)
			{
				case PieceType.O:
					return 1;
				case PieceType.I:
				case PieceType.S:
				case PieceType.Z:
					return 2;
				case PieceType.J:
				case PieceType.L:
				case PieceType.T:
					return 4;
				default:
					return 0;
			}
		}
	}
}