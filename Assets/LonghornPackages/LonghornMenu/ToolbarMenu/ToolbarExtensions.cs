#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public class ToolbarExtensions
{
    static ToolbarExtensions()
    {
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUILeft);
        ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUIRight);
    }

    private static void OnToolbarGUIRight() {
        
        if(GUILayout.Button(new GUIContent("Clear ES Save", "Removes ES Save Data")))
        {
            ES3.DeleteFile();
        }
        
        if(GUILayout.Button(new GUIContent("x1", "")))
        {
            Time.timeScale = 1;
        }
        
        if(GUILayout.Button(new GUIContent("x2", "")))
        {
            Time.timeScale = 2;
        }
        
        if(GUILayout.Button(new GUIContent("x4", "")))
        {
            Time.timeScale = 4;
        }
        
        GUILayout.FlexibleSpace();
    }

    static void OnToolbarGUILeft()
    {
        GUILayout.FlexibleSpace();

        if(GUILayout.Button(new GUIContent("Elephant Scene", "Opens Elephant Scene")))
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene("Assets/Elephant/elephant_scene.unity");
        }

        if(GUILayout.Button(new GUIContent("Start Scene", "Opens Start Scene")))
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene("Assets/03_Scenes/StartScene.unity");
        }
        
        if(GUILayout.Button(new GUIContent("Scene 1", "Opens Scene 1")))
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene("Assets/03_Scenes/Scene Mode/Scene 1.unity");
        }
        
        if(GUILayout.Button(new GUIContent("SO Mode Scene", "Opens SO Mode Scene")))
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene("Assets/03_Scenes/SO Mode/SOModeLevelScene.unity");
        }
    }
}

#endif