using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionBonus : BonusItemBase
{
    public override void OnCollect()
    {
        Use();
    }

    public override void Use()
    {
        SlicerObjectController.I.GetBalls().ForEach(ball =>
        {
            StartCoroutine(ball.EnterSlowMotion());
        });
        
        base.Use();
    }
}
