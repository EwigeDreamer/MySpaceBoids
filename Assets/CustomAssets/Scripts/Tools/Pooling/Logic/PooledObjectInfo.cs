using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MyTools.Pooling
{
    public class PooledObjectInfo
    {
        GameObject m_Obj;
        public GameObject Obj { get { return m_Obj; } }
        IPooledComponent[] m_PooledComps;
        bool m_IsInited = false;
        Action<PooledObjectInfo> m_Deactive;

        public PooledObjectInfo(GameObject obj, Action<PooledObjectInfo> deactive)
        {
            if (obj == null || deactive == null) return;
            obj.SetActive(false);
            var comps = obj.GetComponentsInChildren<IPooledComponent>(true);
            int count = comps.Length;
            for (int i = 0; i < count; ++i)
                comps[i].Deactive += Deactive;
            m_Obj = obj;
            m_PooledComps = comps;
            m_Deactive = deactive;
            m_IsInited = true;
        }

        private void Deactive()
        {
            if (!m_IsInited) return;
            m_Deactive(this);
        }

        public void OnActivation()
        {
            if (!m_IsInited)
            {
#if UNITY_EDITOR
                Debug.LogError(string.Format("{0} is not inited!", typeof(PooledObjectInfo).Name));
#endif
                return;
            }
            m_Obj.SetActive(true);
            var comps = m_PooledComps;
            int count = comps.Length;
            for (int i = 0; i < count; ++i)
                comps[i].OnActivation();
#if UNITY_EDITOR
            //Debug.LogWarning("Active!!!");
#endif
        }
        public void OnDeactivation()
        {
            if (!m_IsInited)
            {
#if UNITY_EDITOR
                Debug.LogError(string.Format("{0} is not inited!", typeof(PooledObjectInfo).Name));
#endif
                return;
            }
            var comps = m_PooledComps;
            int count = comps.Length;
            for (int i = 0; i < count; ++i)
                comps[i].OnDeactivation();
            m_Obj.SetActive(false);
#if UNITY_EDITOR
            //Debug.LogWarning("Deactive!!!");
#endif
        }
        public void SetPosition(Vector3 position)
        {
            if (!m_IsInited)
            {
#if UNITY_EDITOR
                Debug.LogError(string.Format("{0} is not inited!", typeof(PooledObjectInfo).Name));
#endif
                return;
            }
            m_Obj.transform.position = position;
        }
        public void SetRotation(Quaternion rotation)
        {
            if (!m_IsInited)
            {
#if UNITY_EDITOR
                Debug.LogError(string.Format("{0} is not inited!", typeof(PooledObjectInfo).Name));
#endif
                return;
            }
            m_Obj.transform.rotation = rotation;
        }
        public void SetParent(Transform parent)
        {
            if (!m_IsInited)
            {
#if UNITY_EDITOR
                Debug.LogError(string.Format("{0} is not inited!", typeof(PooledObjectInfo).Name));
#endif
                return;
            }
            m_Obj.transform.parent = parent;
        }
    }
}