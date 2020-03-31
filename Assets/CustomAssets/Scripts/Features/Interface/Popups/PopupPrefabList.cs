using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
#if UNITY_EDITOR
using UnityEditor.Build.Reporting;
using UnityEditor;
using UnityEditor.Build;
using System.Linq;
#endif

[CreateAssetMenu(fileName = "PopupPrefabList", menuName = "PopupPrefabList", order = 0)]
public class PopupPrefabList : ScriptableObject
{
    [SerializeField] List<PopupBase> m_Popups = new List<PopupBase>();
    ReadOnlyCollection<PopupBase> m_PopupsReadOnly = null;
    public ReadOnlyCollection<PopupBase> Popups => m_PopupsReadOnly ?? 
        (m_PopupsReadOnly = new ReadOnlyCollection<PopupBase>(m_Popups));

#if UNITY_EDITOR
    void SetList(List<PopupBase> popups)
    { m_Popups = popups; }

    [CustomEditor(typeof(PopupPrefabList))]
    class PopupPrefabListEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUI.Button(EditorGUILayout.GetControlRect(), "Get All Popups"))
            {
                Undo.RecordObject(target, "GetAllPopups");
                var list = PopupPrefabListBuildInitializator.GetAllPopups();
                ((PopupPrefabList)target).SetList(list);
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }
            DrawDefaultInspector();
        }
    }

    class PopupPrefabListBuildInitializator : IPreprocessBuildWithReport
    {
        int IOrderedCallback.callbackOrder => 0;

        void IPreprocessBuildWithReport.OnPreprocessBuild(BuildReport report)
        { SetupPopups(); }

        static void SetupPopups()
        {
            var prefabLists = GetAllPopupLists();
            var popups = GetAllPopups();
            foreach (var list in prefabLists)
            {
                list.SetList(new List<PopupBase>(popups));
                EditorUtility.SetDirty(list);
            }
            AssetDatabase.SaveAssets();
            Debug.Log($"Popups [{popups.Count}] loaded in lists [{prefabLists.Count}]!");
        }

        public static List<PopupBase> GetAllPopups()
        {
            return AssetDatabase.GetAllAssetPaths()
                .Where(path => IsAssetPath(path))
                .Where(path => AssetDatabase.GetMainAssetTypeAtPath(path) == typeof(GameObject))
                .Select(path => AssetDatabase.LoadAssetAtPath<GameObject>(path))
                .Select(go => go.GetComponent<PopupBase>())
                .Where(popup => popup != null)
                .ToList();
        }

        static List<PopupPrefabList> GetAllPopupLists()
        {
            return AssetDatabase.GetAllAssetPaths()
                .Where(path => IsAssetPath(path))
                .Where(path => AssetDatabase.GetMainAssetTypeAtPath(path) == typeof(PopupPrefabList))
                .Select(path => AssetDatabase.LoadAssetAtPath<PopupPrefabList>(path))
                .ToList();
        }

        static bool IsAssetPath(string path) => path.Substring(0, "Assets".Length).Equals("Assets");
    }
#endif
}
