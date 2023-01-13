using UnityEngine;

/// <summary>
/// Shows current FPS 
/// </summary>
public class FPSDisplay : MonoBehaviour
{
	[SerializeField] private Color displayColor;
	[SerializeField] private TextAnchor displayPosition;

	private float deltaTime = 0.0f;

	private void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
	}

	private void OnGUI()
	{
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0f, 0f, w, h * 2f / 100f);
		style.alignment = displayPosition;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = displayColor;
		float mSecond = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = $"{mSecond:0.0} ms ({fps:0.} fps)";
		GUI.Label(rect, text, style);
	}
}