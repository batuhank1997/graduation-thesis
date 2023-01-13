using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Managers
{
    /// <summary>
    /// Load levels from reading Scriptable Object files as named 'GameLevelData'
    /// WARNING: Check enable the scene named "SOModeLevelScene" from Build Settings if you use SOMode !
    /// </summary>
    [RequireComponent(typeof(LevelGenerator))]
    public class LevelManagerSOMode : LevelManager
    {
        [SerializeField] private bool editMode;

        [Header("References")] [SerializeField]
        private GameLevelData currentGameLevelData;

        [SerializeField] private LevelGenerator levelGenerator;
        [SerializeField] private List<GameLevelData> allLevels;

        private void Awake()
        {
            Reset();
            LoadValues();
            SetMaxLevelCount();
            DontDestroyOnLoad(gameObject);
        }

        private GameLevelData GetCurrentLevelSo()
        {
            int calculatedLevel = CalculatedCurrentLevel();

            if (allLevels[calculatedLevel] is null)
                Debug.LogWarning("THERE IS NO LEVEL => " + calculatedLevel);

            return allLevels[calculatedLevel];
        }

        #region NEXT LEVEL FOR TESTING

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                LoadNextLevel();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.I.LevelCompleted();
            }
        }
#endif

        #endregion

        protected override void LoadLevel()
        {
            base.LoadLevel();

            currentGameLevelData = GetCurrentLevelSo();
            levelGenerator.GenerateLevel(currentGameLevelData);

            LevelManager.OnLevelLoaded?.Invoke();
            Debug.Log("Level Loaded Successfully from SO Mode ! ");
        }

        //Generate the level just for see & edit regardless of save operations
        [ShowIf("editMode")]
        [Button("LOAD LEVEL FOR EDIT")]
        public void LoadLevelForEdit()
        {
            if (currentGameLevelData == null)
            {
                Debug.LogWarning("There is no level on inspector ! Level couldn't be loaded.");
                return;
            }

            levelGenerator.GenerateLevel(currentGameLevelData);
        }

        [ShowIf("editMode")]
        [Button("CLEAR TEST LEVEL")]
        public void ClearTestLevel()
        {
            levelGenerator.ClearPreviousLevelObjects();
        }

        public override void SetMaxLevelCount()
        {
            maxLevelCount = allLevels.Count;
            OnMaxLevelCountSet();
        }
    }
}