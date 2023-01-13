using System;
using UnityEngine;

[Serializable]
public class RemoteConfigJSONWrapper<T>
{
    public string key;
    public T value;

    public void GetRemoteValue()
    { 
        var temp = RemoteConfigUtils.GetRemoteJSONValue<T>(key, out var keyExists);
        
        Debug.Log(key + keyExists);
        
        if (keyExists)
            value = temp;
    }
    
}