using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using MyTools.Extensions.GameObjects;

namespace MyTools.Misc
{
    public class OnBuildDestructor : MonoBehaviour
    {
        [SerializeField] GameObject[] m_Objects;
        [SerializeField] Component[] m_Components;

        private void Awake()
        {
#if !UNITY_EDITOR
            var b = new StringBuilder();
            b.AppendLine(string.Format("Some objects was destroyed by {0}:", typeof(OnBuildDestructor).Name));
            var comps = m_Components;
            for (int i = comps.Length - 1; i > -1; --i)
            {
                if (comps[i] == null) continue;
                b.AppendLine(string.Format("Component: {0} [{1}/{2}]", comps[i].GetType().Name, comps[i].gameObject.scene.name, comps[i].gameObject.GetPath()));
                Destroy(comps[i]);
            }
            var objs = m_Objects;
            for (int i = objs.Length - 1; i > -1; --i)
            {
                if (objs[i] == null) continue;
                b.AppendLine(string.Format("GameObject: {0}/{1}", objs[i].scene.name, objs[i].GetPath()));
                Destroy(objs[i]);
            }
            Debug.Log(b.ToString());
            Destroy(this);
#endif
        }
    }
}