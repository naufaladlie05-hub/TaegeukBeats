using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject helpPanel; 

    public void PlayGame()
    {
        SceneManager.LoadScene("Story_Intro");
    }

    public void OpenHelp()
    {
        if (helpPanel != null) helpPanel.SetActive(true);
    }

    public void CloseHelp()
    {
        if (helpPanel != null) helpPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}