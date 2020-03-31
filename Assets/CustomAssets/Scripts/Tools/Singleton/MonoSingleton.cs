namespace MyTools.Singleton
{
    using UnityEngine;
    using MyTools.Helpers;
    using MyTools.Singleton.Managed;

    public abstract class MonoSingleton<TMe> : ManagedSingleton where TMe : MonoSingleton<TMe>
    {
        [SerializeField] bool m_OverrideSingleton = true;
        [SerializeField] bool m_MakeDontDestroy = false;

        static TMe m_I = null;
        public static TMe I => m_I;

        bool InitSingleton()
        {
            if (!m_OverrideSingleton && m_I != null) return false;
            m_I = (TMe)this;
            if (m_MakeDontDestroy) MakeDontDestroy();
            return true;
        }
        void RemoveSingleton()
        {
            if (m_I == null || m_I.GO != GO) return;
            m_I = null;
        }

        protected virtual void Awake()
        {
            if (InitSingleton()) enabled = true;
            else enabled = false;
        }
        protected virtual void OnDestroy()
        {
            RemoveSingleton();
        }

        protected void MakeDontDestroy()
        {
            TR.SetParent(null);
            DontDestroyOnLoad(GO);
        }

        public override void Refresh()
        {
            var overrideTmp = m_OverrideSingleton;
            m_OverrideSingleton = true;
            InitSingleton();
            m_OverrideSingleton = overrideTmp;
        }
    }

    namespace Managed
    {
        public abstract class ManagedSingleton : MonoValidate
        { public abstract void Refresh(); }
    }
}
