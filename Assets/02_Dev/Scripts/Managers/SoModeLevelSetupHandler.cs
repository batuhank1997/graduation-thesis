using Managers;
using UnityEngine;

public class SoModeLevelSetupHandler : MonoBehaviour
{
    private void Start()
    {
        GameManager.I.SetupStartLevel();
    }
}