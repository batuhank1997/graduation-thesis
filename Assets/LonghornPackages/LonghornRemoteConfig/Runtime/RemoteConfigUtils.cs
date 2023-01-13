using UnityEngine;

public class RemoteConfigUtils
{

    public static T GetRemoteJSONValue<T>(string key)
    {
        return GetRemoteJSONValue<T>(key, out var keyExists);
    }
    
    public static T GetRemoteJSONValue<T>(string key, out bool keyExists)
    {
        string jsonString = ElephantSDK.RemoteConfig.GetInstance().Get(key, null);
        
        if (jsonString == null)
        {
            keyExists = false;
            return default;
        }

        var remoteConf = JsonUtility.FromJson<T>(jsonString);
        keyExists = true;
        
        return remoteConf;
    }

    public static float GetRemoteFloatValue(string key, float def) {
        return ElephantSDK.RemoteConfig.GetInstance().GetFloat(key, def);
    }

    public static int GetRemoteIntValue(string key, int def) {
        return ElephantSDK.RemoteConfig.GetInstance().GetInt(key, def);
    }
    
    public static bool GetRemoteBoolValue(string key, bool def) {
        return ElephantSDK.RemoteConfig.GetInstance().GetBool(key, def);
    }
}