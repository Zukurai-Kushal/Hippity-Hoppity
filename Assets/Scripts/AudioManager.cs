using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour

{

    public Sound[] sounds;    

    public static AudioManager instance;

    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else 
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
           s.source.pitch  = s.pitch;
           s.source.loop   = s.loop;
        }
        
    }

    // void Start () 
    // {
    //     Play("Theme");
    // }
    
    public void Play (string name)
    {
        Sound s =  Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + "cannot be found/incorrect name!");
            return;
        }

        s.source.Play();
        // if(PauseMenuScript.gameIsPaused == false)
        // {
        //     s.source.Play();
        // }
    }

    // public void PauseSound()
    // {
    //     foreach (Sound s in sounds)
    //     {
    //         s.source.Pause();
    //     }
    // }

    public void ResumeSound()
    {
        foreach (Sound s in sounds)
        {
            s.source.UnPause();
        }
    }
}
