using UnityEngine;

public class SetActiveFalseOnGlobalMouseClick : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            gameObject.SetActive(false);
        }
    }
}