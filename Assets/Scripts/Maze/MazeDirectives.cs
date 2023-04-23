using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public class MazeDirectives : MonoBehaviour
{
    public int keysToFind;
    public TextMeshProUGUI keysValueText;
    public TextMeshProUGUI timerText;
    public MazeGoal mazeGoalPrefab;
    public MazeKey mazeKeyPrefab;
    public PauseMenu pauseMenu;
    public GameFinishedMenu gameFinishedMenu;
    public delegate void MazeCompletedEventHandler();
    public static event MazeCompletedEventHandler OnMazeCompleted;
    MazeGoal mazeGoal;
    int foundKeys;
    List<Vector3> mazeKeyPositions;

    List<float> topTimes = new List<float>();
    int maxTopTimes = 5;
    float timer;

    private bool playerStarted = false;
    private bool mazeFinished = false;
    private float startTime;
    private float elapsedTime;

    void Awake()
    {
        MazeGenerator.OnMazeReady += StartDirectives;
        pauseMenu = FindObjectOfType<PauseMenu>();
        gameFinishedMenu = FindObjectOfType<GameFinishedMenu>();

        // Load the saved best times
        int count = PlayerPrefs.GetInt("TopTimesCount", 0);
        topTimes = gameFinishedMenu.LoadBestTimes(count);
    }


    void Update()
    {
        if (!mazeFinished)
        {
            timer += Time.deltaTime;
            UpdateTimerText();
        }
    }

    void StartDirectives()
    {
        mazeGoal = Instantiate(mazeGoalPrefab, MazeGenerator.instance.mazeGoalPosition, Quaternion.identity) as MazeGoal;
        mazeGoal.transform.SetParent(transform);

        mazeKeyPositions = MazeGenerator.instance.GetRandomFloorPositions(keysToFind);

        for (int i = 0; i < mazeKeyPositions.Count; i++)
        {
            MazeKey mazeKey = Instantiate(mazeKeyPrefab, mazeKeyPositions[i], Quaternion.identity) as MazeKey;
            mazeKey.transform.SetParent(transform);
        }

        SetKeyValueText();
        timer = 0f;
    }

    public void OnGoalReached()
    {
        Debug.Log("Goal Reached");
        if (foundKeys == keysToFind)
        {
            Debug.Log("Escape the maze");
            mazeFinished = true;

            // Add the time to the top times list and sort it
            topTimes.Add(timer);
            topTimes.Sort();

            // Keep only the top 5 times
            if (topTimes.Count > maxTopTimes)
            {
                topTimes.RemoveAt(topTimes.Count - 1);
            }

            // Save the best times
            gameFinishedMenu.SaveBestTimes(topTimes);

            if (gameFinishedMenu != null)
            {
                gameFinishedMenu.ShowGameFinishedMenu(topTimes); // Updated this line
            }
            else
            {
                Debug.LogError("GameFinishedMenu reference not found in MazeDirectives");
            }
            PlayerPrefs.SetFloat("LastCompletedTime", timer);
            PlayerPrefs.Save();

            OnMazeCompleted?.Invoke();
        }
    }


public List<float> GetTopTimes()
{
    return topTimes;
}

public void OnKeyFound()
{
    foundKeys++;

    SetKeyValueText();

    if (foundKeys == keysToFind)
    {
        GetComponentInChildren<MazeGoal>().OpenGoal();
    }
}

void SetKeyValueText()
{
    keysValueText.text = foundKeys.ToString() + " of " + keysToFind.ToString();
}

private void UpdateTimerText()
{
    if (!playerStarted)
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            playerStarted = true;
            startTime = Time.time;
        }
    }
    else
    {
        if (!mazeFinished)
        {
            elapsedTime = Time.time - startTime;
            string minutes = ((int)elapsedTime / 60).ToString("00");
            string seconds = (elapsedTime % 60).ToString("00.00");
            timerText.text = $"{minutes}:{seconds}";
        }
    }
}

public void GenerateNewMaze()
{
    // Destroy the existing maze first
    foreach (Transform child in transform)
    {
        Destroy(child.gameObject);
    }

    // Then generate a new maze
    StartCoroutine(MazeGenerator.instance.GenerateNewMaze());
}

void OnDestroy()
{
    MazeGenerator.OnMazeReady -= StartDirectives;
}
}
