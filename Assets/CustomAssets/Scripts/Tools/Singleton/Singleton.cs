using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTools.Singleton
{
    public abstract class Singleton<TMe> where TMe : Singleton<TMe>, new()
    {
        static object m_SyncRoot = new object();
        static TMe m_I = null;
        public static TMe I
        {
            get
            {
                if (m_I == null) lock (m_SyncRoot) m_I = m_I ?? new TMe();
                return m_I;
            }
        }
    }

    public abstract class Multiton<TMe> where TMe : Multiton<TMe>, new()
    {
        static Dictionary<string, TMe> m_We = new Dictionary<string, TMe>();
        public static TMe GetMe(string id)
        {
            TMe me;
            if (m_We.TryGetValue(id, out me)) return me;
            lock (m_We)
            {
                if (m_We.TryGetValue(id, out me)) return me;
                me = new TMe();
                m_We[id] = me;
                return me;
            }
        }
    }
}