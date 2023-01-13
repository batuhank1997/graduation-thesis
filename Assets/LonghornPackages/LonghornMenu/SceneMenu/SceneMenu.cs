#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class SceneMenu {
    [MenuItem("Longhorn/Scene/CleanStartFromElephantScene %&r")]
    public static void CleanStartFromElephant() {
        ES3.DeleteFile();
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Elephant/elephant_scene.unity");
        EditorApplication.isPlaying = true;
    }
}
#endif