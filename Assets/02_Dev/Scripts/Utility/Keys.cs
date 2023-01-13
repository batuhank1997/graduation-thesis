using UnityEngine;

public class Keys : MonoBehaviour
{
	#region AnimationKeys
	public const string ANIM_RUN = "Run";
	public const string ANIM_IDLE = "Idle";
	public const string ANIM_MODE = "AnimMode";
	#endregion AnimationKeys

	#region Resources
	public const string RESOURCES_PATH_OBJECT_NAME = "";
	#endregion Resources

	#region TagKeys
	public const string TAG_UNTAGGED = "Untagged";
	public const string TAG_PLAYER = "Player";
	public const string TAG_FINISH = "Finish";
	public const string TAG_RESPAWN = "Respawn";
	public const string TAG_EDITOR_ONLY = "EditorOnly";
	public const string TAG_MAIN_CAMERA = "MainCamera";
	public const string TAG_PLATFORM = "Platform";
	public const string TAG_SLICER = "Slicer";
	public const string TAG_EDGE = "Edge";
	public const string TAG_BALL = "Ball";
	public const string TAG_OBSTACLE = "Obstacle";
	#endregion TagKeys

	#region LayerKeys
	public const string LAYER_DEFAULT = "Default";
	public const string LAYER_TRANSPARENT_FX = "TransparentFX";
	public const string LAYER_IGNORE_RAYCAST = "Ignore Raycast";
	public const string LAYER_WATER = "Water";
	public const string LAYER_UI = "UI";
	public const int LAYER_SLICER = 6;
	#endregion LayerKeys

	#region SaveLoadKeys
	public const string PLAYED_LEVEL_COUNT = "playedLevelCount";
	public const string CURRENT_LEVEL = "currentLevel";
	#endregion

	#region OTHER
	public const string EDITOR = "Editor";
	public const string DEFAULT = "Default";
	#endregion OTHER
}