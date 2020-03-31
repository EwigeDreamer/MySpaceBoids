using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTools.Helpers
{
    public abstract class ImprovedBehaviour : MonoBehaviour
    {
        GameObject m_GO = null;
        Transform m_TR = null;

        public GameObject GO => m_GO ?? (m_GO = gameObject);
        public Transform TR => m_TR ?? (m_TR = transform);
    }
}
