using System;
using ElephantSDK;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

namespace Managers
{
    public enum GameState
    {
        Init,
        Playıng,
        Paused,
        Finished
    }

    /// <summary>
    /// * GM should be responsible for theese;  
    /// Play, pause, game over statements 
    /// Save functions, game state changes
    /// Elephant events
    /// </summary>
    /// 
    public class GameManager : ManagerBase<GameManager, GameState>
    {
        [Tooltip("If you use scene mode, then uncheck the SOModeLevelScene on BuildSettings")] 
        
        [SerializeField] private LevelManagerSOMode lmSoMode;
        [SerializeField] private LevelManagerSceneMode lmSceneMode;
        
        [Range(0f, 5f)]
        [SerializeField] private float modifiedGameSpeed;
        
        private LevelManager levelManager;
        private LonghornConfig longhornConfig;

        private void Awake()
        {
            Reset();
            DontDestroyOnLoad(gameObject);
        }
        
        public void Reset()
        {
            SetState(GameState.Init);
        }
        
        private void Start()
        {
            SetLevelModeConfigs();
        }

        private void SetLevelModeConfigs()
        {
            longhornConfig = Resources.Load<LonghornConfig>("LonghornConfig");
            
            if (TryGetComponent(out FPSDisplay fpsDisplay))
                fpsDisplay.enabled = longhornConfig.fpsDisplayEnabled;

            if (!longhornConfig.sceneModeEnabled)
            {
                levelManager = lmSoMode;
                lmSoMode.gameObject.SetActive(true);
                SceneManager.LoadScene(2); //If SO Mode then should be checked "SOModeLevelScene" on BuildSettings at 2nd Index
            }
            else
            {
                levelManager = lmSceneMode;
                lmSceneMode.gameObject.SetActive(true);
                SetupStartLevel();
            }
        }

        public LonghornConfig GetLonghornConfig()
        {
            return longhornConfig;
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.S)) return;
            Time.timeScale = Time.timeScale == 1f ? modifiedGameSpeed : 1f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
#endif
        
        public void SetupStartLevel()
        {
            SetState(GameState.Playıng);

            levelManager.LoadCurrentLevel();
            Elephant.LevelStarted(levelManager.levelCounter + 1);
            
            if (!longhornConfig.sceneModeEnabled)
                UIManager.I.SetLevelNumberFromLevelData();
        }

        [Button("LEVEL COMPLETED")]
        public void LevelCompleted()
        {
            if (State == GameState.Finished)
                return;

            Elephant.LevelCompleted(levelManager.levelCounter + 1);

            levelManager.SetNextLevel();
            SetState(GameState.Finished);
            UIManager.I.SetState(UIState.Won);
        }

        [Button("LEVEL FAILED")]
        public void LevelFailed()
        {
            if (State == GameState.Finished)
                return;

            SetState(GameState.Finished);
            UIManager.I.SetState(UIState.Lost);
            Elephant.LevelFailed(levelManager.levelCounter + 1);
        }
    }
}