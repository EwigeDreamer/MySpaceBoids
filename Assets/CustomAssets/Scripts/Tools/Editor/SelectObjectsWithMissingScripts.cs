using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using MyTools.Extensions.GameObjects;

public static class SelectObjectsWithMissingScripts
{
    const string m_ToolPath = "MyAssets/Tools/Missing Scripts/";

    [MenuItem(m_ToolPath + "Find objects with missing scripts in scenes %&m", false, 3)]
    static void FinfObjectsInScene()
    {
        var count = SceneManager.sceneCount;
        var scenes = new Scene[count];
        for (int i = 0; i < count; ++i)
            scenes[i] = SceneManager.GetSceneAt(i);

        var sickObjs = new List<GameObject>();
        var buffer = new List<Component>();

        foreach (var scene in scenes)
        {
            if (!scene.isLoaded) continue;
            var rootObjs = scene.GetRootGameObjects();
            foreach (var obj in rootObjs)
                CheckChildren(obj.transform, sickObjs, buffer);
        }
        if (sickObjs.Count < 1)
        {
            Debug.Log("No GameObjects in opened scenes have missing scripts! Yay!");
            return;
        }
        foreach (var sickObj in sickObjs)
        {
            Debug.LogWarning(string.Format("Missing scripts in {0}/{1}", sickObj.scene.name, sickObj.GetPath()));
        }
        Selection.objects = sickObjs.ToArray();
    }

    [MenuItem(m_ToolPath + "Find objects with missing scripts in selected %#&m", false, 4)]
    static void FinfObjectsInSelected()
    {
        var sickObjs = new List<GameObject>();
        var curScene = SceneManager.GetActiveScene();
        var buffer = new List<Component>();
        var selObjs = Selection.gameObjects;
        foreach (var obj in selObjs)
            CheckChildren(obj.transform, sickObjs, buffer);
        if (sickObjs.Count < 1)
        {
            Debug.Log("No GameObjects in '" + curScene.name + "' have missing scripts! Yay!");
            return;
        }
        foreach (var sickObj in sickObjs)
        {
            Debug.LogWarning("Missing scripts in " + sickObj.GetPath());
        }
        Selection.objects = sickObjs.ToArray();
    }

    static void CheckChildren(Transform tr, List<GameObject> sickObjs, List<Component> buffer)
    {
        buffer.Clear();
        tr.GetComponents(buffer);
        foreach (var comp in buffer)
            if (comp == null)
            {
                sickObjs.Add(tr.gameObject);
                break;
            }
        foreach (Transform child in tr)
            CheckChildren(child, sickObjs, buffer);
    }
}
