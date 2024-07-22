using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class ChangePrefabFontsEditor : EditorWindow
{
    public Font newFont;
    public TMP_FontAsset newTMPFont;

    [MenuItem("Tools/Change Prefab Fonts")]
    public static void ShowWindow()
    {
        GetWindow<ChangePrefabFontsEditor>("Change Prefab Fonts");
    }

    void OnGUI()
    {
        GUILayout.Label("Change All Fonts in Prefabs", EditorStyles.boldLabel);
        newFont = (Font)EditorGUILayout.ObjectField("New Font", newFont, typeof(Font), false);
        newTMPFont = (TMP_FontAsset)EditorGUILayout.ObjectField("New TMP Font", newTMPFont, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("Change Fonts in Prefabs"))
        {
            if (newFont == null && newTMPFont == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a new font.", "OK");
                return;
            }

            ChangeFontsInPrefabs();
        }
    }

    void ChangeFontsInPrefabs()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            bool isModified = false;

            Text[] textComponents = prefab.GetComponentsInChildren<Text>(true);
            foreach (Text text in textComponents)
            {
                if (newFont != null)
                {
                    Undo.RecordObject(text, "Change Font in Prefab");
                    text.font = newFont;
                    EditorUtility.SetDirty(text);
                    isModified = true;
                }
            }

            TextMeshProUGUI[] tmpTextComponents = prefab.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (TextMeshProUGUI text in tmpTextComponents)
            {
                if (newTMPFont != null)
                {
                    Undo.RecordObject(text, "Change TMP Font in Prefab");
                    text.font = newTMPFont;
                    EditorUtility.SetDirty(text);
                    isModified = true;
                }
            }

            if (isModified)
            {
                PrefabUtility.SavePrefabAsset(prefab);
            }
        }

        EditorUtility.DisplayDialog("Success", "All fonts in prefabs have been changed.", "OK");
    }
}
