using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
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
        Dead

	}

	class Piece
	{

		public PieceType type { get; set; }

		public Vector2i position { get; set; }

		private PieceType[,] pieceArray;

        // (C)
        /// <summary>
        /// Positionne la pièce de jeu au centre de la surface de jeu et en haut de celle-ci.
        /// </summary>
        /// 
        /// <param name="_type">
        /// Le type de bloc qui sera placé.
        /// </param>
		public Piece(PieceType _type = PieceType.Dead)
		{

            // Debug
			if (_type == PieceType.Dead)
			{

				// Chooses a random piece.
				type = (PieceType)Enum.GetValues(typeof(PieceType)).GetValue(RandomTetris.rnd.Next(7));

			}
			else
			{
					
				type = _type;
				
			}

			pieceArray = StaticVars.GetPieceArray(type);

			// Centers the piece and place it at the top of the grid.
			if (type != PieceType.O)
			{
				
				position = new Vector2i(5 - (int)Math.Ceiling((decimal)pieceArray.GetLength(1) / 2), 0);

			}
			else
			{

				position = new Vector2i(5 - (int)Math.Ceiling((decimal)pieceArray.GetLength(1) / 2), -1);

			}

		}

		public PieceType[,] GetPieceArray()
		{

			return pieceArray;

		}

        public void SetPieceArray(PieceType[,] newPieceArray)
        {
            pieceArray = newPieceArray;
        }

	}
}
