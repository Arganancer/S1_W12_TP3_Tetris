using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
	class PieceQueue
	{
		private List<PieceType> pieceList;

		public PieceQueue()
		{

			pieceList = new List<PieceType>();

			SetUpList();

		}

		void SetUpList()
		{

			for (int i = 0; i < 3; i++)
			{

				pieceList.Add(RandomTetris.GetPiece());

			}

		}

		public PieceType GrabNext()
		{

			pieceList.Add(RandomTetris.GetPiece());

			PieceType pieceToReturn = pieceList[0];

			pieceList.RemoveAt(0);

			return pieceToReturn;

		}

		public List<PieceType> GetList()
		{

			return pieceList;

		}

	}
}
