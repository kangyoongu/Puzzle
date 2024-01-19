using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum State : short
{
    Up = -1,
    Normal = 0,
    Down = 1,
    BeforeLaserWork = 2
}
public static class EventBus
{
    private static readonly IDictionary<State, UnityEvent> Events = new Dictionary<State, UnityEvent>();
    public static void Subscribe(State eventType, UnityAction listener)
    {
        UnityEvent thisEvent;
        if (Events.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent(); 
            thisEvent.AddListener(listener); 
            Events.Add(eventType, thisEvent);
        }
    }
    public static void Unsubscribe(State type, UnityAction listener)
    {
        UnityEvent thisEvent;
        if (Events.TryGetValue(type, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    public static void Publish(State type)
    {
        UnityEvent thisEvent;
        if (Events.TryGetValue(type, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}