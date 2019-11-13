namespace TetrisDotnet.Code.Events.EventData
{
    public abstract class EventData
    {
        public EventType Type { get; }

        protected EventData(EventType type)
        {
            Type = type;
        }
    }
}