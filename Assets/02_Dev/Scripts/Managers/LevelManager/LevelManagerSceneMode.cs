using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
	/// <summary>
	/// Load levels as scenes.
	/// WARNING 1 : Uncheck the scene named "SOModeLevelScene" from Build Settings if you use SceneMode !
	/// WARNING 2 : "Don't forget the add your LevelScenes to Build Settings if you if you use SceneMode ! 
	/// 4 LevelScene has been as Example already added to BuildSettings.
	/// </summary>
	public class LevelManagerSceneMode : LevelManager
	{
		private const int nonLevelSceneCount = 2;

		private void Awake()
		{
			Reset();
			LoadValues();
			SetMaxLevelCount();
			DontDestroyOnLoad(gameObject);
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

			LoadLevelScene(CalculatedCurrentLevel());
			OnLevelLoaded?.Invoke();

			Debug.Log("Level Loaded Successfully from Scene Mode ! ");
		}

		private void LoadLevelScene(int index)
		{
			int sceneIndex = index + nonLevelSceneCount;
			SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
		}

		public override void SetMaxLevelCount()
		{
			maxLevelCount = SceneManager.sceneCountInBuildSettings - nonLevelSceneCount;
			OnMaxLevelCountSet();
		}
	}
}