using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PauseManager
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        Application.focusChanged += FocusImpl;
    }

    public static event Action OnPause = delegate { };
    public static event Action OnResume = delegate { };

    static float _timeScaleTmp;

    static bool _pauseEnabled = false;
    public static bool PauseEnabled
    {
        get => _pauseEnabled;
        set
        {
            if (value == _pauseEnabled) return;
            _pauseEnabled = value;
            if (!value) Pause = false;
        }
    }

    static bool _pause;
    public static bool Pause
    {
        get => _pause;
        set
        {
            if (value == _pause) return;
            if (value && !_pauseEnabled) return;
            _pause = value;
            Debug.Log(string.Format("{0}: {1}", typeof(PauseManager), value ? "Pause" : "Resume"));
            if (value)
            {
                _timeScaleTmp = Time.timeScale;
                Time.timeScale = 0;
                OnPause();
            }
            else
            {
                Time.timeScale = _timeScaleTmp;
                OnResume();
            }
        }
    }

    static void FocusImpl(bool hasFocus)
    { if (!hasFocus) Pause = true; }
}
