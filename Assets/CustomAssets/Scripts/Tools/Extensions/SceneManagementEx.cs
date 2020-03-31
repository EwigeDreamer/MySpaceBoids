namespace MyTools.Extensions.SceneManagement
{
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using UnityEngine.SceneManagement;
    public static class SceneEx
    {
        public static T FindObjectOfType<T>(this Scene scene, bool includeInactive = false) where T : class
        {
            if (!scene.isLoaded) return null;
            int count = scene.rootCount;
            List<GameObject> objs = new List<GameObject>(count);
            scene.GetRootGameObjects(objs);
            T com;
            for (int i = 0; i < count; ++i)
            {
                com = objs[i].GetComponentInChildren<T>(includeInactive);
                if (com != null) return com;
            }
            return null;
        }
        public static T[] FindObjectsOfType<T>(this Scene scene) where T : class
        {
            if (!scene.isLoaded) return new T[0];
            int count = scene.rootCount;
            List<GameObject> objs = new List<GameObject>(count);
            scene.GetRootGameObjects(objs);
            List<T> total = new List<T>();
            List<T> tmp = new List<T>();
            for (int i = 0; i < count; ++i)
            {
                tmp.Clear();
                objs[i].GetComponentsInChildren(tmp);
                total.AddRange(tmp);
            }
            return total.ToArray();
        }
        public static void FindObjectsOfType<T>(this Scene scene, List<T> results) where T : class
        {
            results.Clear();
            if (!scene.isLoaded) return;
            int count = scene.rootCount;
            List<GameObject> objs = new List<GameObject>(count);
            scene.GetRootGameObjects(objs);
            List<T> tmp = new List<T>();
            for (int i = 0; i < count; ++i)
            {
                tmp.Clear();
                objs[i].GetComponentsInChildren(tmp);
                results.AddRange(tmp);
            }
        }
    }
}
