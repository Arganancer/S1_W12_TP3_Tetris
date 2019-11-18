using System.Collections.Generic;
using TetrisDotnet.Code.Game.Stats;

namespace TetrisDotnet.Code.Events.EventData
{
	public class PiecePlacedEventData : EventData
	{
		public List<Move> Moves;
		
		public PiecePlacedEventData(List<Move> moves)
		{
			Moves = moves;
		}
	}
}