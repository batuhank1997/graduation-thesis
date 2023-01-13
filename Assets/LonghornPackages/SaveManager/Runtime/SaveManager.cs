using System;
using System.Collections;
using System.Collections.Generic;
using Longhorn.Core;
using Managers;
using UnityEngine;

namespace  Longhorn.SaveManager {
    
    public class SaveManager : Singleton<SaveManager>
    {
        private void Awake() {
            LoadOnAwake();
        }

        public static void SaveValue<T>(string key, T value) {
            ES3.Save(key, value);
        }
        
        public static T LoadValue<T>(string key, T defaultValue) {
            return ES3.Load<T>(key, defaultValue);
        }

        public void SaveOnExit() {
            LevelManagerSOMode.I.SaveValues();
        }

        public void LoadOnAwake() {
            LevelManagerSOMode.I.LoadValues();
        }
        
        private void OnApplicationQuit()
        {
            SaveOnExit();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveOnExit();
            }
        }

    }

}

