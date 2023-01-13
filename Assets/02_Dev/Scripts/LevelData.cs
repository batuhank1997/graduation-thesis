using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [Range(0,100)]
    [SerializeField] private float levelFinishPercentage;
    [SerializeField] private Material skybox;

    private void Start()
    {
        RenderSettings.skybox = skybox;
    }

    public float GetLevelFinishPercentage()
    {
        return levelFinishPercentage;
    }
}
