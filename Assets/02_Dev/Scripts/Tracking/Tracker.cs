using System;
using System.Collections.Generic;
using ElephantSDK;
using UnityEngine;
using Longhorn.Core;

namespace Tracking {
    public class Tracker : Singleton<Tracker> {
        public bool logsEnabled;
        
        public void Track(string eventName) {
            Log(eventName);
            Elephant.Event(eventName, 1);
        }

        public void Track(string eventName, Params parameters) {
            Log(eventName);
            Elephant.Event(eventName, 1, parameters);
        }

        private void Log(string eventName) {
            if (!logsEnabled) return;
            print($"tracking event {eventName}");
        }
    }
}
