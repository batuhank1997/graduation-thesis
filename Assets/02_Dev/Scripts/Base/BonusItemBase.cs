using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BonusItemBase : MonoBehaviour
{
    [SerializeField] private BonusType bonusType;

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce((Vector3.back * 1.5f) + (Vector3.up * 2.5f), ForceMode.Impulse);
    }

    public virtual void OnCollect()
    {
        
    }

    public virtual void Use()
    {
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        OnCollect();
    }

    public bool CompareBonusType(BonusType type)
    {
        if (bonusType == type)
            return true;

        return false;
    }
}