using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;

namespace MyTools.Helpers
{
    public abstract class MonoBinderSingleton<TMe, T1> : MonoSingleton<TMe>
        where TMe : MonoBinderSingleton<TMe, T1>
        where T1 : MonoBehaviour
    {
        [SerializeField] T1 m_BindedObj1;
        protected T1 BindedObj1 => m_BindedObj1;
        protected override void OnValidate()
        {
            base.OnValidate();
            ValidateFind(ref m_BindedObj1);
        }
    }

    public abstract class MonoBinderSingleton<TMe, T1, T2> : MonoSingleton<TMe>
        where TMe : MonoBinderSingleton<TMe, T1, T2>
        where T1 : MonoBehaviour
        where T2 : MonoBehaviour
    {
        [SerializeField] T1 m_BindedObj1;
        [SerializeField] T2 m_BindedObj2;
        protected T1 BindedObj1 => m_BindedObj1;
        protected T2 BindedObj2 => m_BindedObj2;
        protected override void OnValidate()
        {
            base.OnValidate();
            ValidateFind(ref m_BindedObj1);
            ValidateFind(ref m_BindedObj2);
        }
    }
    public abstract class MonoBinderSingleton<TMe, T1, T2, T3> : MonoSingleton<TMe>
        where TMe : MonoBinderSingleton<TMe, T1, T2, T3>
        where T1 : MonoBehaviour
        where T2 : MonoBehaviour
        where T3 : MonoBehaviour
    {
        [SerializeField] T1 m_BindedObj1;
        [SerializeField] T2 m_BindedObj2;
        [SerializeField] T3 m_BindedObj3;
        protected T1 BindedObj1 => m_BindedObj1;
        protected T2 BindedObj2 => m_BindedObj2;
        protected T3 BindedObj3 => m_BindedObj3;
        protected override void OnValidate()
        {
            base.OnValidate();
            ValidateFind(ref m_BindedObj1);
            ValidateFind(ref m_BindedObj2);
            ValidateFind(ref m_BindedObj3);
        }
    }
    public abstract class MonoBinderSingleton<TMe, T1, T2, T3, T4> : MonoSingleton<TMe>
        where TMe : MonoBinderSingleton<TMe, T1, T2, T3, T4>
        where T1 : MonoBehaviour
        where T2 : MonoBehaviour
        where T3 : MonoBehaviour
        where T4 : MonoBehaviour
    {
        [SerializeField] T1 m_BindedObj1;
        [SerializeField] T2 m_BindedObj2;
        [SerializeField] T3 m_BindedObj3;
        [SerializeField] T4 m_BindedObj4;
        protected T1 BindedObj1 => m_BindedObj1;
        protected T2 BindedObj2 => m_BindedObj2;
        protected T3 BindedObj3 => m_BindedObj3;
        protected T4 BindedObj4 => m_BindedObj4;
        protected override void OnValidate()
        {
            base.OnValidate();
            ValidateFind(ref m_BindedObj1);
            ValidateFind(ref m_BindedObj2);
            ValidateFind(ref m_BindedObj3);
            ValidateFind(ref m_BindedObj4);
        }
    }
}