using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using MyTools.Extensions.Editor;
using MyTools.Extensions.Reflection;
#endif

namespace MyTools.Pooling
{
    public class ObjectPool : MonoSingleton<ObjectPool>, IPoolKeys
    {
        [SerializeField]
        [HideInInspector]
        List<SubPool> m_Pools = new List<SubPool>();
        Dictionary<string, SubPool> m_PoolsDict = new Dictionary<string, SubPool>();

        string[] IPoolKeys.Keys
        {
            get
            {
                var pools = m_Pools;
                var count = pools.Count;
                string[] keys = new string[count];
                for (int i = 0; i < count; ++i)
                    keys[i] = pools[i].Key;
                return keys;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            var dict = m_PoolsDict;
            var pools = m_Pools;
            int count = m_Pools.Count;
            for (int i = 0; i < count; ++i)
            {
                if (!pools[i].ValidKey) continue;
                m_PoolsDict.Add(pools[i].Key, pools[i]);
                pools[i].Awake();
            }
        }

        public bool TrySpawn(string key, out GameObject obj)
        {
            obj = null;
            if (!m_PoolsDict.TryGetValue(key, out var pool)) return false;
            return pool.TrySpawn(out obj);
        }
        public bool TrySpawn(string key, Vector3 position, Quaternion rotation, Transform parent, out GameObject obj)
        {
            obj = null;
            if (!m_PoolsDict.TryGetValue(key, out var pool)) return false;
            return pool.TrySpawn(position, rotation, parent, out obj);
        }
        public bool Despawn(GameObject obj)
        {
            var pools = m_Pools;
            var count = pools.Count;
            for (int i = 0; i < count; ++i)
                if (pools[i].Despawn(obj))
                    return true;
            return false;
        }
        public void CreatePool(GameObject reference, int count, string key)
        {
            var pool = new SubPool(reference, count, key, this);
            m_PoolsDict.Add(key, pool);
            m_Pools.Add(pool);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ObjectPool))]
    public class ObjectPoolEditor : Editor
    {
        ReorderableList m_RList = null;
        private void OnEnable()
        {
            m_RList = new ReorderableList(serializedObject, serializedObject.FindProperty("m_Pools"), true, true, true, true);
            m_RList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Subpools");
            };
            m_RList.elementHeightCallback = (index) =>
            {
                return EditorGUI.GetPropertyHeight(m_RList.serializedProperty.GetArrayElementAtIndex(index));
            };
            m_RList.drawElementCallback = (rect, index, isAcrive, isFocused) =>
            {
                var prop = m_RList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, prop);
            };
            m_RList.onAddCallback = (list) =>
            {
                var index = list.serializedProperty.arraySize;
                list.serializedProperty.arraySize++;
                list.index = index;
                var prop = list.serializedProperty.GetArrayElementAtIndex(index);

                prop.FindPropertyRelative("m_Key").stringValue = string.Empty;
                prop.FindPropertyRelative("m_Container").objectReferenceValue = null;
                prop.FindPropertyRelative("m_Reference").objectReferenceValue = null;
                prop.FindPropertyRelative("m_Count").intValue = 10;
                prop.FindPropertyRelative("m_IfPoolEmpty").enumValueIndex = 0;
                prop.FindPropertyRelative("m_Objs").arraySize = 0;
                prop.FindPropertyRelative("m_CashedInEditor").boolValue = false;
                prop.FindPropertyRelative("m_PoolCount").intValue = 0;
                prop.FindPropertyRelative("m_PoolCtrl").objectReferenceValue = target;
            };
            m_RList.onRemoveCallback = (list) => {
                if (EditorUtility.DisplayDialog(" Warning! ",
                    "Are you sure you want to delete the SubPool?", "Yes", "No"))
                {
                    var prop = m_RList.serializedProperty.GetArrayElementAtIndex(list.index);
                    prop.GetTargetObjectOfProperty().InvokePrivateMethod("RemoveCashedObjects");
                    ReorderableList.defaultBehaviours.DoRemoveButton(list);
                }
            };
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            m_RList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
