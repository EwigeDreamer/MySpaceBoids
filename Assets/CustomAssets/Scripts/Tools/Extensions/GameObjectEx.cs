namespace MyTools.Extensions.GameObjects
{
    using System.Collections.Generic;
    using UnityEngine;
    using System.Text;
    using MyTools.Extensions.Transforms;
    using System;
    public static class GameObjectEx
    {
        public static TColl SetActive<TColl>(this TColl collection, bool state)
            where TColl : IList<GameObject>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].SetActive(state);
            return collection;
        }
        public static TColl SetActiveOne<TColl>(this TColl collection, int index, bool state)
            where TColl : IList<GameObject>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].SetActive(!state);
            if (index > -1 && index < count) collection[index].SetActive(state);
            return collection;
        }
        public static TColl SetActiveOne<TColl>(this TColl collection, GameObject one, bool state)
            where TColl : IList<GameObject>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].SetActive(collection[i] == one ? state : !state);
            return collection;
        }
        public static GameObject SetParent(this GameObject gameObject, GameObject parent)
        {
            if (gameObject == null) throw new ArgumentNullException(nameof(gameObject));
            gameObject.transform.SetParent(parent.transform);
            return gameObject;
        }
        public static GameObject SetParent(this GameObject gameObject, Transform parent)
        {
            if (gameObject == null) throw new ArgumentNullException(nameof(gameObject));
            gameObject.transform.SetParent(parent);
            return gameObject;
        }
        public static GameObject ClearParent(this GameObject gameObject)
        {
            if (gameObject == null) throw new ArgumentNullException(nameof(gameObject));
            gameObject.transform.SetParent(null);
            return gameObject;
        }
        public static TColl SetParent<TColl>(this TColl collection, GameObject parent)
            where TColl : IList<GameObject>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].transform.SetParent(parent.transform);
            return collection;
        }
        public static TColl SetParent<TColl>(this TColl collection, Transform parent)
            where TColl : IList<GameObject>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].transform.SetParent(parent);
            return collection;
        }
        public static TColl ClearParent<TColl>(this TColl collection)
            where TColl : IList<GameObject>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].transform.SetParent(null);
            return collection;
        }

        public static string GetPath(this GameObject gameObject)
        {
            if (gameObject == null) throw new ArgumentNullException(nameof(gameObject));
            return gameObject.transform.GetPath();
        }
        public static string GetPathWithSiblingIndex(this GameObject gameObject)
        {
            if (gameObject == null) throw new ArgumentNullException(nameof(gameObject));
            return gameObject.transform.GetPathWithSiblingIndex();
        }
        public static bool ValidateGetComponent<T>(this GameObject gameObject, ref T field, bool forced = false) 
            where T : class
        {
            if (gameObject == null) throw new ArgumentNullException(nameof(gameObject));
            if (field == null || forced) field = gameObject.GetComponent<T>();
            return field != null; 
        }

        public static bool ValidateGetComponentInParent<T>(this GameObject gameObject, ref T field, bool forced = false) where T : class
        {
            if (gameObject == null) throw new ArgumentNullException(nameof(gameObject));
            if (field == null || forced) 
                field = gameObject.GetComponentInParent<T>(); 
            return field != null; 
        }
        public static bool ValidateGetComponentInChildren<T>(this GameObject gameObject, ref T field, bool forced = false) where T : class
        {
            if (gameObject == null) throw new ArgumentNullException(nameof(gameObject));
            if (field == null || forced) 
                field = gameObject.GetComponentInChildren<T>(); 
            return field != null;
        }
    }
}
