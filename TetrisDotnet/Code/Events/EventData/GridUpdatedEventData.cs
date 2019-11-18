using TetrisDotnet.Code.Game.World;

namespace TetrisDotnet.Code.Events.EventData
{
	public class GridUpdatedEventData : EventData
	{
		public Grid Grid;

		public GridUpdatedEventData(Grid grid)
		{
			Grid = grid;
		}
	}
}