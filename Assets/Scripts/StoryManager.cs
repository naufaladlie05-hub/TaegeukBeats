using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; 

public class StoryManager : MonoBehaviour
{
    [Header("Komponen UI")]
    public Image displayImage; 
    public TextMeshProUGUI promptText; 

    [Header("Data Cerita")]
    public Sprite[] storyPages; 
    public string nextSceneName = "Level_Tutorial";

    private int currentIndex = 0;

    void Start()
    {
        if (storyPages.Length > 0)
        {
            currentIndex = 0;
            UpdateDisplay();
        }
    }

    public void OnClickNext()
    {
        currentIndex++; 

        if (currentIndex < storyPages.Length)
        {
            UpdateDisplay();
        }
        else
        {
            SkipStory();
        }
    }



    void UpdateDisplay()
    {
        if (displayImage != null)
            displayImage.sprite = storyPages[currentIndex];

        if (promptText != null)
        {
            if (currentIndex == storyPages.Length - 1)
            {
                promptText.text = " START TUTORIAL >>>"; 
            }
            else
            {
                promptText.text = "Click to continue..."; 
            }
        }
    }

    public void SkipStory()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}