namespace TetrisDotnet.Code.Game
{
	class Hold
	{
		public PieceType currentPiece { get; set; }

		public bool canSwap { get; set; }

		public Hold(PieceType piece = PieceType.Empty)
		{
			currentPiece = piece;
		}
	}
}