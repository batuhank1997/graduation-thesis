using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SliceableBase : MonoBehaviour
{
    public Material GetMaterial()
    {
        return GetComponent<MeshRenderer>().material;
    }
}