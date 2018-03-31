using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
	static class RandomTetris
	{

		//Basicly did this not to create a new random every time I call it
		public static Random rnd = new Random();

		private static List<PieceType> list = new List<PieceType>();

		private static void GenerateBag()
		{

			list.Clear();

			PieceType pieceType;

			//Cause theres 7 pieces, the one thing I can hardcode
			for (int i = 0; i < 7; i++)
			{

				do
				{

					pieceType = (PieceType)Enum.GetValues(typeof(PieceType)).GetValue(RandomTetris.rnd.Next(7));

				} while (list.Contains(pieceType));

				list.Add(pieceType);

			}


		}
		
		public static PieceType GetPiece()
		{

			if(list.Count == 0)
			{

				GenerateBag();

			}

			return list.Pop();

		}

	}
}
