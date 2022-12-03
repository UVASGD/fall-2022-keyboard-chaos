using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    public bool muted = false;
    private bool wait = false;
    public bool doDynamicMusic = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    void Start()
    {
        //Play("Theme");
    }
    private void Update()
    {
        if (doDynamicMusic)
        {
            if (Input.GetKeyDown(KeyCode.M) && !wait)
            {
                wait = true;
                if (!muted)
                {
                    muted = true;
                    // Stop("Theme");
                }
                else
                {
                    muted = false;
                    // Play("Theme");
                }
            }
            if (Input.GetKeyUp(KeyCode.M))
            {
                wait = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.M) && !wait)
            {
                wait = true;
                if (!muted)
                {
                    muted = true;
                    // Stop("Theme");
                }
                else
                {
                    muted = false;
                    // Play("Theme");
                }
            }
            if (Input.GetKeyUp(KeyCode.M))
            {
                wait = false;
            }
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("sound: " + name + "not found.");
            return;
        }
        s.source.Play();
    }

    
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("sound: " + name + "not found.");
            return;
        }
        s.source.Stop();
    }
}
