using System.Collections.Generic;
using TetrisDotnet.Code.Utils.Extensions;

namespace TetrisDotnet.Code.Game
{
	class PieceQueue
	{
		private readonly List<PieceType> pieceList;
		private readonly Bag bag;

		public PieceQueue()
		{
			bag = new Bag();
			pieceList = new List<PieceType> {bag.Next(), bag.Next(), bag.Next()};
		}

		public PieceType GrabNext()
		{
			pieceList.Add(bag.Next());

			return pieceList.PopLeft();
		}

		public List<PieceType> GetList()
		{
			return pieceList;
		}
	}
}