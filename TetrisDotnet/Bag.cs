using System;
using System.Collections.Generic;
using System.Linq;
using TetrisDotnet.Extensions;

namespace TetrisDotnet
{
    class Bag
    {
        private readonly Random rnd = new Random();

        private List<PieceType> bag = new List<PieceType>();

        private void GenerateBag()
        {
            bag = new List<PieceType>
                    {PieceType.I, PieceType.J, PieceType.L, PieceType.O, PieceType.S, PieceType.T, PieceType.Z}
                .OrderBy(i => rnd.NextDouble()).ToList();
        }

        public PieceType Next()
        {
            if (bag.Count == 0)
            {
                GenerateBag();
            }

            return bag.Pop();
        }
    }
}