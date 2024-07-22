using UnityEditor;
using UnityEngine;
using System.IO;

public class QuickAccessLogEditor : EditorWindow
{
    [MenuItem("Tools/Quick Access Logs")]
    public static void ShowWindow()
    {
        GetWindow<QuickAccessLogEditor>("Quick Access Logs");
    }

    private void OnGUI()
    {
        GUILayout.Label("Quick Access Unity Logs", EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (GUILayout.Button("Open Editor Log"))
        {
            OpenLog(GetEditorLogPath());
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Open Player Log"))
        {
            OpenLog(GetPlayerLogPath());
        }
    }

    private void OpenLog(string path)
    {
        if (File.Exists(path))
        {
            EditorUtility.RevealInFinder(path);
        }
        else
        {
            EditorUtility.DisplayDialog("Log File Not Found", $"The specified log file could not be found: {path}", "OK");
        }
    }

    private string GetEditorLogPath()
    {
#if UNITY_EDITOR_WIN
        return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "Unity/Editor/Editor.log");
#elif UNITY_EDITOR_OSX
        return "~/Library/Logs/Unity/Editor.log";
#else
        return string.Empty;
#endif
    }

    private string GetPlayerLogPath()
    {
#if UNITY_EDITOR_WIN
        return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "Unity/Player.log");
#elif UNITY_EDITOR_OSX
        return "~/Library/Logs/Unity/Player.log";
#else
        return string.Empty;
#endif
    }
}