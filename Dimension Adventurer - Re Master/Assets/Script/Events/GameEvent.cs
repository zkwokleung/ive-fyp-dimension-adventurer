using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Event
{
    [CreateAssetMenu]
    public class GameEvent : ScriptableObject
    {
        private List<EventListener> listeners = new List<EventListener>();

        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEventRaised();
        }

        public void RegisterListener(EventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(EventListener listener)
        {
            listeners.Remove(listener);
        }
    }

}