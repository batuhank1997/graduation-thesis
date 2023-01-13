using UnityEditor;
using UnityEngine;

public class ReplaceSel : EditorWindow
{
	private GameObject myObject;
	private static float xAngle, yAngle, zAngle;
	private bool customRotationCheck;
	private Vector3 customRotation;
	private Rect customValueCheckRect;

	[MenuItem("Tools/ReplaceSelected %q")]

	public static void ReplaceObjects()
	{

		GetWindow(typeof(ReplaceSel));
	}

	[System.Obsolete]
	void OnGUI()
	{
		GUILayout.Label("Set An Object To Replacement", EditorStyles.boldLabel);

		myObject = EditorGUILayout.ObjectField(myObject, typeof(GameObject), true) as GameObject;

		GUILayout.Label("Custom Rotation Of New Prefab", EditorStyles.boldLabel);

		// Get two lines of rects for the control
		Rect position = EditorGUILayout.GetControlRect(false,  2 * EditorGUIUtility.singleLineHeight);
		Rect position2 = EditorGUILayout.GetControlRect(false, 2 * EditorGUIUtility.singleLineHeight);
		Rect position3 = EditorGUILayout.GetControlRect(false, 2 * EditorGUIUtility.singleLineHeight);

		//Using 3 pos for same calculation seems to be unnecessery, 
		//But if it use only one pos then everything is overlapping on the GUI window.
		position.height  *= 0.5f;
		position2.height *= 0.5f;
		position3.height *= 0.5f;

		xAngle = EditorGUI.Slider(position, "Set X Rotation", xAngle, 0, 360);
		yAngle = EditorGUI.Slider(position2, "Set Y Rotation", yAngle, 0, 360);
		zAngle = EditorGUI.Slider(position3, "Set Z Rotation", zAngle, 0, 360);

		position.y += position.height;
		position.x += EditorGUIUtility.labelWidth;
		position.width -= EditorGUIUtility.labelWidth + 54;

		position2.y += position2.height;
		position2.x += EditorGUIUtility.labelWidth;
		position2.width -= EditorGUIUtility.labelWidth + 54;

		position3.y += position3.height;
		position3.x += EditorGUIUtility.labelWidth;
		position3.width -= EditorGUIUtility.labelWidth + 54;

		GUIStyle style  = GUI.skin.label;
		style.alignment = TextAnchor.UpperLeft;  EditorGUI.LabelField(position,  "0", style);
		style.alignment = TextAnchor.UpperRight; EditorGUI.LabelField(position,  "360", style);
		style.alignment = TextAnchor.UpperLeft;  EditorGUI.LabelField(position2, "0", style);
		style.alignment = TextAnchor.UpperRight; EditorGUI.LabelField(position2, "360", style);
		style.alignment = TextAnchor.UpperLeft;  EditorGUI.LabelField(position3, "0", style);
		style.alignment = TextAnchor.UpperRight; EditorGUI.LabelField(position3, "360", style);

		customValueCheckRect = new Rect(250, 250, 200, 50);
		customRotationCheck  = GUI.Toggle(customValueCheckRect, customRotationCheck, "Use My Custom Values");

		if (GUILayout.Button("Replace!", GUILayout.Height(50)))
		{
			customRotation = new Vector3(xAngle,yAngle,zAngle);

			if (myObject != null)
			{

				foreach (Transform t in Selection.transforms)
				{

					GameObject o = null;
					o = PrefabUtility.GetCorrespondingObjectFromSource(myObject) as GameObject;

					if (PrefabUtility.GetPrefabType(myObject).ToString() == "PrefabInstance")
					{

						o = (GameObject)PrefabUtility.InstantiatePrefab(o);
						PrefabUtility.SetPropertyModifications(o, PrefabUtility.GetPropertyModifications(myObject));
					}

					else if (PrefabUtility.GetPrefabType(myObject).ToString() == "Prefab")
					{

						o = (GameObject)PrefabUtility.InstantiatePrefab(myObject);
					}

					else
					{

						o = Instantiate(myObject) as GameObject;
					}

					Undo.RegisterCreatedObjectUndo(o, "created prefab");

					Transform newT = o.transform;

					if (t != null && customRotationCheck == false)
					{

						newT.position = t.position;
						newT.rotation = t.rotation;
						newT.localScale = t.localScale;
						newT.parent = t.parent;
					}
					else
					{
						newT.position = t.position;
						newT.rotation = Quaternion.Euler(customRotation);
						newT.localScale = t.localScale;
						newT.parent = t.parent;
					}
				}

				foreach (GameObject go in Selection.gameObjects)
				{
					Undo.DestroyObjectImmediate(go);
				}
			}
		}
	}
}