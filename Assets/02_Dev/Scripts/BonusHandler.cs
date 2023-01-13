using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class BonusHandler : MonoBehaviour
{
    [SerializeField] private BonusItemBase[] bonuses;
    [SerializeField] private BonusType bonusType;
    
    void Start()
    {
        SlicerObjectController.onSlashExecuted += OnSlashExecute;
    }

    void OnSlashExecute()
    {
        StartCoroutine(CheckColliders());
    }

    IEnumerator CheckColliders()
    {
        yield return new WaitForEndOfFrame();
        
        Collider[] cols = Physics.OverlapSphere(transform.position, 0.25f);

        var flag = false;
        
        foreach (var col in cols)
        {
            print(col.gameObject.name);

            if (col.TryGetComponent(out Platform platform))
            {
                flag = true;
            }
        }

        if (!flag)
        {
            LaunchBonusItem();
        }
    }
    
    void LaunchBonusItem()
    {
        CreateBonusItem();
        Destroy(gameObject);
    }

    void CreateBonusItem()
    {
        foreach (var item in bonuses)
        {
            if (item.CompareBonusType(bonusType))
            {
                var bonusItem = Instantiate(item, transform.position + Vector3.back, Quaternion.identity);
                bonusItem.transform.SetParent(FindObjectOfType<LevelGenerator>().levelParent);
            }
        }
    }

    private void OnDestroy()
    {
        SlicerObjectController.onSlashExecuted -= OnSlashExecute;
    }
}
