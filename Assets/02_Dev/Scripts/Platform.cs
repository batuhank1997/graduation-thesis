using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Rendering;

public class Platform : SliceableBase
{
    [SerializeField] private List<Ball> balls;

    private void Start()
    {
        SlicerObjectController.I.SetBalls();
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Keys.TAG_BALL))
        {
            print("OOOOO");
        }
    }*/
}
