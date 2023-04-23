using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFinishedMenu : MonoBehaviour
{
    public GameObject gameFinishedPanel;
    public TextMeshProUGUI topTimesText;
    public GameObject topTimesList;
    public GameObject topTimeListItemPrefab;

    private void OnEnable()
    {
        List<float> topTimes = GetTopTimes();
        Debug.Log("TopTimesText: " + topTimesText); // Add this line
        UpdateTopTimesText(topTimes);
    }


    public void ShowGameFinishedMenu(List<float> topTimes)
    {
        // Show the game finished panel
        UpdateTopTimesText(topTimes);
        gameFinishedPanel.SetActive(true);
    }

    public void UpdateTopTimesText(List<float> topTimes)
    {
        string topTimesString = "";
        for (int i = 0; i < topTimes.Count; i++)
        {
            string ordinal = GetOrdinalSuffix(i + 1);
            topTimesString += $"{i + 1}{ordinal}: {FormatTime(topTimes[i])}\n";
        }
        topTimesText.text = topTimesString;
    }

    private string GetOrdinalSuffix(int number)
    {
        int lastDigit = number % 10;
        int lastTwoDigits = number % 100;

        if (lastDigit == 1 && lastTwoDigits != 11)
        {
            return "st";
        }
        else if (lastDigit == 2 && lastTwoDigits != 12)
        {
            return "nd";
        }
        else if (lastDigit == 3 && lastTwoDigits != 13)
        {
            return "rd";
        }
        else
        {
            return "th";
        }
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
   
    public void CloseGameFinishedMenu()
    {
        gameFinishedPanel.SetActive(false);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public void ReturnToMainMenu()
    {
        // Replace "MainMenu" with your main menu scene name
        SceneManager.LoadScene("MainMenu");
        FindObjectOfType<MenuAudioManager>().RestartMusic();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetFloat("LastCompletedTime", Time.timeSinceLevelLoad);
        
        // Add this line to close the game finished menu before restarting
        CloseGameFinishedMenu();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        FindObjectOfType<MenuAudioManager>().RestartMusic();
    }

   public void QuitGame()
    {
        FindObjectOfType<MenuAudioManager>().StopMusic();
        Application.Quit();
    }


    // Call this method to save the new best times
    public void SaveBestTimes(List<float> bestTimes)
    {
        for (int i = 0; i < bestTimes.Count; i++)
        {
            PlayerPrefs.SetFloat($"BestTime_{i}", bestTimes[i]);
        }
        PlayerPrefs.Save();
    }

    // Call this method to load the saved best times
    public List<float> LoadBestTimes(int count)
    {
        List<float> bestTimes = new List<float>();
        for (int i = 0; i < count; i++)
        {
            float time = PlayerPrefs.GetFloat($"BestTime_{i}", float.MaxValue);
            bestTimes.Add(time);
        }
        return bestTimes;
    }
}
