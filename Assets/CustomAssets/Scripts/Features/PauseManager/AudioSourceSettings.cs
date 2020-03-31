using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceSettings : MonoBehaviour
{
    [SerializeField] bool ignoreListenerPause = false;
    [SerializeField] bool ignoreListenerVolume = false;

    void Awake()
    {
        var audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.ignoreListenerPause = this.ignoreListenerPause;
            audio.ignoreListenerVolume = this.ignoreListenerVolume;
        }
    }
}
