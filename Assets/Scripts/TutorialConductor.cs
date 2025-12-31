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

    [Header("Audio Aba-aba Standard")]
    public AudioClip cueSoundF;     
    public AudioClip cueSoundJ;   
    public float cueOffset = 0.40f;

    [Header("Audio Aba-aba Spesial (1-2)")]
    public AudioClip soundOne;     
    public AudioClip soundTwo;     
    [Tooltip("Berapa detik SEBELUM hit, suara 'One' mulai bunyi?")]
    public float countInLeadTime = 1.5f;

    [Header("PENGATURAN KALIBRASI")]
    [Tooltip("Geser angka ini untuk memajukan/mundurkan SEMUA note sekaligus.")]
    public float globalOffset = 0f;

    [Header("Setting Lain")]
    public float noteSpeed = 5f;
    public float playerXPosition = -6f;
    public float musicDelay = 2f;

    [Header("Data Lagu")]
    public List<NoteData> songNotes = new List<NoteData>();

    private int nextSpawnIndex = 0;
    private int nextCueIndex = 0;
    private int nextCountInIndex = 0;

    private float timeToReachPlayer;
    private bool songStarted = false;

    void Start()
    {
        if (sfxSource == null) sfxSource = musicSource;
        SetupSongData();

        float distance = Mathf.Abs(spawnPoint.position.x - playerXPosition);
        timeToReachPlayer = distance / noteSpeed;

        Invoke("StartMusic", musicDelay);
    }

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
        AddNote(31.40266f, "F", true); 
        AddNote(32.40531f, "F");       
        AddNote(32.896f, "F");         
        AddNote(33.38666f, "J");       
        AddNote(36.39466f, "J");       
    }

    void AddNote(float recordedTime, string noteType, bool countIn = false)
    {
        NoteData newData = new NoteData();
        newData.time = recordedTime + globalOffset;
        newData.type = noteType;
        newData.useCountIn = countIn;
        songNotes.Add(newData);
    }

    void StartMusic()
    {
        musicSource.Play();
        songStarted = true;
    }

    void Update()
    {
        if (!songStarted) return;
        if (!musicSource.isPlaying && musicSource.time == 0) return;

        if (nextSpawnIndex < songNotes.Count)
        {
            NoteData spawnNote = songNotes[nextSpawnIndex];
            float spawnTime = spawnNote.time - timeToReachPlayer;

            if (musicSource.time >= spawnTime)
            {
                SpawnNote(spawnNote.type);
                nextSpawnIndex++;
            }
        }

        if (nextCueIndex < songNotes.Count)
        {
            NoteData cueNote = songNotes[nextCueIndex];
            float cueTime = cueNote.time - cueOffset;

            if (musicSource.time >= cueTime)
            {
                if (cueNote.type == "J" && cueSoundJ != null) sfxSource.PlayOneShot(cueSoundJ);
                else if (cueSoundF != null) sfxSource.PlayOneShot(cueSoundF);

                nextCueIndex++;
            }
        }

        if (nextCountInIndex < songNotes.Count)
        {
            NoteData countNote = songNotes[nextCountInIndex];

            if (countNote.useCountIn)
            {
                float startCountTime = countNote.time - countInLeadTime;
                if (musicSource.time >= startCountTime)
                {
                    StartCoroutine(PlayCountInSequence());
                    nextCountInIndex++;
                }
            }
            else
            {
                if (musicSource.time >= countNote.time) nextCountInIndex++;
            }
        }
    }

    IEnumerator PlayCountInSequence()
    {
        if (soundOne != null)
        {
            sfxSource.PlayOneShot(soundOne);
            yield return new WaitForSeconds(soundOne.length);
        }
        else yield return new WaitForSeconds(0.5f);

        if (soundTwo != null) sfxSource.PlayOneShot(soundTwo);
    }

    void SpawnNote(string type)
    {
        GameObject prefabToSpawn = (type == "J") ? notePrefabJ : notePrefabF;
        GameObject newNote = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

        NoteObject noteScript = newNote.GetComponent<NoteObject>();
        if (noteScript != null) noteScript.speed = noteSpeed;
    }
}