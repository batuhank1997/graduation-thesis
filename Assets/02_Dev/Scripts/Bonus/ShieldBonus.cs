using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShieldBonus : BonusItemBase
{
    public override void OnCollect()
    {
        Use();
    }

    public override void Use()
    {
        BonusController.I.AddShield();
        base.Use();
    }
}