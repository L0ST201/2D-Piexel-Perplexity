using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    public AudioClip buttonHover;
    public AudioClip buttonClick;
    public AudioClip backgroundMusic;
    public GameObject pauseMenuPanel;
    public GameObject gameFinishedPanel;
    public GameObject startMenuPanel;

    private AudioSource audioSource;
    private AudioSource backgroundMusicSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    private void Update()
    {
        if (pauseMenuPanel.activeInHierarchy || gameFinishedPanel.activeInHierarchy || startMenuPanel.activeInHierarchy)
        {
            backgroundMusicSource.Pause();
        }
        else
        {
            if (!backgroundMusicSource.isPlaying)
            {
                backgroundMusicSource.Play();
            }
        }
    }

    public void PlayButtonHoverSound()
    {
        audioSource.PlayOneShot(buttonHover);
    }

    public void PlayButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClick);
    }

    public void RestartMusic()
    {
        backgroundMusicSource.Stop();
        backgroundMusicSource.Play();
    }

    public void StopMusic()
    {
        backgroundMusicSource.Stop();
    }
}
