using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;

public class MainAudioListener : MonoSingleton<MainAudioListener>
{
    static AudioListener audioListener = null;
    public static AudioListener AudioListener => audioListener;

    protected override void Awake()
    {
        base.Awake();
        audioListener = GetComponent<AudioListener>();
    }
}
