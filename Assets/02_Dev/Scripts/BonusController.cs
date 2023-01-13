using System;
using System.Collections;
using System.Collections.Generic;
using Longhorn.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusController : Singleton<BonusController>
{
    [SerializeField] private Image shieldImg;
    [SerializeField] private int shieldCount;
    [SerializeField] private TextMeshProUGUI shieldCountText;

    private bool hasShield;

    private void Start()
    {
        UpdateShieldCountText();
    }

    public void UpdateShieldCountText()
    {
        if (shieldCount == 0)
        {
            shieldImg.enabled = false;
            shieldCountText.enabled = false;
            hasShield = false;
        }
        else
        {
            shieldImg.enabled = true;
            shieldCountText.enabled = true;
            hasShield = true;
        }

        shieldCountText.text = shieldCount.ToString();
    }

    public bool GetShieldStatus()
    {
        return hasShield;
    }
    
    public void AddShield()
    {
        shieldCount++;
        UpdateShieldCountText();
    }
    
    public void RemoveShield()
    {
        shieldCount--;
        UpdateShieldCountText();
    }
}
