using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using MyTools.Helpers;

namespace MyTools.SceneManagement
{
    public class SceneWrapper
    {
        private static List<SceneWrapper> wrappers = null;
        private static Dictionary<SceneKind, SceneWrapper> kindWrapperDict = null;
        private static Dictionary<int, SceneWrapper> indexWrapperDict = null;

        static SceneWrapper()
        {
            var kinds = SceneData.SceneKinds;
            var count = kinds.Count;
            wrappers = new List<SceneWrapper>(count);
            kindWrapperDict = new Dictionary<SceneKind, SceneWrapper>(count);
            indexWrapperDict = new Dictionary<int, SceneWrapper>(count);
            foreach (var kind in kinds)
            {
                var wrapper = new SceneWrapper(kind);
                wrappers.Add(wrapper);
                kindWrapperDict.Add(kind, wrapper);
                indexWrapperDict.Add(wrapper.BuildIndex, wrapper);
            }
            SceneManager.sceneLoaded += SceneLoaded;
            SceneManager.sceneUnloaded += SceneUnloaded;
            SceneManager.activeSceneChanged += ActiveSceneChanged;
        }

        public static SceneWrapper GetWrapper(SceneKind kind) => kindWrapperDict[kind];
        public static SceneWrapper GetWrapper(int buildIndex) => indexWrapperDict[buildIndex];
        public static SceneWrapper GetWrapper(GameObject obj) => indexWrapperDict[obj.scene.buildIndex];
        public static void GetLoaded(List<SceneWrapper> list)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            list.Clear();
            foreach (var wrapper in wrappers)
                if (wrapper.IsLoaded) list.Add(wrapper);
        }
        public static void GetLoaded(List<SceneKind> list)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            list.Clear();
            foreach (var wrapper in wrappers)
                if (wrapper.IsLoaded) list.Add(wrapper.Kind);
        }

        private static void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex < 0) return;
            var wrapper = GetWrapper(scene.buildIndex);
            wrapper.scene = scene;
            wrapper.OnLoad(wrapper);
        }
        private static void SceneUnloaded(Scene scene)
        {
            if (scene.buildIndex < 0) return;
            var wrapper = GetWrapper(scene.buildIndex);
            wrapper.scene = scene;
            wrapper.OnUnload(wrapper);
        }
        private static void ActiveSceneChanged(Scene current, Scene next)
        {
            if (current.buildIndex < 0) return;
            var currentWrapper = GetWrapper(current.buildIndex);
            var nextWrapper = GetWrapper(next.buildIndex);
            currentWrapper.scene = current;
            nextWrapper.scene = next;
            currentWrapper.OnChangeActive(currentWrapper);
            nextWrapper.OnChangeActive(nextWrapper);
        }

        public event Action OnStartLoad = delegate { };
        public event Action OnFinishLoad = delegate { };
        public event Action OnStartUnload = delegate { };
        public event Action OnFinishUnload = delegate { };

        private Scene scene;
        private bool isLoading = false;
        private bool isUnloading = false;
        public SceneKind Kind { get; }
        public int BuildIndex { get; }
        public bool IsLoaded => this.scene.IsValid() && this.scene.isLoaded;
        public bool IsActive => SceneManager.GetActiveScene().buildIndex == BuildIndex;
        public Scene Scene => this.scene;

        public event Action<SceneWrapper> OnLoad = delegate { };
        public event Action<SceneWrapper> OnUnload = delegate { };
        public event Action<SceneWrapper> OnChangeActive = delegate { };

        private SceneWrapper(SceneKind kind)
        {
            this.Kind = kind;
            this.BuildIndex = kind.GetBuildIndex();
            this.scene = SceneManager.GetSceneByBuildIndex(this.BuildIndex);
        }

        public Coroutine Load(bool allowActivation = true, LoadSceneMode mode = LoadSceneMode.Additive, Action<float> onProgress = null)
        {
            if (this.isLoading) return null;
            if (IsLoaded) return null;
            return CorouWaiter.Start(GetRoutine());
            IEnumerator GetRoutine()
            {
                try { OnStartLoad(); } catch(Exception e) { Debug.LogError(e); }
                this.isLoading = true;
                var operation = SceneManager.LoadSceneAsync(this.BuildIndex, mode);
                operation.allowSceneActivation = allowActivation;
                bool done = false;
                OnLoad += Loaded;
                void Loaded(SceneWrapper wrapper)
                {
                    OnLoad -= Loaded;
                    done = true;
                }
                while (!done)
                {
                    onProgress?.Invoke(operation.progress / 0.9f);
                    yield return null;
                }
                this.isLoading = false;
                try { OnFinishLoad(); } catch (Exception e) { Debug.LogError(e); }
            }
        }

        public Coroutine Unload(Action<float> onProgress = null)
        {
            if (this.isUnloading) return null;
            if (!IsLoaded) return null;
            return CorouWaiter.Start(GetRoutine());
            IEnumerator GetRoutine()
            {
                try { OnStartUnload(); } catch (Exception e) { Debug.LogError(e); }
                this.isUnloading = true;
                var operation = SceneManager.UnloadSceneAsync(this.BuildIndex);
                bool done = false;
                OnUnload += Unloaded;
                void Unloaded(SceneWrapper wrapper)
                {
                    OnUnload -= Unloaded;
                    done = true;
                }
                while (!done)
                {
                    onProgress?.Invoke(operation.progress / 0.9f);
                    yield return null;
                }
                this.isUnloading = false;
                try { OnFinishUnload(); } catch (Exception e) { Debug.LogError(e); }
            }
        }

        public void Active()
        {
            if (!IsLoaded) return;
            SceneManager.SetActiveScene(this.scene);
        }
    }
}
