using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{

	class Hold
	{
        
		public PieceType currentPiece { get; set; }

		public bool canSwap { get; set; }

		public Hold(PieceType piece = PieceType.Empty)
		{

			currentPiece = piece;

		}

	}
}
