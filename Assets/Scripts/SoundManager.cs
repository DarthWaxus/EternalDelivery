using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public List<AudioSource> audioSources;
    public bool mute = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public AudioSource GetFreeSource()
    {
        foreach (var item in audioSources)
        {
            if (!item.isPlaying)
                return item;
        }
        return null;
    }

    public void PlaySound(AudioClip clip, float volume)
    {
        if (mute)
            return;
        AudioSource audioSource = GetFreeSource();
        if (audioSource == null)
            return;
        audioSource.volume = volume;
        audioSource.PlayOneShot(clip, volume);
    }
    public void PlaySound(Sound sound)
    {
        PlaySound(sound.clip, sound.volume);
    }
}
[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public float volume = 1;
    public Sound(AudioClip clip, float volume = 1)
    {
        this.clip = clip;
        this.volume = volume;
    }
}