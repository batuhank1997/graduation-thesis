using System;
using Longhorn.Core;
using System.Collections.Generic;

namespace Managers {
    
    public class EventManager: Singleton<EventManager> {
        private Dictionary<string, Action<Dictionary<string, object>>> eventDictionary;

        private void Awake()
        {
            eventDictionary ??= new Dictionary<string, Action<Dictionary<string, object>>>();
        }

        public static void StartListening(string eventName, Action<Dictionary<string, object>> listener) {
            if (I.eventDictionary.TryGetValue(eventName, out var thisEvent)) {
                thisEvent += listener;
                I.eventDictionary[eventName] = thisEvent;
            } else {
                thisEvent += listener;
                I.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, Action<Dictionary<string, object>> listener) {
            if (I == null) return;

            if (!I.eventDictionary.TryGetValue(eventName, out var thisEvent)) return;
            thisEvent -= listener;
            I.eventDictionary[eventName] = thisEvent;
        }

        public static void TriggerEvent(string eventName, Dictionary<string, object> message) {
            if (I.eventDictionary.TryGetValue(eventName, out var thisEvent)) {
                thisEvent.Invoke(message);
            }
        }      
    }
}