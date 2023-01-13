using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBoardingLine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SlicerObjectController.onSlashExecuted += EndOnBoarding;
    }

    // Update is called once per frame
    void EndOnBoarding()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SlicerObjectController.onSlashExecuted -= EndOnBoarding;
    }
}
