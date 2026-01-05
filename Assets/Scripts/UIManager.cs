using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Mengatur semua tampilan antarmuka (UI) seperti Skor, HP bar, dan Panel Menu.
/// Mendengarkan event dari GameManager (Observer Pattern) untuk update otomatis.
/// </summary>
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

    void Start()
    {
        // OBSERVER: Mendaftar (Subscribe) ke event GameManager agar UI update otomatis
        if (GameManager.instance != null)
        {
            GameManager.instance.OnGameStateChanged += UpdateHUD;
        }
    }

    void OnDestroy()
    {
        // OBSERVER: Berhenti langganan (Unsubscribe) saat objek hancur untuk mencegah memory leak
        if (GameManager.instance != null)
        {
            GameManager.instance.OnGameStateChanged -= UpdateHUD;
        }
    }

    /// <summary>
    /// Memperbarui tampilan HUD (Heads Up Display) in-game.
    /// Fungsi ini dipanggil otomatis oleh event GameManager saat skor/darah berubah.
    /// </summary>
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

    // Helper untuk mengatur animasi fade in/out panel UI
    private void TogglePanel(CanvasGroup cg, bool show)
    {
        if (cg == null) return;

        StopAllCoroutines();
        StartCoroutine(AnimatePanel(cg, show));
    }

    // Coroutine untuk transisi halus (Alpha fading)
    IEnumerator AnimatePanel(CanvasGroup cg, bool show)
    {
        float duration = 0.4f;
        float startAlpha = cg.alpha;
        float endAlpha = show ? 1f : 0f;

        if (show)
        {
            cg.gameObject.SetActive(true);
            cg.interactable = true;
            cg.blocksRaycasts = true;
            if (cg.alpha > 0.9f) cg.alpha = 0f; // Reset alpha kalau mau fade in
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
            // Menggunakan unscaledDeltaTime agar animasi tetap jalan saat game di-pause
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
            pauseButtonRef.SetActive(!isActive); // Sembunyikan tombol pause saat panel muncul
        }
    }

    /// <summary>
    /// Menampilkan panel hasil akhir (Finish/Game Over) beserta Rank yang didapat.
    /// </summary>
    public void ShowResultPanel(int finalScore, int maxCombo, string rank, Color rankColor)
    {
        if (finalScoreText != null) finalScoreText.text = "Final Score: " + finalScore;
        if (maxComboText != null) maxComboText.text = "Max Combo: " + maxCombo;
        if (rankText != null)
        {
            rankText.text = rank;
            rankText.color = rankColor;
            if (rank == "S") rankText.fontSize = rankText.fontSize * 1.5f; // Efek visual jika dapat rank S
        }

        TogglePanel(resultCG, true);
    }

    public void TriggerGameOverAnim()
    {
        TogglePanel(gameOverCG, true);
    }
}