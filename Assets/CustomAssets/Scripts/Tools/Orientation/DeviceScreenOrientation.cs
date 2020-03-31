using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Extensions.RichText;
using System;
using TMPro;

public class DeviceScreenOrientation : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        GameObject go = new GameObject(typeof(DeviceScreenOrientation).Name);
        DontDestroyOnLoad(go);
        go.AddComponent<DeviceScreenOrientation>();
    }

    public static event Action<ScreenOrientation> OnOrientationChange = delegate { };

    void Update()
    {
        CheckOrientation();
    }

    ScreenOrientation? m_OrientTmp = null;
    void CheckOrientation()
    {
        var orient = Screen.orientation;
        if (m_OrientTmp != null && m_OrientTmp.Value == orient) return;
        m_OrientTmp = orient;
        OnOrientationChange(orient);
    }
}
