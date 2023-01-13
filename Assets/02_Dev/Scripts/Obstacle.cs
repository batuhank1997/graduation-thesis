using System;
using Sirenix.Utilities;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void Start()
    {
        SlicerObjectController.onSlashExecuted += CheckPlatform;
    }

    void CheckPlatform()
    {
        print("CHECKING PLATFORM");
        
        var colliders = Physics.OverlapSphere(transform.position, 0.25f);
        
        colliders.ForEach(collider => 
        {
            if (collider.CompareTag("Hull"))
            {
                transform.SetParent(collider.transform);
            }
        });
    }

    private void OnDestroy()
    {
        SlicerObjectController.onSlashExecuted -= CheckPlatform;
    }
}
