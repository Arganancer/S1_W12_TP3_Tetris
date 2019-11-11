using System.Collections.Generic;

namespace TetrisDotnet.Code.Game
{
	class PieceQueue
	{
		private readonly Queue<PieceType> pieceList;
		private readonly Bag bag;

		public PieceQueue()
		{
			bag = new Bag();
			pieceList = new Queue<PieceType>();
			for (int i = 0; i < 3; i++)
			{
				pieceList.Enqueue(bag.Next());
			}
		}

		public PieceType GrabNext()
		{
			pieceList.Enqueue(bag.Next());

			return pieceList.Dequeue();
		}

		public IEnumerable<PieceType> Get()
		{
			return pieceList;
		}
	}
}