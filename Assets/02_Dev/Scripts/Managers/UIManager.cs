using TMPro;
using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Longhorn.UIViewManager;
using System.Collections.Generic;
using DG.Tweening;
using ElephantSDK;
using UnityEngine.UI;

namespace Managers
{
    public enum UIState
    {
        Init,
        Won,
        Lost
    }

    public class UIManager : ManagerBase<UIManager, UIState>
    {
        [Header("IN GAME SPECIFIC ELEMENTS")]
        [SerializeField] private Score score;
        [SerializeField] private TextMeshProUGUI levelNumberText;
        [SerializeField] private Image progressBar;
        [SerializeField] private Image progressBarBG;
        [SerializeField] private Image limitImage;
        
        private void Awake()
        {
            OnStateChange += OnStateChanged;
        }

        private void Start()
        {
            SetState(UIState.Init);
            LonghornConfigSetUp(GameManager.I.GetLonghornConfig());
        }

        public void SetLimitImagePosition(float offset)
        {
            limitImage.transform.localPosition = new Vector3(-215 + (offset * 4), 15, 0);
        }
        
        public void SetLeftVolumeImage(float leftVolume, float totalMeshVolume)
        {
            progressBar.fillAmount = leftVolume / totalMeshVolume;
            DOTween.To(()=> progressBarBG.fillAmount, x=> progressBarBG.fillAmount = x, leftVolume / totalMeshVolume, 0.5f);
        }

        public void ResetProgressBar()
        {
            progressBar.fillAmount = 1;
            progressBarBG.fillAmount = 1;
        }

        private void LonghornConfigSetUp(LonghornConfig longhornConfig)
        {
            if (longhornConfig == null) return;
            
            if (longhornConfig.sceneModeEnabled)
                SetLevelNumberFromLevelData();
        }

        private void OnDestroy()
        {
            OnStateChange -= OnStateChanged;
        }

        // Gets invoked when state changes via ManagerBase.SetState method
        private void OnStateChanged()
        {
            switch (State)
            {
                case UIState.Init:
                    ViewManager.I.SetVisibilityByOptions(
                        VisibilityOptions.New
                         .Show(new List<Type>() { typeof(GamePanelView)} )
                         .Hide(new List<Type>() { typeof(WinPanelView), typeof(LostPanelView) })
                    );
                    break;

                case UIState.Won:
                    ViewManager.I.SetVisibilityByOptions(
                        VisibilityOptions.New
                         .Show(new List<Type>() { typeof(WinPanelView)} )
                         .Hide(new List<Type>() { typeof(GamePanelView), typeof(LostPanelView) })
                    );
                    Elephant.LevelCompleted(LevelManager.I.levelCounter + 1);

                    break;

                case UIState.Lost:
                    ViewManager.I.SetVisibilityByOptions(
                        VisibilityOptions.New
                         .Show(new List<Type>() { typeof(LostPanelView)} )
                         .Hide(new List<Type>() { typeof(GamePanelView), typeof(WinPanelView) })
                    );
                    Elephant.LevelFailed(LevelManager.I.levelCounter + 1);
                    break;

                default:
                    ViewManager.I.SetVisibilityByOptions(
                        VisibilityOptions.New
                         .Show(new List<Type>() { typeof(GamePanelView)} )
                         .Hide(new List<Type>() { typeof(LostPanelView), typeof(WinPanelView) })
                    );
                    break;
            }
        }
        
        [Button]
        private void TestState(UIState state) {
            SetState(state);
        }
        
        #region Score Methods

        public void IncreaseScore(int amount) => score.IncreaseScore(amount);

        public void DecreaseScore(int amount) => score.DecreaseScore(amount);

        #endregion
        
        #region Level Number Methods

        public void SetLevelNumberFromLevelData()
        {
            Debug.LogWarning("CALLED!");
            SetLevelNumberText(LevelManager.I.levelCounter + 1);
        }

        private void SetLevelNumberText(int levelNumber)
        {
            levelNumberText.text = "Level " + levelNumber;
        }

        #endregion
    }
}