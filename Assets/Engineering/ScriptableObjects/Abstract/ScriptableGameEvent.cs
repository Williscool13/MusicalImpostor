using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableGameEvent<T> : ScriptableObject
{
    private List<ScriptableGameEventListener<T>> listeners = new List<ScriptableGameEventListener<T>>();

    public void Raise(T eventData) {
        Debug.Log($"[{name}] ScriptableGameEvent raised with data: {eventData}");
        for (int i = listeners.Count - 1; i >= 0; i--) {
            listeners[i].OnEventRaised(eventData);
        }
    }

    public void RegisterListener(ScriptableGameEventListener<T> listener) {
        listeners.Add(listener);
    }

    public void UnregisterListener(ScriptableGameEventListener<T> listener) {
        listeners.Remove(listener);
    }
}