using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;

    public static AudioManager s_instance;
    void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

    }

    public void Play(string name)
    {
        Sound sName = Array.Find(sounds, sound => sound.name.Equals(name));
        if (sName == null)
        {
            Debug.LogError("the name of SFX does not in AudioManger");
            return;
        }
        sName.source.Play();
    }

    public void Stop(string name)
    {
        Sound sName = Array.Find(sounds, sound => sound.name.Equals(name));
        if (sName == null)
        {
            Debug.LogError("the name of SFX does not in AudioManger");
            return;
        }
        sName.source.Stop();

    }
}
