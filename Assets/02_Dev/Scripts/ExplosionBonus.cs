using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ExplosionBonus : BonusItemBase
{
    public override void OnCollect()
    {
        base.OnCollect();
        Use();
    }

    public override void Use()
    {
        AudioController.I.PlayBombFX();
        var balls = SlicerObjectController.I.GetBalls();
        var ball = balls[(Random.Range(0, balls.Count))];
        ball.PlaySplashFX();
        base.Use();
    }
}
