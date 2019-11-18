using TetrisDotnet.Code.Game.Stats;

namespace TetrisDotnet.Code.Events.EventData
{
	public class PieceDroppedEventData : EventData
	{
		public Drop DropType;
		public int DistanceDropped;
		
		public PieceDroppedEventData(Drop dropType, int distanceDropped)
		{
			DropType = dropType;
			DistanceDropped = distanceDropped;
		}
	}
}