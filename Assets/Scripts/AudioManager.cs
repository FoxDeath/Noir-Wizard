using System;
using UnityEngine;


//Audio Manager who controlls all the sounds and their behaviours
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {
        //Using the singleton pattern
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        //Adding a sourse to all sounds
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    /*Playing the sound whose name is the string "name" 
    and also giving the posibility to change the audio source with the optional paramarer "source" */
    public void Play(string name, AudioSource source = null)
    {
        if(PauseMenu.GameIsPaused)
        {
            return;
        }

        Sound s = Array.Find(sounds, sound => sound.name.Equals(name));

        if(s == null)
        {
            return;
        }

        if(source != null)
        {
            s.source = source;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        s.source.Play();
        s.isPlaying = true;
    }

    //Stopping the sound whose name is the string "name"
    public void Stop(string name)
    {
        if(!IsPlaying(name))
        {
            return;
        }
        else
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            
            if(s == null)
            {
                return;
            }

            s.source.Stop();
            s.isPlaying = false;
        }
    }

    //Stopping all sounds who are currently playing
    public void StopAll()
    {
        foreach(Sound s in sounds)
        {
            if(s.source != null && s.isPlaying)
            {
                Stop(s.name);
            }
        }
    }

    //Returning true if the sound whose name is the string "name" is playing or false if it's not playing
    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        
        if(s == null)
        {
            return false;
        }

        return s.isPlaying;
    }

    //Setting the pitch of the sound whose name is the string "name" withing the rance 0.1 and 3
    public void SetPitch(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        
        if(s == null || pitch < 0.1f || pitch > 3f)
        {
            return;
        }

        s.source.pitch = pitch; 
    }

    //Gets the pitch of the sound whose name is the string "name"
    public float GetPitch(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        
        if(s == null) 
        {
            return 0f;
        }

        return s.source.pitch;
    }

    //Setting the master volume of the whole game
    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
