namespace TetrisDotnet.Code.Events
{
    public enum EventType
    {
        // Input Events
        InputRotateClockwise,
        InputRotateCounterClockwise,
        InputDown,
        InputLeft,
        InputRight,
        InputHold,
        InputHardDrop,
        InputPause,
        InputEscape,
        
        // UI Input Events
        MouseEnter,
        MouseLeave,
        GainFocus,
        LoseFocus,
        
        // Scoring
        PiecePlaced,
        PieceDropped,
        LevelUp,
        ScoreUpdated,
        ComboUpdated,
        PiecesPlacedUpdated,
        
        // Logic
        NewHeldPiece,
        UpdatedPieceQueue,
        GridUpdated,
        GamePauseToggled,
        TimeUpdated,
    }
}