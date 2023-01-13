using System;
using Longhorn.Core;
using UnityEngine;

[Serializable]
public class LevelConfig
{
	[Range(1, 500)]
    public int levelNumber;
    public bool isEnabled;
}

[Serializable]
public class SettingsOptions
{
    public bool isSoundEnabledOnStart;
    public bool isHapticEnabledOnStart;
}

[Serializable]
public class InterstitialOptions
{
    public string interPlacement;
    public float interWinWaitTime;
}

[Serializable]
public class PopupConfig
{
    public int rateUsPopupLevel;
}

public class RemoteConfigData : PersistentSingleton<RemoteConfigData>
{
    public RemoteConfigJSONWrapper<LevelConfigRemoteData> levelConfig;
    public RemoteConfigJSONWrapper<SettingsOptions> settingsOptions;
    public RemoteConfigJSONWrapper<InterstitialOptions> interstitialOptions;
    public RemoteConfigJSONWrapper<PopupConfig> popupOptions;
    
    private void Awake()
    {
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
        GetRemoteConfigValues();
    }

    public void GetRemoteConfigValues(){
        levelConfig.GetRemoteValue();
        settingsOptions.GetRemoteValue();
        interstitialOptions.GetRemoteValue();
        popupOptions.GetRemoteValue();
    }
}