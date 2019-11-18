using TetrisDotnet.Code.Game;

namespace TetrisDotnet.Code.Events.EventData
{
	public class PieceTypeEventData : EventData
	{
		public PieceType PieceType;

		public PieceTypeEventData(PieceType pieceType)
		{
			PieceType = pieceType;
		}
	}
}