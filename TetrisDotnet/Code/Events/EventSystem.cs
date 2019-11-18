using System.Collections.Generic;
using System.Linq;

namespace TetrisDotnet.Code.Events
{
    public delegate void OnEvent(EventData.EventData eventData);

    public class EventSystem
    {
        private readonly SortedDictionary<EventType, List<OnEvent>> subscribers = new SortedDictionary<EventType, List<OnEvent>>();
        
        private readonly SortedDictionary<EventType, List<EventData.EventData>> queuedEvents = new SortedDictionary<EventType, List<EventData.EventData>>();

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

        public void ProcessEvent(EventType eventType, EventData.EventData eventData = null)
        {
            if (subscribers.TryGetValue(eventType, out List<OnEvent> eventSubscribers))
            {
                foreach (OnEvent eventSubscriber in eventSubscribers)
                {
                    eventSubscriber(eventData);
                }
            }
        }

        public void ProcessQueuedEvents()
        {
            foreach ((EventType eventType, List<EventData.EventData> eventDataList) in queuedEvents)
            {
                foreach (EventData.EventData eventData in eventDataList)
                {
                    ProcessEvent(eventType, eventData);
                }
            }
        }

        /// <summary>
        /// Queued events are processed after all updates, just before the draw call.
        /// </summary>
        /// <param name="isUnique">If true, the event will replace all events of the same type already in the list.</param>
        public void QueueEvent(EventType eventType, bool isUnique, EventData.EventData eventData = null)
        {
            if (queuedEvents.TryGetValue(eventType, out List<EventData.EventData> eventDataList))
            {
                if (isUnique && eventDataList.Count > 0)
                {
                    eventDataList.Clear();
                }
            }
            else
            {
                eventDataList = new List<EventData.EventData>();
                queuedEvents.Add(eventType, eventDataList);
            }

            eventDataList.Add(eventData);
        }
    }
}