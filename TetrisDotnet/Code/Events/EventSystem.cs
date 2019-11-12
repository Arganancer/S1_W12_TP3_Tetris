using System.Collections.Generic;

namespace TetrisDotnet.Code.Events
{
    public delegate void OnEvent(EventData.EventData eventData);

    public delegate void OnTrigger();

    public class EventSystem
    {
        private readonly SortedDictionary<EventType, List<OnEvent>> subscribers = new SortedDictionary<EventType, List<OnEvent>>();
        private readonly SortedDictionary<EventType, List<OnTrigger>> triggerSubscribers = new SortedDictionary<EventType, List<OnTrigger>>();

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
        
        public bool Subscribe(EventType eventType, OnTrigger onTrigger)
        {
            if (!triggerSubscribers.TryGetValue(eventType, out List<OnTrigger> eventSubscribers))
            {
                eventSubscribers = new List<OnTrigger>();
                triggerSubscribers.Add(eventType, eventSubscribers);
            }
            else if (eventSubscribers.Contains(onTrigger))
            {
                return false;
            }

            eventSubscribers.Add(onTrigger);
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
        
        public bool Unsubscribe(EventType eventType, OnTrigger onTrigger)
        {
            if (triggerSubscribers.TryGetValue(eventType, out List<OnTrigger> eventSubscribers))
            {
                return eventSubscribers.Remove(onTrigger);
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
        
        public void ProcessEvent(EventType eventType)
        {
            if (triggerSubscribers.TryGetValue(eventType, out List<OnTrigger> eventSubscribers))
            {
                foreach (OnTrigger eventSubscriber in eventSubscribers)
                {
                    eventSubscriber();
                }
            }
        }
    }
}