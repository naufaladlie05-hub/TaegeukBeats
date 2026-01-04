using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
    public int score = 0;
    public int combo = 0;
    public int maxCombo = 0;
    public float currentHP = 100f;
    public float maxHP = 100f;
    public float damage = 20f;
    public float heal = 5f;

    public bool isGameOver = false;
    public bool isGamePaused = false;
    private bool hasStarted = false;

    private void Awake()
    {
        instance = this;
        // optimize
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }





    // stat
    void Start()
    {
        currentHP = maxHP;
        Time.timeScale = 1f;

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
        if (Input.GetKeyDown(KeyCode.Escape) && hasStarted && !isGameOver)
        {
            TogglePause();
        }
    }

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








    // main/hit logic
    public void CheckHit(bool isInputF)
    {
        if (isGameOver || isGamePaused || !hasStarted) return;

        NoteObject noteToHit = mySensor.GetHittableNote(isInputF);

        if (noteToHit != null)
        {
            HitSuccess(isInputF);
            noteToHit.enabled = false;

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
                GhostBehavior ghost = noteToHit.GetComponentInChildren<GhostBehavior>();
                if (ghost != null) ghost.OnPlayerHitMe();
                Destroy(noteToHit.gameObject, (ghost != null) ? 1f : 0f);
            }
        }
    }



    void ChangeHealth(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (UIManager.instance != null)
            UIManager.instance.UpdateHUD(score, combo, currentHP, maxHP);

        if (currentHP <= 0 && !isGameOver)
        {
            GameOver();
        }
    }

    void HitSuccess(bool isTypeF)
    {
        score += 100 + (combo * 10);
        combo++;
        if (combo > maxCombo) maxCombo = combo;
        ChangeHealth(heal);

        ShowPopup("PERFECT!", Color.green);
        PlayHitSound(isTypeF);
    }

    public void NoteMissed()
    {
        if (!isGameOver && hasStarted)
        {
            combo = 0;
            ChangeHealth(-damage);

            ShowPopup("MISS!", Color.red);
        }
    }

    void GameOver()
    {
        isGameOver = true;

        if (conductor != null) conductor.PauseMusic();
        Time.timeScale = 0f;

        if (UIManager.instance != null) UIManager.instance.TriggerGameOverAnim();
    }





    public void ShowPopup(string text, Color color)
    {
        if (popupTextPrefab != null)
        {
            GameObject spawnTarget = GameObject.Find("PopupSpawnPoint");

            Vector3 spawnPos = Vector3.zero;

            if (spawnTarget != null)
            {
                spawnPos = spawnTarget.transform.position;
            }
            else
            {
                spawnPos = new Vector3(-6f, 2f, 0f);
            }

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







    public void ShowResults()
    {
        if (isGameOver) return;
        isGameOver = true;

        string rank = "D";
        Color rankColor = Color.white; 

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