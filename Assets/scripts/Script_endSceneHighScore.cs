using UnityEngine;
using TMPro;

public class EndSceneHighScore : MonoBehaviour
{
    public TextMeshProUGUI highScoreText; 

    void Start()
    {
        DisplayHighScore();
    }

    private void DisplayHighScore()
    {
        int highScore = PlayerPrefs.GetInt("highScore", 0); // Load the high score
        highScoreText.text = "Your High Score: " + highScore.ToString(); 
    }
}
