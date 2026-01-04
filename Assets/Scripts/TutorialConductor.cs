using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class NoteData
{
    public float time;
    public string type;
    public bool useCountIn;
}

public class TutorialConductor : MonoBehaviour
{
    [Header("Komponen")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public GameObject notePrefabF;
    public GameObject notePrefabJ;
    public Transform spawnPoint;

    [Header("Audio Aba-aba")]
    public AudioClip cueSoundF;
    public AudioClip cueSoundJ;

    [Header("Setting")]
    public float noteSpeed = 5f;
    public float playerXPosition = -6f;
    public float musicDelay = 2f;
    public float globalOffset = 0f;
    public float cueOffset = 0.40f;
    public float countInLeadTime = 1.5f;

    [Header("Data Lagu")]
    public List<NoteData> songNotes = new List<NoteData>();
    private bool isPaused = false;
    private int nextSpawnIndex = 0;
    private int nextCueIndex = 0;
    private float timeToReachPlayer;
    private bool songStarted = false;
    private bool tutorialFinished = false;

    void Start()
    {
        if (sfxSource == null) sfxSource = musicSource;
        SetupSongData();

        float distance = Mathf.Abs(spawnPoint.position.x - playerXPosition);
        timeToReachPlayer = distance / noteSpeed;
    }


    public void BeginGameplay()
    {
        Invoke("StartMusic", musicDelay);
    }

    void StartMusic()
    {
        Debug.Log("Start Music");
        musicSource.Play();
        songStarted = true;
    }

    public void PauseMusic()
    {
        if (songStarted && musicSource.isPlaying)
        {
            musicSource.Pause();
            isPaused = true;
        }
    }

    public void ResumeMusic()
    {
        if (songStarted && isPaused)
        {
            musicSource.UnPause();
            isPaused = false;
        }
    }





    void Update()
    {
        if (!songStarted || isPaused) return;

        if (!musicSource.isPlaying && musicSource.time == 0 && !tutorialFinished)
        {
            if (musicSource.timeSamples >= musicSource.clip.samples - 100 || (!musicSource.isPlaying && songStarted))
            {
                tutorialFinished = true;
                Debug.Log("song finish");
                if (GameManager.instance != null)
                {
                    GameManager.instance.Invoke("ShowResults", 1f);
                }
                return;
            }
        }







        // sinkron note dgn musik
        float songTime = musicSource.time;

        if (nextSpawnIndex < songNotes.Count) //note
        {
            NoteData spawnNote = songNotes[nextSpawnIndex];

            float spawnTime = spawnNote.time - timeToReachPlayer;

            if (songTime >= spawnTime) { SpawnNote(spawnNote.type); nextSpawnIndex++; }
        }

        if (nextCueIndex < songNotes.Count) // cue sound
        {
            NoteData cueNote = songNotes[nextCueIndex];
            float cueTime = cueNote.time - cueOffset;
            if (songTime >= cueTime)
            {
                if (cueNote.type == "J" && cueSoundJ != null) sfxSource.PlayOneShot(cueSoundJ);
                else if (cueSoundF != null) sfxSource.PlayOneShot(cueSoundF);
                nextCueIndex++;
            }
        }
    }





    // rhytmrecord debug
    void SetupSongData()
    {
        songNotes.Clear(); 
        AddNote(1.429312f, "F");
        AddNote(5.354656f, "F");
        AddNote(7.424f, "F");
        AddNote(9.429313f, "F");
        AddNote(10.38931f, "F");
        AddNote(11.43466f, "F");
        AddNote(13.41866f, "J");
        AddNote(17.408f, "F");
        AddNote(21.39731f, "F");
        AddNote(23.38131f, "F");
        AddNote(25.408f, "F");
        AddNote(26.368f, "F");
        AddNote(27.41331f, "F");
        AddNote(28.88531f, "J");
        AddNote(31.40266f, "F");
        AddNote(32.40531f, "F");
        AddNote(32.896f, "F");
        AddNote(33.38666f, "J");
        AddNote(36.39466f, "J");
        AddNote(38.79466f, "J");

    }

    void AddNote(float recordedTime, string noteType, bool countIn = false)
    {
        NoteData newData = new NoteData();
        newData.time = recordedTime + globalOffset;
        newData.type = noteType;
        newData.useCountIn = countIn;
        songNotes.Add(newData);
    }

    void SpawnNote(string type) 
    {
        GameObject prefabToSpawn = (type == "J") ? notePrefabJ : notePrefabF; // f/j set
        GameObject newNote = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
        NoteObject noteScript = newNote.GetComponent<NoteObject>();
        if (noteScript != null) noteScript.speed = noteSpeed;
    }
}