using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    [System.Serializable]
    public class Audios
    {
        public string name;
        public AudioClip clip;
        [Range(0.1f, 20)] public float volume;
        [Range(0.1f, 2)] public float pitch;
    }

    [SerializeField] Audios[] audios;
    
    public void Play(AudioSource source, string audioName)
    {
        if (!PlayerData.Instance.audioOn) return;
        var sound = Array.Find(audios, a => a.name.Equals(audioName));
        source.volume = sound.volume;
        source.pitch = sound.pitch;
        source.PlayOneShot(sound.clip);
    }

    public void Play(string audioName)
    {
        var sound = Array.Find(audios, a => a.name.Equals(audioName));
        audioSource.volume = sound.volume;
        audioSource.pitch = sound.pitch;
        audioSource.PlayOneShot(sound.clip);
    }

    public void StopAll()
    {
        audioSource.Stop();
    }
}
