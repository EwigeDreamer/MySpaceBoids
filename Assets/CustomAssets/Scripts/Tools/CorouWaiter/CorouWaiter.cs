using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MyTools.Helpers
{
    public class CorouWaiter : MonoBehaviour
    {
        static CorouWaiter instance;

        static WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        static CorouWaiter()
        {
            GameObject go = new GameObject(typeof(CorouWaiter).Name);
            instance = go.AddComponent<CorouWaiter>();
            DontDestroyOnLoad(go);
        }

#if UNITY_EDITOR
        [SerializeField][HideInInspector] int count = 0;
#endif

        public static Coroutine WaitFor(Coroutine coroutine, Action action)
        {
            return instance.StartCoroutine(GetRoutine());
            IEnumerator GetRoutine()
            {
#if UNITY_EDITOR
                ++instance.count;
#endif
                yield return coroutine;
                action?.Invoke();
#if UNITY_EDITOR
                --instance.count;
#endif
            }
        }
        public static Coroutine WaitFor(Func<bool> condition, Func<bool> cancel = null)
        {
            return instance.StartCoroutine(GetRoutine());
            IEnumerator GetRoutine()
            {
#if UNITY_EDITOR
                ++instance.count;
#endif
                if (condition == null) yield break;
                if (cancel != null)
                {
                    while (!condition())
                    {
                        if (cancel()) yield break;
                        yield return null;
                    }
                }
                else
                {
                    while (!condition())
                    {
                        yield return null;
                    }
                }
#if UNITY_EDITOR
                --instance.count;
#endif
            }
        }
        public static Coroutine WaitFor(Func<bool> condition, Action action, Func<bool> cancel = null)
        {
            return instance.StartCoroutine(GetRoutine());
            IEnumerator GetRoutine()
            {
#if UNITY_EDITOR
                ++instance.count;
#endif
                if (condition == null)
                {
                    if (cancel == null || !cancel()) action?.Invoke();
                    yield break;
                }
                if (cancel != null)
                {
                    while (!condition())
                    {
                        if (cancel()) yield break;
                        yield return null;
                    }
                }
                else
                {
                    while (!condition())
                    {
                        yield return null;
                    }
                }
                action?.Invoke();
#if UNITY_EDITOR
                --instance.count;
#endif
            }
        }
        public static Coroutine WaitFor(float delay, bool realTime = false)
        {
            return instance.StartCoroutine(GetRoutine());
            IEnumerator GetRoutine()
            {
#if UNITY_EDITOR
                ++instance.count;
#endif
                if (delay > 0)
                {
                    if (realTime)
                        yield return new WaitForSecondsRealtime(delay);
                    else 
                        yield return new WaitForSeconds(delay);
                }
#if UNITY_EDITOR
                --instance.count;
#endif
            }
        }
        public static Coroutine WaitFor(float delay, Action action, bool realTime = false)
        {
            return instance.StartCoroutine(GetRoutine());
            IEnumerator GetRoutine()
            {
#if UNITY_EDITOR
                ++instance.count;
#endif
                if (delay > 0)
                {
                    if (realTime)
                        yield return new WaitForSecondsRealtime(delay);
                    else
                        yield return new WaitForSeconds(delay);
                }
                action?.Invoke();
#if UNITY_EDITOR
                --instance.count;
#endif
            }
        }
        public static Coroutine WaitForUpdate(Action action)
        {
            return instance.StartCoroutine(GetRoutine());
            IEnumerator GetRoutine()
            {
                yield return null;
                action?.Invoke();
            }
        }
        public static Coroutine WaitForFixedUpdate(Action action)
        {
            return instance.StartCoroutine(GetRoutine());
            IEnumerator GetRoutine()
            {
                yield return waitForFixedUpdate;
                action?.Invoke();
            }
        }
        public static Coroutine WaitForEndOfFrame(Action action)
        {
            return instance.StartCoroutine(GetRoutine());
            IEnumerator GetRoutine()
            {
                yield return new WaitForEndOfFrame();
                action?.Invoke();
            }
        }
        public static Coroutine Repeat(Action action, float interval, int count, bool realTime = false, Func<bool> cancel = null)
        {
            if (action == null) return null;
            if (count < 1) return null;
            interval = Mathf.Max(interval, 0f);
            return instance.StartCoroutine(GetRoutine());
            IEnumerator GetRoutine()
            {
#if UNITY_EDITOR
                ++instance.count;
#endif
                float timer = 0f;
                if (cancel != null)
                {
                    while (count > 0f)
                    {
                        if (cancel()) yield break;
                        timer -= DeltaTime(realTime);
                        if (timer <= 0f)
                        {
                            action();
                            timer += interval;
                            --count;
                        }
                        yield return null;
                    }
                }
                else
                {
                    while (count > 0f)
                    {
                        timer -= DeltaTime(realTime);
                        if (timer <= 0f)
                        {
                            action();
                            timer += interval;
                            --count;
                        }
                        yield return null;
                    }
                }
#if UNITY_EDITOR
                --instance.count;
#endif
            }
        }
        public static Coroutine Repeat(Action action, float interval, float duration, bool realTimeInterval = false, bool realTimeDuration = false, Func<bool> cancel = null)
        {
            if (action == null) return null;
            if (interval < 0f) interval = 0f;
            return instance.StartCoroutine(GetRoutine());
            IEnumerator GetRoutine()
            {
#if UNITY_EDITOR
                ++instance.count;
#endif
                float timer = 0f;
                if (cancel != null)
                {
                    while (duration > 0f)
                    {
                        if (cancel()) yield break;
                        timer -= DeltaTime(realTimeInterval);
                        if (timer <= 0f)
                        {
                            action();
                            timer += interval;
                        }
                        duration -= DeltaTime(realTimeDuration);
                        yield return null;
                    }
                }
                else
                {
                    while (duration > 0f)
                    {
                        timer -= DeltaTime(realTimeInterval);
                        if (timer <= 0f)
                        {
                            action();
                            timer += interval;
                        }
                        duration -= DeltaTime(realTimeDuration);
                        yield return null;
                    }
                }
#if UNITY_EDITOR
                --instance.count;
#endif
            }
        }
        public static Coroutine RepeatInUpdate(Action action, Func<bool> cancel)
        {
            if (action == null) return null;
            if (cancel == null) return null;
            return instance.StartCoroutine(GetRoutine());
            IEnumerator GetRoutine()
            {
#if UNITY_EDITOR
                ++instance.count;
#endif
                while (!cancel())
                {
                    action();
                    yield return null;
                }
#if UNITY_EDITOR
                --instance.count;
#endif
            }
        }

        public static void Stop(Coroutine coroutine)
        {
            if (coroutine == null) return;
            instance.StopCoroutine(coroutine);
        }

        public static Coroutine Start(IEnumerator routine, Action action = null)
        {
            if (routine == null) return null;
            return WaitFor(instance.StartCoroutine(routine), action);
        }

        static float DeltaTime(bool unscaled) => unscaled ? TimeManager.UnscaledDeltaTime : TimeManager.DeltaTime;

#if UNITY_EDITOR
        [CustomEditor(typeof(CorouWaiter))]
        private class CorouWaiterEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
                var count = serializedObject.FindProperty("count").intValue;
                EditorGUILayout.LabelField($"Active coroutines: {count}");
            }
        }
#endif
    }
}