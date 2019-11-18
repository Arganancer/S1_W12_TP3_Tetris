using System;
using System.Collections.Generic;
using SFML.System;
using TetrisDotnet.Code.Game.World;

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
		Ghost = 8
	}

	public enum PieceState
	{
		Active,
		Dead,
		None,
	}

	public static class PieceTypeUtils
	{
		public static IEnumerable<PieceType> GetRealPieces()
		{
			return new[] {PieceType.I, PieceType.O, PieceType.T, PieceType.S, PieceType.Z, PieceType.J, PieceType.L};
		}

		public static Vector2i GetDefaultPosition(PieceType pieceType)
		{
			switch (pieceType)
			{
				case PieceType.I:
					return new Vector2i(Grid.GridWidth / 2 - 1, 1);
				case PieceType.O:
					return new Vector2i(Grid.GridWidth / 2 - 1, 0);
				case PieceType.T:
					return new Vector2i(Grid.GridWidth / 2 - 1, 0);
				case PieceType.S:
					return new Vector2i(Grid.GridWidth / 2 - 1, 0);
				case PieceType.Z:
					return new Vector2i(Grid.GridWidth / 2 - 1, 0);
				case PieceType.J:
					return new Vector2i(Grid.GridWidth / 2 - 1, 0);
				case PieceType.L:
					return new Vector2i(Grid.GridWidth / 2 - 1, 0);
				default:
					throw new ArgumentOutOfRangeException(nameof(pieceType), pieceType,
						$"Invalid Piece Type does not have a default position: {Enum.GetName(typeof(PieceType), pieceType)}");
			}
		}

		public static List<Vector2i> GetPieceTypeBlocks(PieceType pieceType)
		{
			switch (pieceType)
			{
				case PieceType.I:
					return new List<Vector2i>
					{
						new Vector2i(0, 0),
						new Vector2i(1, 0),
						new Vector2i(2, 0),
						new Vector2i(3, 0)
					};
				case PieceType.O:
					return new List<Vector2i>
					{
						new Vector2i(1, 0),
						new Vector2i(2, 0),
						new Vector2i(1, 1),
						new Vector2i(2, 1)
					};
				case PieceType.T:
					return new List<Vector2i>
					{
						new Vector2i(1, 0),
						new Vector2i(0, 1),
						new Vector2i(1, 1),
						new Vector2i(2, 1)
					};
				case PieceType.S:
					return new List<Vector2i>
					{
						new Vector2i(1, 0),
						new Vector2i(2, 0),
						new Vector2i(0, 1),
						new Vector2i(1, 1)
					};
				case PieceType.Z:
					return new List<Vector2i>
					{
						new Vector2i(0, 0),
						new Vector2i(1, 0),
						new Vector2i(1, 1),
						new Vector2i(2, 1)
					};
				case PieceType.J:
					return new List<Vector2i>
					{
						new Vector2i(0, 0),
						new Vector2i(0, 1),
						new Vector2i(1, 1),
						new Vector2i(2, 1)
					};
				case PieceType.L:
					return new List<Vector2i>
					{
						new Vector2i(2, 0),
						new Vector2i(0, 1),
						new Vector2i(1, 1),
						new Vector2i(2, 1)
					};
				default:
					throw new ArgumentOutOfRangeException(nameof(pieceType), pieceType,
						$"Invalid Piece Type does not contain blocks: {Enum.GetName(typeof(PieceType), pieceType)}");
			}
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