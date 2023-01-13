using UnityEngine;

[CreateAssetMenu(fileName = "New_GameLevelData", menuName = "ScriptableObjects/Game Level Data", order = 2)]
public class GameLevelData : ScriptableObject
{
	public EnvironmentOptions environmentOption;

	//Level-specific properties or methods can be add
}