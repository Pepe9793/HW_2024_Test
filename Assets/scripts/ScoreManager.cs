using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreTextTMP;
    private int score = 0;

    void Start()
    {
        UpdateScore();
    }

    public void IncrementScore()
    {
        score++;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreTextTMP.text = "Score: " + score.ToString();
    }
}
