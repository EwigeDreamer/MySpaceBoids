using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using MyTools.Prefs;
using AudioManagerCostyls;
using System.Collections.ObjectModel;

public enum AudioChannel
{
    Master,
    Music,
    Effects,
    Interface,
}
public static class AudioManager
{
    const string prefsKey = "audio_settings";
    const string mixerPath = "AudioMixer";

    const string volumeSuffix = "Volume";

    const string adioSourceObjectName = "AudioSources";

    public static ReadOnlyCollection<AudioChannel> Channels { get; }
        = new ReadOnlyCollection<AudioChannel>((AudioChannel[])System.Enum.GetValues(typeof(AudioChannel)));

    #region DATA
    [System.Serializable]
    private class AudioSettings
    {
        [SerializeField]
        public ChannelSettings[] settings;
        public AudioSettings()
        {
            var channels = Channels;
            int count = Channels.Count;
            var settings = this.settings = new ChannelSettings[count];
            for (int i = 0; i < count; ++i) settings[i] = new ChannelSettings();
        }
        public ChannelSettings this[AudioChannel ch] => settings[(int)ch];
    }
    [System.Serializable]
    public class ChannelSettings
    {
        public float volume = 1f;
    }
    static AudioSettings settings;
    #endregion//DATA

    static AudioMixer mixer;

    static Dictionary<AudioChannel, string> volumeKeyDict;
    static Dictionary<AudioChannel, AudioSource> channelDict; 

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        InitData();
        InitMixerKeys();
        InitMixer();
        InitAudioSources();
    }

    public static float GetVolume(AudioChannel channel) => settings[channel].volume;
    public static void SetVolume(AudioChannel channel, float value)
    {
        settings[channel].volume = value;
        mixer.SetFloat(volumeKeyDict[channel], RatioToFader(value));
    }

    static bool pause = false;
    public static bool Pause
    {
        get { return pause; }
        set
        {
            if (value == pause) return;
            pause = value;
            AudioListener.pause = pause;
        }
    }

    public static void Play(AudioClip clip, AudioChannel channel = AudioChannel.Master)
    { channelDict[channel].PlayOneShot(clip); }

    static float RatioToFader(float ratio) { return ratio > 1e-5f ? 20f * Mathf.Log(ratio) : -80f; }

    static void InitMixer()
    {
        mixer = (AudioMixer)Resources.Load(mixerPath);
        var obj = new GameObject(typeof(AudioManagerComponent).Name);
        Object.DontDestroyOnLoad(obj);
        var component = obj.AddComponent<AudioManagerComponent>();
        component.OnStart += SetUpMixer;
        void SetUpMixer()
        {
            var channels = Channels;
            foreach (var channel in channels)
                mixer.SetFloat(volumeKeyDict[channel], RatioToFader(settings[channel].volume));
            component.OnStart -= SetUpMixer;
            Object.Destroy(obj);
        }
    }

    static void InitMixerKeys()
    {
        var channels = Channels;
        var dict = volumeKeyDict = new Dictionary<AudioChannel, string>(channels.Count);
        foreach (var channel in channels)
            dict.Add(channel, channel.ToString() + volumeSuffix);
    }

    static void InitAudioSources()
    {
        var channels = (AudioChannel[])System.Enum.GetValues(typeof(AudioChannel));
        var dict = channelDict = new Dictionary<AudioChannel, AudioSource>(channels.Length);
        var obj = new GameObject(adioSourceObjectName);
        Object.DontDestroyOnLoad(obj);
        foreach (var channel in channels)
            dict.Add(channel, AddAudioSource(obj, channel.ToString()));
    }
    static AudioSource AddAudioSource(GameObject obj, string mixerGroupSubPath)
    {
        var audio = obj.AddComponent<AudioSource>();
        audio.outputAudioMixerGroup = mixer.FindMatchingGroups(mixerGroupSubPath)[0];
        audio.mute = false;
        audio.bypassEffects = false;
        audio.bypassReverbZones = false;
        audio.playOnAwake = false;
        audio.loop = false;
        audio.priority = 128;
        audio.volume = 1f;
        audio.pitch = 1f;
        audio.panStereo = 0f;
        audio.spatialBlend = 0f;
        audio.reverbZoneMix = 1f;
        return audio;
    }

    static void InitData()
    {
        settings = MyPlayerPrefs.GetObject(prefsKey, new AudioSettings());
        MyPlayerPrefs.OnSave += () => MyPlayerPrefs.SetObject(prefsKey, settings);
    }
}
