#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class GameToolEditor : EditorWindow
{
    static Vector2 scrollPos = Vector2.zero;
    //private static EgoEditorGUIUtils.PopUpData sceneSelectData = new EgoEditorGUIUtils.PopUpData("");
    //private static List<PopUpDataCollection> sceneList;
    private static bool timeScaleEnable;

    [MenuItem("Miao/Game Tool", false, 3)]
    public static void ShowEditor()
    {
        EditorWindow.GetWindow<GameToolEditor>(false, "Game Tool", true).Show();
    }

    void OnGUI()
    {
        DrawOnGUI();
    }

    public static void DrawOnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos, false, false);

        EditorGUILayout.BeginVertical();

        DrawTimeTool();
        EditorGUILayout.EndVertical();
        GUILayout.EndScrollView();
    }

    static void DrawTimeTool()
    {
        //EgoEditorGUIUtils.BeginContents();

        timeScaleEnable = EditorGUILayout.Toggle("调节速率", timeScaleEnable);
        bool enable = GUI.enabled;
        GUI.enabled = timeScaleEnable;
        var timeScale = Time.timeScale;
        var newScale = EditorGUILayout.FloatField("游戏速率：", timeScale);
        if (timeScaleEnable)
        {
            if (timeScale != newScale)
            {
                timeScale = timeScale > 100 ? 100 : timeScale;
                Time.timeScale = newScale;
            }
        }
        else
        {
            Time.timeScale = 1;

        }
        GUI.enabled = enable;

        //EgoEditorGUIUtils.EndContents();
    }
}
#endif