using UnityEngine;
using UnityEngine.Events;

public abstract class EventListener<T> : MonoBehaviour
{
    [SerializeField] private EventChannel<T> channel;
    [SerializeField] private UnityEvent<T> unityEvent;

    protected void Awake()
    {
        channel.Register(this);
    }

    protected void OnDestroy()
    {
        channel.Deregister(this);
    }

    public void Raise(T value)
    {
        unityEvent?.Invoke(value);
    }
}
