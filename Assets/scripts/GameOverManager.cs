using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;  
    public GameObject gameOverPanel;  
    public float fallThreshold = -10f; 

    private bool gameIsOver = false;

    void Start()
    {
      
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
     
        if (!gameIsOver && transform.position.y < fallThreshold)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        gameIsOver = true;
        gameOverPanel.SetActive(true);  
        gameOverText.text = "Game Over!";  
        Time.timeScale = 0f;  
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void QuitGame()
    {
        Application.Quit();  
        Debug.Log("Game Quit");  
    }
}
