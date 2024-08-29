using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("UI Settings")]
    public TextMeshProUGUI scoreText;      // Reference to the TMP text component displaying the score
    public TextMeshProUGUI highScoreText;  // Reference to the TMP text component displaying the high score

    private int score = 0; // Current score
    private int highScore = 0; // High score
    private HashSet<string> processedPulpits = new(); // Set to track processed pulpits

    private const string ScoreKey = "currentScore"; // PlayerPrefs key for the score
    private const string HighScoreKey = "highScore"; // PlayerPrefs key for the high score


    void Start()
    {
        InitializeScore();
        InitializeHighScore();
        UpdateScoreUI();
        UpdateHighScoreUI();
    }

    private void InitializeScore()
    {
        // Initialize or reset the score from PlayerPrefs
        PlayerPrefs.SetInt(ScoreKey, 0);
        score = PlayerPrefs.GetInt(ScoreKey, 0);
    }

    private void InitializeHighScore()
    {
        // Load the high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        string objectName = CleanObjectName(other.gameObject.name);

        if (IsPulpit(objectName) && !HasProcessed(objectName))
        {
            ProcessPulpit(objectName);
        }
    }

    private string CleanObjectName(string name)
    {
        return name.Replace("(Clone)", "");
    }

    private bool IsPulpit(string name)
    {
        return name.StartsWith("platform");
    }

    private bool HasProcessed(string name)
    {
        return processedPulpits.Contains(name);
    }

    private void ProcessPulpit(string objectName)
    {
        processedPulpits.Add(objectName);

        if (TryParsePulpitScore(objectName, out int pulpitScore))
        {
            UpdateScoreIfNecessary(pulpitScore);
        }
        else
        {
            Debug.LogError("Failed to convert pulpit number to score.");
        }
    }

    private bool TryParsePulpitScore(string objectName, out int score)
    {
        string pulpitNumberStr = objectName.Replace("platform", "");
        return int.TryParse(pulpitNumberStr, out score);
    }

    private void UpdateScoreIfNecessary(int pulpitScore)
    {
        if (pulpitScore > score)
        {
            score = pulpitScore;
            UpdateScoreUI();
            SaveScore();

            if (score > highScore)
            {
                UpdateHighScore();
            }
        }
    }

    private void UpdateHighScore()
    {
        highScore = score;
        UpdateHighScoreUI();
        SaveHighScore();
    }

    private void SaveScore()
    {
        PlayerPrefs.SetInt(ScoreKey, score);
        PlayerPrefs.Save();
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt(HighScoreKey, highScore);
        PlayerPrefs.Save();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = score.ToString();  // Update the score display
    }

    private void UpdateHighScoreUI()
    {
        highScoreText.text = "High Score: " + highScore.ToString();  // Update the high score display
    }
}
