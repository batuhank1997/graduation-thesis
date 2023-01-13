using UnityEngine;

/// <summary>
/// Level environments can be change via this SO.
/// To do this, assign a different EnvironmentOptionsSO to GameLevel is enough.
/// </summary>
[CreateAssetMenu(fileName = "New_Environment", menuName = "ScriptableObjects/Game Environment", order = 3)]
public class EnvironmentOptions : ScriptableObject
{
	public Color bgColor;
	public GameObject levelEnvironment;

	//Environment-specific properties and methods can be add here
}	