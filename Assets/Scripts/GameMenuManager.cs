using UnityEngine;
using UnityEngine.SceneManagement; 
public class GameMenuManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverPanel.activeSelf)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true); 
        Time.timeScale = 0f; 
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false); 
        Time.timeScale = 1f; 
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Keluar Game...");
    }

    public void TriggerGameOver()
    {
        gameOverPanel.SetActive(true);
    }
}