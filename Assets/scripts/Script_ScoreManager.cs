using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    void Start()
    {
        PlayerPrefs.SetInt("current_score", 0);
        score = PlayerPrefs.GetInt("current_score", 0);
        UpdateScoreUI();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.StartsWith("Pulpit_"))
        {
            string pulpitNumberStr = collision.gameObject.name.Replace("Pulpit_", "");
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
