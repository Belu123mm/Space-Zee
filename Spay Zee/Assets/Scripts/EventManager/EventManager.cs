using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public delegate void eventReceiver(params object[] parameterContainer);
    private static Dictionary<EventsType, eventReceiver> _Events;

    public static void SubscribeToEvent(EventsType myEvent, eventReceiver listener)
    {
        if (_Events == null)
            _Events = new Dictionary<EventsType, eventReceiver>();

        if (!_Events.ContainsKey(myEvent))
            _Events.Add(myEvent, null);

        _Events[myEvent] += listener;
    }

    public static void UnsubscribeToEvent(EventsType myEvent, eventReceiver listener)
    {
        if (_Events != null)
            if (_Events.ContainsKey(myEvent))
                _Events[myEvent] -= listener;
    }

    public static void TriggerEvent(EventsType myEvent)
    {
        TriggerEvent(myEvent, null);
    }

    public static void TriggerEvent(EventsType myEvent, params object[] parameters)
    {
        if(_Events == null)
        {
            Debug.Log("subscribe to events first");
            return;
        }
        if(_Events.ContainsKey(myEvent))
            if(_Events[myEvent] != null)
                _Events[myEvent](parameters);
    }

    public enum EventsType
    {
        event_takeDMG,
        event_win,
        event_lose
    }
}
