using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Longhorn.Core;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    void Start()
    {
        // SlicerObjectController.onSlashExecuted += SuccessCamShake;
    }

    public void SuccessCamShake()
    {
        transform.DOShakePosition( 0.05f,0.075f,1,180,false,false);
    }
    
    public void FailCamShake()
    {
        if (DOTween.IsTweening(transform))
            return;

        transform.DOShakePosition( 0.15f,new Vector3(0.5f, 0.5f, 0),5,180,false,false).OnComplete(() =>
        {
            transform.DOShakePosition(0.15f, new Vector3(0.5f, 0.5f, 0), 5, 180, false, false);
        });
    }
}
