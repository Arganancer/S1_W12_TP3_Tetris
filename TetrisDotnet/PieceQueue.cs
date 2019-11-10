using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisDotnet;
using TetrisDotnet.Extensions;

namespace Tetris
{
    class PieceQueue
    {
        private List<PieceType> pieceList;
        private Bag bag;

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