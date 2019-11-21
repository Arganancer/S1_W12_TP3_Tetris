using TetrisDotnet.Code.Game.Stats;

namespace TetrisDotnet.Code.Events.EventData
{
	public class PieceScoreEventData : EventData
	{
		public Statistics.PieceScore PieceScore;

		public PieceScoreEventData(Statistics.PieceScore pieceScore)
		{
			PieceScore = pieceScore;
		}
	}
}