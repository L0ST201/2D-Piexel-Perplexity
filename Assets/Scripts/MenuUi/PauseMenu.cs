using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button resumeButton;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        FindObjectOfType<MenuAudioManager>().RestartMusic();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        List<float> topTimes = GetTopTimes();
        topTimes.Add(Time.timeSinceLevelLoad);
        topTimes.Sort();
        SaveTopTimes(topTimes);
        
        // Add this line to close the game finished menu before restarting
        FindObjectOfType<GameFinishedMenu>().CloseGameFinishedMenu();
        FindObjectOfType<MenuAudioManager>().RestartMusic();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private List<float> GetTopTimes()
    {
        List<float> topTimes = new List<float>();
        int count = PlayerPrefs.GetInt("TopTimesCount", 0);

        for (int i = 0; i < count; i++)
        {
            topTimes.Add(PlayerPrefs.GetFloat($"TopTime_{i}"));
        }

        return topTimes;
    }

    private void SaveTopTimes(List<float> topTimes)
    {
        int count = Mathf.Min(topTimes.Count, 5); // Save only the top 5 times

        for (int i = 0; i < count; i++)
        {
            PlayerPrefs.SetFloat($"TopTime_{i}", topTimes[i]);
        }
        PlayerPrefs.SetInt("TopTimesCount", count);
        PlayerPrefs.Save();
    }

    public void QuitGame()
    {
        FindObjectOfType<MenuAudioManager>().StopMusic();
        Debug.Log("Quitting game...");
        Application.Quit();
    }

}
