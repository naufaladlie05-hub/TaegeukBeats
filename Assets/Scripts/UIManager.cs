using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Main Canvas Groups")]
    public CanvasGroup helpCG;
    public CanvasGroup pauseCG;
    public CanvasGroup resultCG;
    public CanvasGroup gameOverCG;

    [Header("HUD Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public Slider hpSlider;
    public GameObject pauseButtonRef;

    [Header("Result Elements")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI maxComboText;
    public TextMeshProUGUI rankText;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateHUD(int score, int combo, float currentHP, float maxHP)
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (comboText != null) comboText.text = "Combo: " + combo;
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
    }




    private void TogglePanel(CanvasGroup cg, bool show) //set
    {
        if (cg == null) return;

        StopAllCoroutines();

        StartCoroutine(AnimatePanel(cg, show));
    }

    IEnumerator AnimatePanel(CanvasGroup cg, bool show)
    {
        float duration = 0.4f; // fade speed
        float startAlpha = cg.alpha;
        float endAlpha = show ? 1f : 0f;



        if (show)
        {
            cg.gameObject.SetActive(true);
            cg.interactable = true;
            cg.blocksRaycasts = true;
            if (cg.alpha > 0.9f) cg.alpha = 0f;
            startAlpha = cg.alpha;
        }
        else
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }



        float timer = 0f;
        while (timer < duration)
        {
            // fix Game Pause timescale
            timer += Time.unscaledDeltaTime;

            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);

            yield return null;
        }



        cg.alpha = endAlpha;

        if (!show)
        {
            cg.gameObject.SetActive(false);
        }
    }






    public void SetHelpPanel(bool isActive)
    {
        if (helpCG != null)
        {
            StopAllCoroutines();

            helpCG.alpha = isActive ? 1f : 0f;

            helpCG.interactable = isActive;
            helpCG.blocksRaycasts = isActive;

            helpCG.gameObject.SetActive(isActive);
        }
    }



    public void SetPausePanel(bool isActive)
    {
        TogglePanel(pauseCG, isActive);
        if (pauseButtonRef != null)
        {
            pauseButtonRef.SetActive(!isActive);
        }
    }

    public void ShowResultPanel(int finalScore, int maxCombo, string rank, Color rankColor)
    {
        if (finalScoreText != null) finalScoreText.text = "Final Score: " + finalScore;
        if (maxComboText != null) maxComboText.text = "Max Combo: " + maxCombo;
        if (rankText != null)
        {
            rankText.text = rank;
            rankText.color = rankColor;
            if (rank == "S") rankText.fontSize = rankText.fontSize * 1.5f;
        }

        TogglePanel(resultCG, true);
    }

    public void TriggerGameOverAnim()
    {
        TogglePanel(gameOverCG, true);
    }
}