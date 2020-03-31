namespace MyTools.Extensions.Coroutines
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public static class CoroutinetEx
    {
        public static Coroutine StartCoroutine(this Func<IEnumerator> routineFunc, MonoBehaviour behaviour)
        {
            if (routineFunc == null) return null;
            if (behaviour == null) return null;
            return behaviour.StartCoroutine(routineFunc());
        }
        public static Coroutine StartCoroutine<T1>(this Func<T1, IEnumerator> routineFunc, MonoBehaviour behaviour, T1 param1)
        {
            if (routineFunc == null) return null;
            if (behaviour == null) return null;
            return behaviour.StartCoroutine(routineFunc(param1));
        }
        public static Coroutine StartCoroutine<T1, T2>(this Func<T1, T2, IEnumerator> routineFunc, MonoBehaviour behaviour, T1 param1, T2 param2)
        {
            if (routineFunc == null) return null;
            if (behaviour == null) return null;
            return behaviour.StartCoroutine(routineFunc(param1, param2));
        }
        public static Coroutine StartCoroutine<T1, T2, T3>(this Func<T1, T2, T3, IEnumerator> routineFunc, MonoBehaviour behaviour, T1 param1, T2 param2, T3 param3)
        {
            if (routineFunc == null) return null;
            if (behaviour == null) return null;
            return behaviour.StartCoroutine(routineFunc(param1, param2, param3));
        }
        public static Coroutine StartCoroutine<T1, T2, T3, T4>(this Func<T1, T2, T3, T4, IEnumerator> routineFunc, MonoBehaviour behaviour, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            if (routineFunc == null) return null;
            if (behaviour == null) return null;
            return behaviour.StartCoroutine(routineFunc(param1, param2, param3, param4));
        }
        public static Coroutine StartCoroutine(this IEnumerator routine, MonoBehaviour behaviour)
        {
            if (routine == null) return null;
            if (behaviour == null) return null;
            return behaviour.StartCoroutine(routine);
        }
    }
}
