using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class PlayerPrefsCleaner
{
    const string m_ToolPath = "MyAssets/Tools/PlayerPrefs/";

    [MenuItem(m_ToolPath + "Clear PlayerPrefs")]
    static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    [MenuItem(m_ToolPath + "Clear EditorPrefs")]
    static void ClearEditorPrefs()
    {
        EditorPrefs.DeleteAll();
    }
}
