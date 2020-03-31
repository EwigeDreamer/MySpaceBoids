using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyTools.Extensions.String;
using System;
using System.Text;
using System.Collections.ObjectModel;

namespace MyTools.SceneManagement
{
    public static class SceneData
    {
        static Dictionary<string, int> m_NameIndexDict;
        static Dictionary<int, string> m_IndexNameDict;
        static Dictionary<SceneKind, int> m_KindIndexDict;
        static Dictionary<int, SceneKind> m_IndexKindDict;

        public static ReadOnlyCollection<SceneKind> SceneKinds { get; }
        = new ReadOnlyCollection<SceneKind>((SceneKind[])System.Enum.GetValues(typeof(SceneKind)));

        public const SceneKind immortalSceneKind = SceneKind.ZeroScene;

        public static bool TryGetIndex(string name, out int index)
        { return m_NameIndexDict.TryGetValue(name, out index); }
        public static bool TryGetName(int index, out string name)
        { return m_IndexNameDict.TryGetValue(index, out name); }
        public static bool TryGetIndex(SceneKind kind, out int index)
        { return m_KindIndexDict.TryGetValue(kind, out index); }
        public static bool TryGetKind(int index, out SceneKind kind)
        { return m_IndexKindDict.TryGetValue(index, out kind); }

        static SceneData()
        {
            int count = SceneManager.sceneCountInBuildSettings;
            var nameIndexDict = m_NameIndexDict = new Dictionary<string, int>(count);
            var indexNameDict = m_IndexNameDict = new Dictionary<int, string>(count);
#if UNITY_EDITOR
            var log = new StringBuilder();
            log.AppendLine("Scenes in build settings:");
#endif
            for (int i = 0; i < count; ++i)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                if (string.IsNullOrWhiteSpace(path)) continue;
                string name = path.SubstringFromTo('/', '.', true);
                if (string.IsNullOrWhiteSpace(name)) continue;
#if UNITY_EDITOR
                log.AppendLine(string.Format("[{0}] {1}", i, name));
                if (nameIndexDict.ContainsKey(name))
                    Debug.LogError("Scene name is repeated: " + name);
#endif
                nameIndexDict[name] = i;
                indexNameDict[i] = name;
            }
#if UNITY_EDITOR
            Debug.Log(log.ToString());
#endif

            var kinds = (SceneKind[])Enum.GetValues(typeof(SceneKind));
            count = kinds.Length;
            var kindIndexDict = m_KindIndexDict = new Dictionary<SceneKind, int>(count);
            var indexKindDict = m_IndexKindDict = new Dictionary<int, SceneKind>(count);
            for (int i = 0; i < count; ++i)
            {
                var kind = kinds[i];
                string name = kind.ToString();
                if (nameIndexDict.TryGetValue(name, out var index))
                {
                    kindIndexDict[kind] = index;
                    indexKindDict[index] = kind;
                }
#if UNITY_EDITOR
                else Debug.LogError("Scene name not found for Scene kind: " + name);
#endif
            }
        }
    }

    public static class SceneDataExtentions
    {
        public static int GetBuildIndex(this SceneKind kind)
        {
            if (SceneData.TryGetIndex(kind, out var buildIndex)) return buildIndex;
            throw new InvalidOperationException(string.Format($"SceneData doesn't contain {nameof(buildIndex)} for {nameof(kind)}: {kind}"));
        }

        public static SceneKind GetSceneKind(this int buildIndex)
        {
            if (SceneData.TryGetKind(buildIndex, out var kind)) return kind;
            throw new InvalidOperationException(string.Format($"SceneData doesn't contain {nameof(kind)} for {nameof(buildIndex)}: {buildIndex}"));
        }

        public static SceneWrapper GetSceneWrapper (this GameObject obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return SceneWrapper.GetWrapper(obj);
        }
    }
}

