using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameMenuManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pausePanel;
    public CanvasGroup pauseCanvasGroup;

    public GameObject gameOverPanel;
    public CanvasGroup gameOverCanvasGroup; 

    public float fadeDuration = 0.5f; 

    private bool isPaused = false;

    void Start()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
            if (pauseCanvasGroup != null) pauseCanvasGroup.alpha = 0;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            if (gameOverCanvasGroup != null) gameOverCanvasGroup.alpha = 0;
        }
    }

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
        if (pauseCanvasGroup != null) StartCoroutine(FadeCanvas(pauseCanvasGroup, 0f, 1f));
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pauseCanvasGroup != null)
        {
            StartCoroutine(FadeCanvas(pauseCanvasGroup, 1f, 0f, () =>
            {
                pausePanel.SetActive(false);
                Time.timeScale = 1f;
            }));
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void TriggerGameOver()
    {
        gameOverPanel.SetActive(true);

        if (gameOverCanvasGroup != null)
        {
            StartCoroutine(FadeCanvas(gameOverCanvasGroup, 0f, 1f));
        }
    }

    IEnumerator FadeCanvas(CanvasGroup cg, float start, float end, System.Action onComplete = null)
    {
        float timer = 0f;

        if (end == 1f)
        {
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(start, end, timer / fadeDuration);
            yield return null;
        }
        cg.alpha = end;
        if (onComplete != null) onComplete.Invoke();
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
}