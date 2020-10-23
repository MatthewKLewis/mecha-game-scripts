using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    #region Singleton!

    public static AudioManager instance;

    private void Awake()
    {
        instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    #endregion

    public Sound[] sounds; //the Awake method in Singleton searches this array and adds adds AudioSource components for each sound.

    private void Start()
    {
        Play("Theme");
        Play("GameStart");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

}
