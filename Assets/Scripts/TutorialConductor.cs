using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class NoteData
{
    public float time;      // Waktu (detik) not harus dipukul
    public string type;     // Tipe not ("F" atau "J")
    public bool useCountIn; // Apakah perlu aba-aba (opsional)
}

/// <summary>
/// "Otak" dari level permainan. Mengatur timing munculnya not (spawning)
/// agar sinkron dengan musik yang sedang diputar.
/// </summary>
public class TutorialConductor : MonoBehaviour
{
    [Header("Komponen")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public GameObject notePrefabF; // Prefab not warna biru/atas
    public GameObject notePrefabJ; // Prefab not warna merah/bawah
    public Transform spawnPoint;   // Titik awal not muncul (sebelah kanan layar)

    [Header("Audio Aba-aba")]
    public AudioClip cueSoundF;
    public AudioClip cueSoundJ;

    [Header("Setting")]
    public float noteSpeed = 5f;
    public float playerXPosition = -6f; // Posisi pemain (garis finish not)
    public float musicDelay = 2f;       // Jeda sebelum musik mulai
    public float globalOffset = 0f;     // Kalibrasi manual jika audio delay
    public float cueOffset = 0.40f;     // Seberapa cepat suara aba-aba muncul sebelum not
    public float countInLeadTime = 1.5f;

    [Header("Data Lagu")]
    public List<NoteData> songNotes = new List<NoteData>(); // Daftar semua not di lagu ini
    private bool isPaused = false;
    private int nextSpawnIndex = 0; // Penunjuk not mana yang akan dispawn berikutnya
    private int nextCueIndex = 0;
    private float timeToReachPlayer;
    private bool songStarted = false;
    private bool tutorialFinished = false;

    void Start()
    {
        if (sfxSource == null) sfxSource = musicSource;

        // Memuat data not (hardcoded untuk level tutorial ini)
        SetupSongData();

        // Menghitung berapa detik waktu yang dibutuhkan not untuk jalan dari Spawn ke Player
        float distance = Mathf.Abs(spawnPoint.position.x - playerXPosition);
        timeToReachPlayer = distance / noteSpeed;
    }

    public void BeginGameplay()
    {
        // Mulai musik dengan sedikit delay agar pemain siap
        Invoke("StartMusic", musicDelay);
    }

    void StartMusic()
    {
        Debug.Log("Start Music");
        musicSource.Play();
        songStarted = true;
    }

    // Fungsi pause musik yang dipanggil GameManager
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

        // Cek apakah lagu sudah selesai
        if (!musicSource.isPlaying && musicSource.time == 0 && !tutorialFinished)
        {
            // Jika musik berhenti atau sampel audio habis
            if (musicSource.timeSamples >= musicSource.clip.samples - 100 || (!musicSource.isPlaying && songStarted))
            {
                tutorialFinished = true;
                Debug.Log("Lagu Selesai");

                // Minta GameManager tampilkan skor akhir setelah delay sedikit
                if (GameManager.instance != null)
                {
                    GameManager.instance.Invoke("ShowResults", 1f);
                }
                return;
            }
        }

        // LOGIKA SINKRONISASI: Menggunakan waktu lagu (DSP Time) sebagai patokan utama
        float songTime = musicSource.time;

        // Cek antrian spawn not
        if (nextSpawnIndex < songNotes.Count)
        {
            NoteData spawnNote = songNotes[nextSpawnIndex];

            // Rumus: Waktu Spawn = Waktu Pukul - Waktu Perjalanan Not
            float spawnTime = spawnNote.time - timeToReachPlayer;

            // Jika waktu lagu sekarang sudah melewati waktu spawn, munculkan not
            if (songTime >= spawnTime)
            {
                SpawnNote(spawnNote.type);
                nextSpawnIndex++;
            }
        }

        // Cek antrian suara aba-aba (Cue Sound) agar pemain tau ritme
        if (nextCueIndex < songNotes.Count)
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

    // Data Mapping (Hasil record manual menggunakan RhythmRecorder)
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

    // Helper untuk memasukkan data not ke list dengan rapi
    void AddNote(float recordedTime, string noteType, bool countIn = false)
    {
        NoteData newData = new NoteData();
        newData.time = recordedTime + globalOffset; // Tambah offset jika perlu
        newData.type = noteType;
        newData.useCountIn = countIn;
        songNotes.Add(newData);
    }

    // Instansiasi (Memunculkan) prefab not ke dalam game world
    void SpawnNote(string type)
    {
        GameObject prefabToSpawn = (type == "J") ? notePrefabJ : notePrefabF;
        GameObject newNote = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

        // Set kecepatan not agar sinkron
        NoteObject noteScript = newNote.GetComponent<NoteObject>();
        if (noteScript != null) noteScript.speed = noteSpeed;
    }
}