namespace MyTools.SceneManagement
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using MyTools.Helpers;

    public static class SceneLoader
    {
#pragma warning disable 649
        static Dictionary<int, Action<Scene>> m_ScenesInLoadActionDict;
        static Dictionary<int, Action<Scene>> m_ScenesInUnloadActionDict;
#pragma warning restore 649

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            int count = SceneManager.sceneCountInBuildSettings;
            m_ScenesInLoadActionDict = new Dictionary<int, Action<Scene>>(count);
            m_ScenesInUnloadActionDict = new Dictionary<int, Action<Scene>>(count);
            SceneManager.sceneLoaded += SceneLoaded;
            SceneManager.sceneUnloaded += SceneUnloaded;
#if UNITY_EDITOR
            Debug.Log(string.Format("{0} is inited.", typeof(SceneLoader).Name));
#endif
        }

        static void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var dict = m_ScenesInLoadActionDict;
            var buildIndex = scene.buildIndex;
            if (!dict.TryGetValue(buildIndex, out var act)) return;
            act?.Invoke(scene);
            dict.Remove(buildIndex);
        }
        static void SceneUnloaded(Scene scene)
        {
            var dict = m_ScenesInUnloadActionDict;
            var buildIndex = scene.buildIndex;
            if (!dict.TryGetValue(buildIndex, out var act)) return;
            act?.Invoke(scene);
            dict.Remove(buildIndex);
        }


        public static bool AppendScene(int buildIndex, Action<float> onProgress = null, Action<Scene> onLoaded = null)
        {
#if UNITY_EDITOR
            Debug.Log("Append Scene!");
#endif
            if (!IsValidIndex(buildIndex)) throw new InvalidOperationException("Index of scene is wrong!");
            var dict = m_ScenesInLoadActionDict;
            if (dict.ContainsKey(buildIndex))
            {
#if UNITY_EDITOR
                Debug.LogWarning("Append Failed!");
#endif
                return false;
            }
            var scene = SceneManager.GetSceneByBuildIndex(buildIndex);
            if (scene.isLoaded)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Append Failed!");
#endif
                return false;
            }
            dict.Add(buildIndex, onLoaded);
            var operation = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
            if (operation == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Append Failed!");
#endif
                return false;
            }
            operation.allowSceneActivation = true;
            if (onProgress != null) CorouWaiter.RepeatInUpdate(
                () => { onProgress(operation.progress); }, 
                () => operation.isDone);
            return true;
        }

        public static bool LoadScene(int buildIndex, Action<float> onProgress = null, Action<Scene> onLoaded = null)
        {
#if UNITY_EDITOR
            Debug.Log("Load Scene!");
#endif
            if (!IsValidIndex(buildIndex)) throw new InvalidOperationException("Index of scene is wrong!");
            var dict = m_ScenesInLoadActionDict;
            if (dict.ContainsKey(buildIndex))
            {
#if UNITY_EDITOR
                Debug.LogWarning("Load Failed!");
#endif
                return false;
            }
            dict.Clear();
            dict.Add(buildIndex, onLoaded);
            var operation = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Single);
            if (operation == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Load Failed!");
#endif
                return false;
            }
            operation.allowSceneActivation = true;
            if (onProgress != null) CorouWaiter.RepeatInUpdate(
                () => { onProgress(operation.progress); },
                () => operation.isDone);
            return true;
        }

        public static bool RemoveScene(int buildIndex, Action<float> onProgress = null, Action<Scene> onUnloaded = null)
        {
#if UNITY_EDITOR
            Debug.Log("Remove Scene!");
#endif
            if (!IsValidIndex(buildIndex)) throw new InvalidOperationException("Index of scene is wrong!");
            var dict = m_ScenesInUnloadActionDict;
            if (dict.ContainsKey(buildIndex))
            {
#if UNITY_EDITOR
                Debug.LogWarning("Remove Failed!");
#endif
                return false;
            }
            var scene = SceneManager.GetSceneByBuildIndex(buildIndex);
            if (!scene.isLoaded)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Remove Failed!");
#endif
                return false;
            }
            dict.Add(buildIndex, onUnloaded);
            var operation = SceneManager.UnloadSceneAsync(buildIndex);
            if (operation == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Remove Failed!");
#endif
                return false;
            }
            operation.allowSceneActivation = true;
            if (onProgress != null) CorouWaiter.RepeatInUpdate(
                () => { onProgress(operation.progress); },
                () => operation.isDone);
            return true;
        }

        public static bool IsValidIndex(int buildIndex)
        {
            return buildIndex > -1 && buildIndex < SceneManager.sceneCountInBuildSettings;
        }
    }
}
