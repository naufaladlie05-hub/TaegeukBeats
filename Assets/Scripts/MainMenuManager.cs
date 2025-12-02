using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitApp()
    {
        Debug.Log("Keluar dari Aplikasi...");
        Application.Quit();
    }
}