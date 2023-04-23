using UnityEngine;

[System.Serializable]
public class Sound {
    public string name;
    [Range(0,1)]
    public float volume;
    [Range(0.3f,2)]
    public float pitch;
    public bool isLooping;
    public AudioClip audioClip;
    [HideInInspector] public AudioSource audioSource;
}
