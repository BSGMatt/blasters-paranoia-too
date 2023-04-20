using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;

    public float spatialBlend = 1f;

    void Awake() {

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = spatialBlend;
        }
    }

    public bool Play (string name) {

        Debug.Log("Attempting to play: " + name);

        Sound s = Array.Find(sounds, sound => sound.name == name); 
        if (s != null) {

            Debug.Log("Song Found: " + name);
            s.source.Play();
            return true;
        }
        else {
            return false;
        }
        
    }

    public void ButtonPlay(string name) {
        Play(name);
    }

    public void ButtonStop(string name) {
        StopPlaying(name);
    }


    /// <summary>
    /// Checks if a sound is playing. 
    /// </summary>
    /// <param name="name">The name of the sound. </param>
    /// <throws>A NullReferenceException if no sound with that given name. </throws>
    /// <returns>True if the sound is playing, false otherwise. </returns>
    public bool IsPlaying (string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null) {
            return s.source.isPlaying;
        }
        else {
            throw new NullReferenceException();
        }
    }

    /// <summary>
    /// Stops the given sound
    /// </summary>
    /// <param name="name">The name of the sound. </param>
    /// <returns>False if there was no sound with that name that playing, 
    /// true otherwise. </returns>
    public bool StopPlaying (string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null) {
            s.source.Stop();
            return true;
        }
        else {
            return false;
        }
    }

    public void StopAll() {

        Debug.Log("Attempting to Stop all Sound. ");

        foreach (Sound s in sounds) {

            if (s != null) {

                s.source.Stop();
            }          
        }
    }

    /// <summary>
    /// Stops a sound if it is already playing, and plays it again. 
    /// </summary>
    /// <param name="name"></param>
    public void SinglePlay(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null) {
            if (s.source.isPlaying) s.source.Stop();
            s.source.Play();
        }
        else {
            Debug.LogWarning("No sound with name: " + name + " was found.");
        }
    }
}
