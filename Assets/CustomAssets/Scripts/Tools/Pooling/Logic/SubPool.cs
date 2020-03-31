using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Pooling.Base;
using MyTools.Extensions.GameObjects;
#if UNITY_EDITOR
using UnityEditor;
using MyTools.Extensions.Rects;
using MyTools.Extensions.Editor;
using MyTools.Extensions.Reflection;
#endif

namespace MyTools.Pooling
{
    public enum PoolBehaviour { CreateNew, ReuseActive, DoNothing }
    
    [System.Serializable]
    public class SubPool
    {
#pragma warning disable 649
        Pool<PooledObjectInfo> m_Pool;
        Dictionary<GameObject, PooledObjectInfo> m_InfoDict = new Dictionary<GameObject, PooledObjectInfo>();
        [SerializeField] ObjectPool m_PoolCtrl;
        [SerializeField] string m_Key;
        public string Key { get { return m_Key; } }
        [SerializeField] Transform m_Container = null;
        [SerializeField] GameObject m_Reference;
        [SerializeField] int m_Count;
        [SerializeField] PoolBehaviour m_IfPoolEmpty;
        [SerializeField] [HideInInspector] List<GameObject> m_Objs;
        [SerializeField] bool m_CashedInEditor;
        [SerializeField] int m_PoolCount = 0;
        bool m_IsInited = false;
#pragma warning restore 649

        public bool ValidKey { get { return !(string.IsNullOrEmpty(m_Key) || string.IsNullOrWhiteSpace(m_Key)); } }
        bool ValidObjects { get { return m_Objs != null && m_Objs.Count > 0; } }

        public SubPool() { }

        public SubPool(GameObject reference, int count, string key, ObjectPool controller = null)
        {
            m_Key = key;
            if (reference == null || count < 1 || !ValidKey) return;
            m_PoolCtrl = controller;
            CreateContainer();
            var refer = Object.Instantiate(reference);
            refer.SetActive(false);
            refer.name = string.Format("{0} [{1}][{2}]", reference.name, m_Key, "REFERENCE");
            refer.transform.parent = m_Container;
            m_Reference = refer;
            m_Count = count;
            Awake();
        }

        void CreateCashedObjects()
        {
            if (m_Reference == null) return;
            if (m_Count < 1) return;
            if (ValidObjects) return;
            if (!ValidKey) return;
            CreateContainer();
            InstatiateObjs(m_Reference, m_Count, m_Container, ref m_Objs);
            m_CashedInEditor = true;
        }
        void RemoveCashedObjects()
        {
            DestroyObjs(m_Objs);
            if (m_Container != null) Object.DestroyImmediate(m_Container.gameObject);
            m_CashedInEditor = false;
        }

        void CreateContainer()
        {
            if (m_Container != null) return;
            var con = new GameObject(string.Format("POOL_CONTAINER [{0}]", m_Key));
            if (m_PoolCtrl != null)
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(con, m_PoolCtrl.gameObject.scene);
            m_Container = con.transform;
        }

        void InstatiateObjs(GameObject refer, int count, Transform parent, ref List<GameObject> list)
        {
            if (refer == null) return;
            if (count < 1) return;
            DestroyObjs(list);
            list = new List<GameObject>(count);
            for (int i = 0; i < count; ++i) list.Add(CreateObject(refer));
            list.SetActive(false);
            list.SetParent(parent);
        }
        void DestroyObjs(List<GameObject> list)
        {
            if (list == null) return;
            int count = list.Count;
            for (int i = 0; i < count; ++i) Object.DestroyImmediate(list[i]);
            list.Clear();
        }
        GameObject CreateObject(GameObject refer)
        {
            GameObject obj;
#if UNITY_EDITOR
            if (Application.isPlaying)
                obj = Object.Instantiate(refer);
            else
                obj = (GameObject)PrefabUtility.InstantiatePrefab(refer);
#else
            obj = Object.Instantiate(refer);
#endif
            obj.name = string.Format("{0} [POOLED][{1}][{2}]", refer.name, m_Key, obj.GetInstanceID());
            return obj;
        }

        public void Awake()
        {
            if (m_IsInited) return;
            if (!ValidKey) return;
            if (!m_CashedInEditor) CreateCashedObjects();
            if (!ValidObjects) return;
            var dict = m_InfoDict;
            var objs = m_Objs;
            int count = objs.Count;
            var objInfs = new PooledObjectInfo[count];
            for (int i = 0; i < count; ++i)
            {
                objInfs[i] = new PooledObjectInfo(objs[i], Deactive);
                dict.Add(objInfs[i].Obj, objInfs[i]);
            }
            var pool = new Pool<PooledObjectInfo>(objInfs);
            m_PoolCount = pool.TotalCount;
            pool.OnDeactivation += OnDeactivationEvent;
            m_Pool = pool;
            m_Objs = null;
            m_IsInited = true;
        }

        private void OnDeactivationEvent(PooledObjectInfo objInfo)
        {
            objInfo?.SetParent(m_Container);
        }

        public bool TrySpawn(out GameObject obj)
        {
            obj = null;
            if (!TryGetObjectInfo(out var info)) return false;
            obj = info.Obj;
            if (obj != null)
            {
                info.SetPosition(Vector3.zero);
                info.SetRotation(Quaternion.identity);
                info.SetParent(null);
                info.OnActivation();
                return true;
            }
            return false;
        }
        public bool TrySpawn(Vector3 position, Quaternion rotation, Transform parent, out GameObject obj)
        {
            obj = null;
            if (!TryGetObjectInfo(out var info)) return false;
            obj = info.Obj;
            if (obj != null)
            {
                info.SetPosition(position);
                info.SetRotation(rotation);
                info.SetParent(parent);
                info.OnActivation();
                return true;
            }
            return false;
        }

        bool TryGetObjectInfo(out PooledObjectInfo info)
        {
            info = null;
            var pool = m_Pool;
            if (pool == null) return false;
            if (pool.InactiveCount > 0)
                info = pool.TakeFromInactiveObjs();
            else
            {
                var beh = m_IfPoolEmpty;
                if (beh == PoolBehaviour.CreateNew)
                {
                    var refer = m_Reference;
                    if (refer == null) return false;
                    var newInf = new PooledObjectInfo(CreateObject(refer), Deactive);
                    m_InfoDict.Add(newInf.Obj, newInf);
                    newInf.SetParent(m_Container);
                    pool.Add(newInf);
                    info = pool.TakeFromInactiveObjs();
                }
                else if (beh == PoolBehaviour.ReuseActive)
                {
                    info = pool.TakeFromActiveObjs();
                }
            }
            return info != null;
        }

        public bool Despawn(GameObject obj)
        {
            if (obj == null) return false;
            if (!m_InfoDict.TryGetValue(obj, out var info)) return false;
            Deactive(info);
            return true;
        }

        public bool Contains(GameObject obj)
        {
            if (obj == null) return false;
            return m_InfoDict.ContainsKey(obj);
        }

        void Deactive(PooledObjectInfo objInfo)
        {
            objInfo.OnDeactivation();
            m_Pool.ReturnToPool(objInfo);
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SubPool))]
    public class SubPoolEditor : PropertyDrawer
    {
        float LineHeight => EditorGUIUtility.singleLineHeight;
        float LineSpacing => EditorGUIUtility.standardVerticalSpacing;
        float LabelWidth => EditorGUIUtility.labelWidth;
        const float c_Spacing = 8f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int lines = 6;
            return (LineHeight * lines) + (LineSpacing * (lines - 1)) + c_Spacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.yMin += c_Spacing / 2f;
            position.yMax -= c_Spacing / 2f;


            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            
            position.GetCustomLabelFieldPair(10f, out var space, out var field);
            GUI.Box(field, "");
            field.GetRowsNonAlloc(LineSpacing, out var rect1, out var rect2, out var rect3, out var rect4, out var rect5, out var rect6);
            rect5.GetColumnsNonAlloc(LineSpacing, out var rect5_rect1, out var rect5_rect2);


            float labWidthTmp = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 100f;

            var cashFlagSerProp = property.FindPropertyRelative("m_CashedInEditor");
            bool cashedInEditor = cashFlagSerProp.boolValue;
            var runTime = Application.isPlaying;

            EditorGUI.BeginDisabledGroup(cashedInEditor || runTime);
            EditorGUI.PropertyField(rect1, property.FindPropertyRelative("m_Key"));
            EditorGUI.PropertyField(rect2, property.FindPropertyRelative("m_Reference"));
            EditorGUI.PropertyField(rect3, property.FindPropertyRelative("m_Count"));
            EditorGUI.EndDisabledGroup();
            EditorGUI.PropertyField(rect4, property.FindPropertyRelative("m_IfPoolEmpty"));
            
            EditorGUI.BeginDisabledGroup(cashedInEditor || runTime);
            if (GUI.Button(rect5_rect1, "Create cash")) property.GetTargetObjectOfProperty().InvokePrivateMethod("CreateCashedObjects");
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(!cashedInEditor || runTime);
            if (GUI.Button(rect5_rect2, "Remove cash")) property.GetTargetObjectOfProperty().InvokePrivateMethod("RemoveCashedObjects");
            EditorGUI.EndDisabledGroup();

            if (runTime)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.LabelField(rect6, "Don't edit in RunTime");
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                var cashArrSerProp = property.FindPropertyRelative("m_Objs");
                int cashCount = cashArrSerProp.arraySize;
                EditorGUI.LabelField(rect6, string.Format("Cashed objects: {0}", cashCount));
            }

            EditorGUIUtility.labelWidth = labWidthTmp;
            EditorGUI.indentLevel = indentLevel;
        }
    }
#endif

}
