using System.Collections.Generic;
using UnityEngine;

public abstract class EventChannel<T> : ScriptableObject
{
    private readonly HashSet<EventListener<T>> observers = new();

    public void Invoke (T value)
    {
        foreach (var observer in observers)
        {
            observer.Raise(value);
        }
    }

    public void Register(EventListener<T> listener) => observers.Add(listener);

    public void Deregister(EventListener<T> listener) => observers.Remove(listener);


}
