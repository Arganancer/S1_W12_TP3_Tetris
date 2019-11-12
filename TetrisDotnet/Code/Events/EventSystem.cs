using System.Collections.Generic;

namespace TetrisDotnet.Code.Events
{
    public delegate void OnEvent(EventData.EventData eventData);

    public class EventSystem
    {
        private readonly SortedDictionary<EventType, List<OnEvent>> subscribers = new SortedDictionary<EventType, List<OnEvent>>();

        public bool Subscribe(EventType eventType, OnEvent onEvent)
        {
            if (!subscribers.TryGetValue(eventType, out List<OnEvent> eventSubscribers))
            {
                eventSubscribers = new List<OnEvent>();
                subscribers.Add(eventType, eventSubscribers);
            }
            else if (eventSubscribers.Contains(onEvent))
            {
                return false;
            }

            eventSubscribers.Add(onEvent);
            return true;
        }

        public bool Unsubscribe(EventType eventType, OnEvent onEvent)
        {
            if (subscribers.TryGetValue(eventType, out List<OnEvent> eventSubscribers))
            {
                return eventSubscribers.Remove(onEvent);
            }

            return false;
        }

        public void ProcessEvent(EventData.EventData eventData)
        {
            if (subscribers.TryGetValue(eventData.Type, out List<OnEvent> eventSubscribers))
            {
                foreach (OnEvent eventSubscriber in eventSubscribers)
                {
                    eventSubscriber(eventData);
                }
            }
        }
    }
}