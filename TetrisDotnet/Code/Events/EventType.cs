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
        MouseMove,
        MouseButton,
        
        // Scoring
        PiecePlaced,
        PieceDropped,
        LevelUp,
        ScoreUpdated,
        ComboUpdated,
        PiecesPlacedUpdated,
        PieceScore,
        
        // Logic
        NewHeldPiece,
        UpdatedPieceQueue,
        GridUpdated,
        GamePauseToggled,
        TimeUpdated,
        ToggleAi,
        
        // Window Events
        WindowResized,
    }
}