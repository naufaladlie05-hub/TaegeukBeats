using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Script utama yang mengatur alur permainan, skor, dan logika menang/kalah.
/// Menggunakan pola Singleton dan Observer.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Observer Pattern: Event untuk memberi tahu script lain (seperti UI) kalau data game berubah
    public event Action<int, int, float, float> OnGameStateChanged;

    // Singleton Pattern: Memastikan hanya ada satu GameManager dalam game
    public static GameManager instance;

    [Header("Gameplay References")]
    public HitSensor mySensor;
    public TutorialConductor conductor;

    [Header("Audio SFX")]
    public AudioSource sfxSource;
    public AudioClip hitSound;
    public AudioClip enemyHurtSound;
    public AudioClip enemyHurtSoundSpecial;

    [Header("Popup System")]
    public GameObject popupTextPrefab;
    public Transform popupSpawnPoint;

    [Header("Game Data")]
    // Encapsulation: Variable private, akses dari luar harus via Property
    [SerializeField] private int score = 0;
    [SerializeField] private int combo = 0;
    [SerializeField] private int maxCombo = 0;
    [SerializeField] private float currentHP = 100f;

    // Property (Hanya bisa diambil nilainya/Read Only oleh script lain)
    public int Score => score;
    public int Combo => combo;
    public float CurrentHP => currentHP;

    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float heal = 5f;

    public bool isGameOver = false;
    public bool isGamePaused = false;
    private bool hasStarted = false;

    private void Awake()
    {
        instance = this;
        // Optimasi frame rate biar smooth
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    // Inisialisasi status awal game
    void Start()
    {
        currentHP = maxHP;
        Time.timeScale = 1f;

        // Cek apakah UIManager ada, kalau ada update tampilan awal
        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateHUD(score, combo, currentHP, maxHP);
            UIManager.instance.SetHelpPanel(true);
            UIManager.instance.SetPausePanel(false);
        }
        else
        {
            StartGameSequence();
        }
    }

    void Update()
    {
        // Tombol pause (ESC)
        if (Input.GetKeyDown(KeyCode.Escape) && hasStarted && !isGameOver)
        {
            TogglePause();
        }
    }

    /// <summary>
    /// Menutup panel bantuan dan memulai musik/gameplay.
    /// </summary>
    public void CloseHelpAndStart()
    {
        if (UIManager.instance != null) UIManager.instance.SetHelpPanel(false);
        StartGameSequence();
    }

    void StartGameSequence()
    {
        hasStarted = true;
        if (conductor != null) conductor.BeginGameplay();
    }

    /// <summary>
    /// Menghentikan atau melanjutkan game (Pause/Resume).
    /// Mengatur TimeScale agar game berhenti total saat pause.
    /// </summary>
    public void TogglePause()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0f;
            if (conductor != null) conductor.PauseMusic();
        }
        else
        {
            Time.timeScale = 1f;
            if (conductor != null) conductor.ResumeMusic();
        }

        if (UIManager.instance != null) UIManager.instance.SetPausePanel(isGamePaused);
    }

    public void ResumeGame()
    {
        if (isGamePaused) TogglePause();
    }

    /// <summary>
    /// Logika utama untuk mengecek input pemain.
    /// Memvalidasi apakah tombol yang ditekan sesuai dengan not yang ada di sensor.
    /// </summary>
    /// <param name="isInputF">True jika input F (atas), False jika input J (bawah)</param>
    public void CheckHit(bool isInputF)
    {
        if (isGameOver || isGamePaused || !hasStarted) return;

        // Minta sensor mencarikan not yang valid
        NoteObject noteToHit = mySensor.GetHittableNote(isInputF);

        if (noteToHit != null)
        {
            HitSuccess(isInputF);
            noteToHit.enabled = false;

            // Handle animasi kematian not/musuh
            Animator anim = noteToHit.GetComponent<Animator>();
            if (mySensor.notesInRange.Contains(noteToHit))
                mySensor.notesInRange.Remove(noteToHit);

            if (anim != null)
            {
                anim.SetTrigger("Die");
                Destroy(noteToHit.gameObject, 1f);
            }
            else
            {
                // Polymorphism: Memanggil method mati pada musuh (Ghost)
                GhostBehavior ghost = noteToHit.GetComponentInChildren<GhostBehavior>();
                if (ghost != null) ghost.OnPlayerHitMe();
                Destroy(noteToHit.gameObject, (ghost != null) ? 1f : 0f);
            }
        }
    }

    /// <summary>
    /// Mengubah darah pemain dan memicu event update ke UI.
    /// </summary>
    void ChangeHealth(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        // Observer: Kabari semua subscriber (seperti UI) bahwa data berubah
        OnGameStateChanged?.Invoke(score, combo, currentHP, maxHP);

        if (currentHP <= 0 && !isGameOver)
        {
            GameOver();
        }
    }

    // Dipanggil kalau pemain berhasil menekan not dengan tepat
    void HitSuccess(bool isTypeF)
    {
        score += 100 + (combo * 10); // Skor bertambah sesuai combo
        combo++;
        if (combo > maxCombo) maxCombo = combo;
        ChangeHealth(heal);

        ShowPopup("PERFECT!", Color.green);
        PlayHitSound(isTypeF);
    }

    /// <summary>
    /// Dipanggil otomatis oleh not jika not tersebut lewat tanpa ditekan.
    /// </summary>
    public void NoteMissed()
    {
        if (!isGameOver && hasStarted)
        {
            combo = 0; // Reset combo
            ChangeHealth(-damage);
            ShowPopup("MISS!", Color.red);

            // Update UI via event
            OnGameStateChanged?.Invoke(score, combo, currentHP, maxHP);
        }
    }

    void GameOver()
    {
        isGameOver = true;

        if (conductor != null) conductor.PauseMusic();
        Time.timeScale = 0f;

        if (UIManager.instance != null) UIManager.instance.TriggerGameOverAnim();
    }

    // Memunculkan teks efek (seperti "Perfect" atau "Miss") di layar
    public void ShowPopup(string text, Color color)
    {
        if (popupTextPrefab != null)
        {
            GameObject spawnTarget = GameObject.Find("PopupSpawnPoint");
            Vector3 spawnPos = (spawnTarget != null) ? spawnTarget.transform.position : new Vector3(-6f, 2f, 0f);

            GameObject popup = Instantiate(popupTextPrefab, spawnPos, Quaternion.identity);

            if (popup.GetComponent<PopupText>())
                popup.GetComponent<PopupText>().Setup(text, color);
        }
    }

    void PlayHitSound(bool isTypeF)
    {
        if (hitSound != null) sfxSource.PlayOneShot(hitSound);
        if (isTypeF && enemyHurtSound != null) sfxSource.PlayOneShot(enemyHurtSound);
        else if (!isTypeF && enemyHurtSoundSpecial != null) sfxSource.PlayOneShot(enemyHurtSoundSpecial);
    }

    /// <summary>
    /// Menghitung rank akhir berdasarkan skor dan menampilkannya.
    /// </summary>
    public void ShowResults()
    {
        if (isGameOver) return;
        isGameOver = true;

        string rank = "D";
        Color rankColor = Color.white;

        // Logika penentuan rank
        if (score >= 3500) { rank = "S"; rankColor = Color.yellow; }
        else if (score >= 3000) { rank = "A"; rankColor = Color.green; }
        else if (score >= 1500) { rank = "B"; rankColor = Color.cyan; }
        else { rank = "C"; rankColor = Color.grey; }

        if (UIManager.instance != null)
            UIManager.instance.ShowResultPanel(score, maxCombo, rank, rankColor);
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}