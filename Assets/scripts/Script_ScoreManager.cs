using System.Collections.Generic;
using UnityEngine;
using TMPro;  

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;  
    private int score = 0;
    private HashSet<string> processedPulpits = new();

    void Start()
    {
        PlayerPrefs.SetInt("current_score", 0);
        score = PlayerPrefs.GetInt("current_score", 0);
        UpdateScoreUI();
    }

    void OnTriggerEnter(Collider other)
    {
        
        string objectName = other.gameObject.name.Replace("(Clone)", "");

        // Check if the name starts with "Pulpit"
        if (objectName.StartsWith("capsule") && !processedPulpits.Contains(objectName))
        {
            processedPulpits.Add(objectName);

            string pulpitNumberStr = objectName.Replace("capsule", "");
            if (int.TryParse(pulpitNumberStr, out int pulpitScore))
            {
                if (pulpitScore > score)
                {
                    score = pulpitScore;
                    UpdateScoreUI();
                    PlayerPrefs.SetInt("current_score", score);
                    PlayerPrefs.Save();
                }
            }
            else
            {
                Debug.LogError("Failed to convert pulpit number to score.");
            }
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "" + score;  
    }
}
