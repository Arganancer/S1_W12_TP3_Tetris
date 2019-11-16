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
        
        // UI Events
        MouseEnter,
        MouseLeave,
        GainFocus,
        LoseFocus,
    }
}