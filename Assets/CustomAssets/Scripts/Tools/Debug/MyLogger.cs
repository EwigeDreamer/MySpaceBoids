using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyLogger
{
    #region SIMLE_LOG
    public static void Log(string msg)
    {
#if UNITY_EDITOR
        Debug.Log(msg);
#endif
    }
    public static void Warning(string msg)
    {
#if UNITY_EDITOR
        Debug.LogWarning(msg);
#endif
    }
    public static void Error(string msg)
    {
#if UNITY_EDITOR
        Debug.LogError(msg);
#endif
    }

    public static void ObjectLog<T>(string msg)
    {
#if UNITY_EDITOR
        Debug.Log(string.Format("{0}: {1}", typeof(T), msg));
#endif
    }
    public static void ObjectWarning<T>(string msg)
    {
#if UNITY_EDITOR
        Debug.LogWarning(string.Format("{0}: {1}", typeof(T), msg));
#endif
    }
    public static void ObjectError<T>(string msg)
    {
#if UNITY_EDITOR
        Debug.LogError(string.Format("{0}: {1}", typeof(T), msg));
#endif
    }
    #endregion //SIMLE_LOG

    #region FORMAT_LOG
    public static void LogFormat(string format, params object[] args)
    {
#if UNITY_EDITOR
        Debug.Log(string.Format(format, args));
#endif
    }
    public static void WarningFormat(string format, params object[] args)
    {
#if UNITY_EDITOR
        Debug.LogWarning(string.Format(format, args));
#endif
    }
    public static void ErrorFormat(string format, params object[] args)
    {
#if UNITY_EDITOR
        Debug.LogError(string.Format(format, args));
#endif
    }

    public static void ObjectLogFormat<T>(string format, params object[] args)
    {
#if UNITY_EDITOR
        Debug.Log(string.Format("{0}: {1}", typeof(T), string.Format(format, args)));
#endif
    }
    public static void ObjectWarningFormat<T>(string format, params object[] args)
    {
#if UNITY_EDITOR
        Debug.LogWarning(string.Format("{0}: {1}", typeof(T), string.Format(format, args)));
#endif
    }
    public static void ObjectErrorFormat<T>(string format, params object[] args)
    {
#if UNITY_EDITOR
        Debug.LogError(string.Format("{0}: {1}", typeof(T), string.Format(format, args)));
#endif
    }
    #endregion //FORMAT_LOG

    #region CUSTOM_LOG
    public static void NotFoundWarning<T>()
    {
#if UNITY_EDITOR
        Debug.LogWarning(string.Format("{0} is not found!", typeof(T)));
#endif
    }
    public static void NotFoundError<T>()
    {
#if UNITY_EDITOR
        Debug.LogError(string.Format("{0} is not found!", typeof(T)));
#endif
    }
    public static void NotFoundObjectWarning<TObj, TFound>()
    {
#if UNITY_EDITOR
        Debug.LogWarning(string.Format("{0}: {1} is not found!", typeof(TObj), typeof(TFound)));
#endif
    }
    public static void NotFoundObjectError<TObj, TFound>()
    {
#if UNITY_EDITOR
        Debug.LogError(string.Format("{0}: {1} is not found!", typeof(TObj), typeof(TFound)));
#endif
    }
    #endregion //CUSTOM_LOG
}