using System.Collections.Generic;
using System.Linq;
using TetrisDotnet.Code.Utils;

namespace TetrisDotnet.Code.Game
{
	internal class Bag
	{
		private Queue<PieceType> bag = new Queue<PieceType>();

		private void GenerateBag()
		{
			bag = new Queue<PieceType>(PieceTypeUtils.GetRealPieces().OrderBy(i => Rand.NextDouble()));
		}

		public PieceType Next()
		{
			if (bag.Count == 0)
			{
				GenerateBag();
			}

			return bag.Dequeue();
		}
	}
}