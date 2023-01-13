using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class LevelConfigRemoteData
{
    public List<LevelConfig> levelConfigs = new List<LevelConfig>();
	[Range(1, 500)]
    public int levelToLoopFrom;

    public bool CheckIsLevelEnabled(int index)
    {
		foreach (var levelConfig in levelConfigs)
		{
			if (levelConfig.levelNumber == index + 1)
			{
				return levelConfig.isEnabled;
			}
		}

		return true;
	}
}