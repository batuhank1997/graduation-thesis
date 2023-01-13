using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Managers
{
    public class LevelGenerator : MonoBehaviour
    {
        [Header("Properties")] [SerializeField]
        public Transform levelParent;

        [SerializeField] private LevelManagerSOMode levelManager;

        public void GenerateLevel(GameLevelData level)
        {
            ClearPreviousLevelObjects();

            Instantiate(level.environmentOption.levelEnvironment, levelParent);
            //Other necessary instantiating processes can be implement here
        }

        public void ClearPreviousLevelObjects()
        {
            if (levelParent.childCount <= 0) return;

            List<Transform> allChildren = levelParent.Cast<Transform>().ToList();

            foreach (Transform t in allChildren)
            {
                if (!Application.isPlaying)
                {
                    DestroyImmediate(t.gameObject);
                }
                else
                {
                    Destroy(t.gameObject);
                }
            }
        }
    }
}