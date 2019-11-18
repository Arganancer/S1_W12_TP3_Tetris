using System.Collections.Generic;
using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;

namespace TetrisDotnet.Code.Game
{
	class PieceQueue
	{
		private readonly Queue<PieceType> pieceQueue;
		private readonly Bag bag;

		public PieceQueue()
		{
			bag = new Bag();
			pieceQueue = new Queue<PieceType>();
			for (int i = 0; i < 3; i++)
			{
				pieceQueue.Enqueue(bag.Next());
			}
		}

		public PieceType GrabNext()
		{
			pieceQueue.Enqueue(bag.Next());
			PieceType pieceType = pieceQueue.Dequeue();
			Application.EventSystem.ProcessEvent(EventType.UpdatedPieceQueue, new UpdatedPieceQueueEventData(pieceQueue));
			return pieceType;
		}

		public IEnumerable<PieceType> Get()
		{
			return pieceQueue;
		}
	}
}