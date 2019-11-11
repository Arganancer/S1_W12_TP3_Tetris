using System;
using System.Diagnostics;
using SFML.System;
using TetrisDotnet.Code.Utils;

namespace TetrisDotnet.Code.Game
{
	class Piece
	{
		public PieceType[,] pieceArray { get; set; }
		public PieceType type { get; }
		public Vector2i position { get; set; }

		public Piece(PieceType type)
		{
			this.type = type;
			Debug.Assert(this.type == PieceType.Dead, "New piece was \"Dead\". This should never happen.");

			pieceArray = StaticVars.GetPieceArray(this.type);

			// Center the piece and place it at the top of the grid.
			if (this.type != PieceType.O)
			{
				position = new Vector2i(5 - (int) Math.Ceiling((decimal) pieceArray.GetLength(1) / 2), 0);
			}
			else
			{
				position = new Vector2i(5 - (int) Math.Ceiling((decimal) pieceArray.GetLength(1) / 2), -1);
			}
		}
	}
}