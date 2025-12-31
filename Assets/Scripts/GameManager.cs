using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections; 

public class GameManager : MonoBehaviour
{
    [Header("Setting")]
    public HitSensor mySensor;
    public GameMenuManager menuManager;
    public float targetX = -5f;
    public float hitWindow = 1.5f;


    [Header("Komponen UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public Slider hpSlider;

    [Header("Popup System")]
    public GameObject popupTextPrefab;
    public Transform spawnPoint;

    [Header("Data Game")]
    public int score = 0;
    public int combo = 0;
    public float currentHP = 100f;
    public float maxHP = 100f;
    public float damage = 10f;
    public float heal = 2f;

    [Header("Audio SFX")]
    public AudioSource sfxSource;
    public AudioClip hitSound;
    public AudioClip enemyHurtSound;       
    public AudioClip enemyHurtSoundSpecial;

    private bool isGameOver = false;

    void Start()
    {
        currentHP = maxHP;
        UpdateUI();

    }

    public void CheckHit(bool isInputF)
    {
        if (isGameOver) return;

        NoteObject noteToHit = mySensor.GetHittableNote(isInputF);

        if (noteToHit != null)
        {
            HitSuccess(isInputF);

            GhostBehavior ghost = noteToHit.GetComponentInChildren<GhostBehavior>();

            if (ghost != null)
            {
                ghost.OnPlayerHitMe(); 

                noteToHit.enabled = false;

                mySensor.notesInRange.Remove(noteToHit);
            }
            else
            {
                Destroy(noteToHit.gameObject);
            }
        }
        else
        {
           
        }
    }

    public void NoteMissed()
    {
        if (!isGameOver) HitFailed("MISS!");
    }

    public void QuitGame()
    {
        Debug.Log("Keluar Game...");
        Application.Quit();
    }

    void ShowPopup(string text, Color color)
    {
        GameObject popup = Instantiate(popupTextPrefab, spawnPoint.position, Quaternion.identity);
        popup.GetComponent<PopupText>().Setup(text, color);
    }

    void HitSuccess(bool isTypeF)
    {
        score += 100;
        combo++;
        ChangeHealth(heal);
        UpdateUI();
        ShowPopup("PERFECT!", Color.green);

        if (hitSound != null) sfxSource.PlayOneShot(hitSound);

        if (isTypeF)
        {
            if (enemyHurtSound != null) sfxSource.PlayOneShot(enemyHurtSound);
        }
        else
        {
            if (enemyHurtSoundSpecial != null) sfxSource.PlayOneShot(enemyHurtSoundSpecial);
            else if (enemyHurtSound != null) sfxSource.PlayOneShot(enemyHurtSound);
        }
    }

    void HitFailed(string message)
    {
        combo = 0;
        ChangeHealth(-damage);
        UpdateUI();
        ShowPopup(message, Color.red);
    }

    void ChangeHealth(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (currentHP <= 0 && !isGameOver) 
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        if (menuManager != null)
        {
            menuManager.TriggerGameOver();
        }

        FindObjectOfType<Spawner>().enabled = false;

        NoteObject[] allNotes = FindObjectsOfType<NoteObject>();
        foreach (NoteObject note in allNotes) Destroy(note.gameObject);

    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        comboText.text = "Combo: " + combo;
        if (hpSlider != null) hpSlider.value = currentHP;
    }
}