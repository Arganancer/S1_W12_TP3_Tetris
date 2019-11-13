using TetrisDotnet.Code.Game;

namespace TetrisDotnet.Code.AI
{
	class Action
	{
		public ActionType actionType { get; }
		public Piece destinationPiece { get; }

		public Action(ActionType actionType, Piece destinationPiece)
		{
			this.actionType = actionType;
			this.destinationPiece = destinationPiece;
		}
	}
}