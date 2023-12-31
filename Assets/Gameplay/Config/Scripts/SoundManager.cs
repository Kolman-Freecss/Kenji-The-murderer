#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

public class SoundManager : MonoBehaviour
{
    [Serializable]
    public enum BackgroundMusic
    {
        Intro,
        InGameInit,
        InGameSecond,
        WinGame,
        LostGame,
        StartEncounter
    }

    #region Inspector Variables

    [Range(0, 100)] public float EffectsAudioVolume = 100f;

    [Range(0, 100)] public float MusicAudioVolume = 100f;

    [Range(0, 100)] public float MasterAudioVolume = 100f;

    public List<SerializableDictionaryEntry<BackgroundMusic, AudioClip>> BackgroundMusicClips;
    public AudioClip ButtonClickSound;

    [SerializeField] private AudioSource backgroundAudioSources;

    [SerializeField] private AudioSource uiAudioSource;

    public bool debug;

    #endregion

    #region Member Variables

    public static SoundManager Instance { get; private set; }

    private BackgroundMusic currentBackgroundClip;
    private BackgroundMusic previousBackgroundClip;

    #endregion

    #region InitData

    private void Awake()
    {
        ManageSingleton();
    }

    void ManageSingleton()
    {
        if (Instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        if (BackgroundMusicClips == null)
            BackgroundMusicClips = new List<SerializableDictionaryEntry<BackgroundMusic, AudioClip>>();
        SetEffectsVolume(EffectsAudioVolume);
        SetMusicVolume(MusicAudioVolume);
        if (debug)
            StartBackgroundMusic(BackgroundMusic.InGameInit);
        else
            StartBackgroundMusic(BackgroundMusic.Intro);
    }

    #endregion

    #region Logic

    public void StartTemporalBackground(BackgroundMusic backgroundMusic)
    {
        previousBackgroundClip = currentBackgroundClip;
        StartBackgroundMusic(backgroundMusic);
    }

    public void StartPreviousBackground()
    {
        StartBackgroundMusic(previousBackgroundClip);
    }

    public void StartBackgroundMusic(BackgroundMusic backgroundMusic)
    {
        AudioClip clip = BackgroundMusicClips.Find(x => x.Key == backgroundMusic).Value;
        if (clip != null)
        {
            if (backgroundAudioSources.isPlaying)
                backgroundAudioSources.Stop();
            backgroundAudioSources.clip = clip;
            backgroundAudioSources.Play();
            currentBackgroundClip = backgroundMusic;
        }
        else
        {
            Debug.LogWarning($"No clip found for {backgroundMusic}");
        }
    }

    public void SetEffectsVolume(float volume)
    {
        EffectsAudioVolume = volume;
        uiAudioSource.volume = volume / 100;
    }

    public void SetMusicVolume(float volume)
    {
        MusicAudioVolume = volume;
        backgroundAudioSources.volume = volume / 100;
    }

    public void SetMasterVolume(float volume)
    {
        MasterAudioVolume = volume;
        AudioListener.volume = volume / 100;
    }

    public void PlayButtonClickSound()
    {
        uiAudioSource.PlayOneShot(ButtonClickSound);
    }

    public void PlayAudioSourceEffect(AudioSource audioSource)
    {
        audioSource.volume = EffectsAudioVolume / 100;
        audioSource.Play();
    }

    public void PlayWorldEffectAtPosition(Vector3 position, AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, position, EffectsAudioVolume / 100);
    }

    #endregion
}