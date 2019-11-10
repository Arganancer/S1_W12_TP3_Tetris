using System;
using SFML.System;
using Tetris;

namespace TetrisDotnet
{
    class Piece
    {
        public PieceType[,] pieceArray { get; set; }
        private readonly Random random = new Random();

        public PieceType type { get; set; }
        public Vector2i position { get; set; }

        public Piece(PieceType type = PieceType.Dead)
        {
            // Debug
            if (type == PieceType.Dead)
            {
                // Chooses a random piece.
                this.type = (PieceType) Enum.GetValues(typeof(PieceType)).GetValue(random.Next(7));
            }
            else
            {
                this.type = type;
            }

            pieceArray = StaticVars.GetPieceArray(this.type);

            // Centers the piece and place it at the top of the grid.
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