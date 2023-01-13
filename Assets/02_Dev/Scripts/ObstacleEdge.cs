using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

public class ObstacleEdge : MonoBehaviour
{
    private void Update()
    {
        var cols = Physics.OverlapSphere(transform.position, 0.25f).Where(col => col.tag == "Platform").ToArray();

        if (cols.Length == 0)
            Destroy(gameObject);
    }
}
