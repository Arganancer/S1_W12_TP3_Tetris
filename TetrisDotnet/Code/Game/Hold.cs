using TetrisDotnet.Code.Events;
using TetrisDotnet.Code.Events.EventData;

namespace TetrisDotnet.Code.Game
{
	class Hold
	{
		private PieceType currentPiece;
		public PieceType CurrentPiece
		{
			get => currentPiece;
			set
			{
				if (currentPiece != value)
				{
					currentPiece = value;
					Application.EventSystem.ProcessEvent(EventType.NewHeldPiece, new PieceTypeEventData(currentPiece));
				}
			}}

		public bool CanSwap { get; set; }

		public Hold(PieceType piece = PieceType.Empty)
		{
			currentPiece = piece;
		}
	}
}