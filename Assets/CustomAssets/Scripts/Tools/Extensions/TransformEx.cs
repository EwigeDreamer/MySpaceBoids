namespace MyTools.Extensions.Transforms
{
    using System.Collections.Generic;
    using UnityEngine;
    using System.Text;
    using System;
    public static class TransformEx
    {
        public static Transform SetActive(this Transform tr, bool state)
        {
            tr.gameObject.SetActive(state);
            return tr;
        }
        public static TColl SetActive<TColl>(this TColl collection, bool state)
            where TColl : IList<Transform>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].gameObject.SetActive(state);
            return collection;
        }
        public static TColl SetActiveOne<TColl>(this TColl collection, int index, bool state)
            where TColl : IList<Transform>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].gameObject.SetActive(!state);
            if (index > -1 && index < count) collection[index].gameObject.SetActive(state);
            return collection;
        }
        public static TColl SetParent<TColl>(this TColl collection, GameObject parent)
            where TColl : IList<Transform>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].SetParent(parent.transform);
            return collection;
        }
        public static TColl SetParent<TColl>(this TColl collection, Transform parent)
            where TColl : IList<Transform>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].SetParent(parent);
            return collection;
        }
        public static TColl ClearParent<TColl>(this TColl collection)
            where TColl : IList<Transform>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].SetParent(null);
            return collection;
        }

        public static string GetPath(this Transform tr)
        {
            return BuildObjPath(tr, AppendName).ToString();
        }
        public static string GetPathWithSiblingIndex(this Transform tr)
        {
            return BuildObjPath(tr, AppendNameWithSiblingIndex).ToString();
        }

        #region GAMEOBJECT_PATH
        static StringBuilder BuildObjPath(Transform tr, System.Action<Transform, StringBuilder> appendName)
        {
            var sb = new StringBuilder();
            BuildObjPathRecur(tr, appendName, sb);
            return sb;
        }
        static void BuildObjPathRecur(Transform tr, System.Action<Transform, StringBuilder> appendName, StringBuilder sb)
        {
            var parent = tr.parent;
            if (parent != null) BuildObjPathRecur(parent, appendName, sb);
            appendName(tr, sb);
            sb.Append('/');
        }
        static void AppendName(Transform tr, StringBuilder sb)
        {
            sb.Append(tr.name);
        }
        static void AppendNameWithSiblingIndex(Transform tr, StringBuilder sb)
        {
            sb.Append(tr.name);
            var parent = tr.parent;
            if (parent == null) return;
            foreach (Transform ch in parent)
                if (ch != tr && ch.name == tr.name)
                {
                    sb.Append(' ');
                    sb.Append('[');
                    sb.Append(tr.GetSiblingIndex());
                    sb.Append(']');
                    return;
                }
        }
        #endregion //GAMEOBJECT_PATH

        public static void MoveUnderParent(this Transform tr, Transform parent)
        {
            tr.parent = parent;
            tr.localPosition = Vector3.zero;
            tr.localRotation = Quaternion.identity;
        }
    }
}
