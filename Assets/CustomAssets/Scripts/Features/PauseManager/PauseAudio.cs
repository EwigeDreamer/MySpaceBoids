using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;
using UnityEngine.Audio;

public static class PauseAudio
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        PauseManager.OnPause += Pause;
        PauseManager.OnResume += Resume;
    }

    static void Pause()
    { AudioListener.pause = true; }
    static void Resume()
    { AudioListener.pause = false; }
}
