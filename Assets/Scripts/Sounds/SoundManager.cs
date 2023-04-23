using System;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public Sound[] sounds;
    public static SoundManager Instance;

    void Awake() {
        Instance = this;
        
        foreach (Sound sound in sounds) {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.isLooping;
        }
    }

    public void Play(string name) {
       Sound sound = Array.Find(sounds, sound => sound.name == name);
       sound.audioSource.Play();
    }
}
