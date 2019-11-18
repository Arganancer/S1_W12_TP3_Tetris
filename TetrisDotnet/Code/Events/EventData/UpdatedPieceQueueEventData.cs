using System.Collections.Generic;
using TetrisDotnet.Code.Game;

namespace TetrisDotnet.Code.Events.EventData
{
	public class UpdatedPieceQueueEventData : EventData
	{
		public readonly IEnumerable<PieceType> PieceTypes;

		public UpdatedPieceQueueEventData(IEnumerable<PieceType> pieceTypes)
		{
			PieceTypes = pieceTypes;
		}
	}
}