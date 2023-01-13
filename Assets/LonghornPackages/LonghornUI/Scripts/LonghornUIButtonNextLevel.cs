using Managers;
using UnityEngine;
using UnityEngine.UI;

public class LonghornUIButtonNextLevel : MonoBehaviour 
{
    [SerializeField] Button button;

    private void Start() 
    {
        button.onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick() 
    {
        GameManager.I.SetupStartLevel();
        UIManager.I.SetState(UIState.Init);
    }
}