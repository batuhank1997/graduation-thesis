using Managers;
using UnityEngine;
using UnityEngine.UI;

public class LonghornUIButtonReloadLevel : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick()
    {
        LevelManager.I.isRestarted = true;
        GameManager.I.SetupStartLevel();
        UIManager.I.SetState(UIState.Init);
    }
}