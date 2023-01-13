using System;
using System.Linq;
using UnityEngine;
using Longhorn.SaveManager;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Managers
{
    public enum LevelState
    {
        Init,
        Started,
        Won,
        Lost
    }

    public abstract class LevelManager : ManagerBase<LevelManager, LevelState>
    {
        public static Action OnLevelLoaded;

        #region PROPERTIES

        [Header("Level Load Properties")] [Space(10)]
        public bool startByLevelNumber;

        [ShowIf("startByLevelNumber")] [Range(1, 500)]
        public int startLevelNumber;

        [NonSerialized] public int levelCounter;
        [NonSerialized] public bool isRestarted;

        protected int maxLevelCount;
        private int currentLevelIndex;
        private int prevLoadedLevelIndex = -1;

        #endregion

        #region INDEX LISTS

        private readonly List<int> levelIndexesToUse = new List<int>();
        private readonly List<int> levelIndexesToUseAfterLoop = new List<int>();

        #endregion

        private static LevelConfigRemoteData LevelOptions => RemoteConfigData.I.levelConfig.value;

        #region SAVE

        public abstract void SetMaxLevelCount();

        public virtual void LoadValues()
        {
            levelCounter = SaveManager.LoadValue(Keys.PLAYED_LEVEL_COUNT, 0);
            currentLevelIndex = SaveManager.LoadValue(Keys.CURRENT_LEVEL, 0);
        }

        public virtual void SaveValues()
        {
            SaveManager.SaveValue(Keys.PLAYED_LEVEL_COUNT, levelCounter);
            SaveManager.SaveValue(Keys.CURRENT_LEVEL, currentLevelIndex);
        }

        #endregion

        #region LEVEL LOADING

        private void LoopLevelNumberCheck()
        {
            if (LevelOptions.levelToLoopFrom <= maxLevelCount) return;

            Debug.LogWarning(
                "'levelToLoopFrom' variable on Remote Manager Level Config should assign to be between '1' & maxLevelCount ! " +
                "It has been set as '1' to loop from start level");

            LevelOptions.levelToLoopFrom = 1;
        }

        protected void OnMaxLevelCountSet()
        {
            LoopLevelNumberCheck();
            SetLevelIndexLists();
        }

        protected virtual void LoadLevel()
        {
            isRestarted = false;
            SaveValues();
            SetState(LevelState.Init);
        }

        public virtual void LoadCurrentLevel()
        {
            if (CalculatedCurrentLevel() == prevLoadedLevelIndex && !isRestarted)
                SetNextLevel();

            prevLoadedLevelIndex = CalculatedCurrentLevel();
            LoadLevel();
            startByLevelNumber = false;
        }

        [Button("LOAD NEXT LEVEL")]
        public void LoadNextLevel()
        {
            SetNextLevel();
            LoadCurrentLevel();
        }

        protected int CalculatedCurrentLevel()
        {
            if (startByLevelNumber) levelCounter = currentLevelIndex = startLevelNumber - 1;
            else currentLevelIndex = SaveManager.LoadValue(Keys.CURRENT_LEVEL, 0);

            if (levelCounter >= maxLevelCount)
            {
                currentLevelIndex = (levelCounter - (LevelOptions.levelToLoopFrom - 1)) %
                                    levelIndexesToUseAfterLoop.Count;
                return levelIndexesToUseAfterLoop[currentLevelIndex];
            }
            else
            {
                currentLevelIndex %= maxLevelCount;
                return levelIndexesToUse[currentLevelIndex];
            }
        }

        public virtual void SetNextLevel()
        {
            levelCounter++;
            currentLevelIndex++;
            SaveValues();
        }

        public virtual void Reset()
        {
            SetState(LevelState.Init);
        }

        public int GetCurrentLevel()
        {
            return currentLevelIndex;
        }

        private void SetLevelIndexLists()
        {
            for (int i = 0; i < maxLevelCount; i++)
            {
                levelIndexesToUse.Add(i);
                levelIndexesToUseAfterLoop.Add(i);
            }

            for (int i = 0; i < LevelOptions.levelToLoopFrom - 1; i++)
            {
                levelIndexesToUseAfterLoop.Remove(i);
            }

            foreach (LevelConfig t in LevelOptions.levelConfigs.Where(t => !t.isEnabled))
            {
                maxLevelCount--;

                int disabledLevelIndex = t.levelNumber - 1;

                if (levelIndexesToUse.Contains(disabledLevelIndex))
                {
                    levelIndexesToUse.Remove(disabledLevelIndex);
                }
                else
                {
                    Debug.LogWarning("There is no 'Level => (" + (disabledLevelIndex + 1) +
                                     ")' so it couldn't be removed from game on RemoteManager LevelConfig !");
                    return;
                }

                if (levelIndexesToUseAfterLoop.Contains(disabledLevelIndex))
                    levelIndexesToUseAfterLoop.Remove(disabledLevelIndex);
            }
        }

        #endregion
    }
}