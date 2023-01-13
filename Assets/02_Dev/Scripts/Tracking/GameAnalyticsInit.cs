using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using UnityEngine;

public class GameAnalyticsInit : MonoBehaviour, IGameAnalyticsATTListener
{
    void Start()
    {
        GameAnalytics.Initialize();        
    }

    public void GameAnalyticsATTListenerNotDetermined() {
        GameAnalytics.Initialize();        
    }

    public void GameAnalyticsATTListenerRestricted() {
        GameAnalytics.Initialize();        
    }

    public void GameAnalyticsATTListenerDenied() {
        GameAnalytics.Initialize();        
    }

    public void GameAnalyticsATTListenerAuthorized() {
        GameAnalytics.Initialize();        
    }
}
