using UnityEngine;
using UnityEngine.Events;

public abstract class ScriptableGameEventListener<T> : MonoBehaviour
{
    public ScriptableGameEvent<T> Event;
    public UnityEvent<T> Response;

    private void OnEnable() {
        Event.RegisterListener(this);
    }

    private void OnDisable() {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(T eventData) {
        Response.Invoke(eventData);
    }
}