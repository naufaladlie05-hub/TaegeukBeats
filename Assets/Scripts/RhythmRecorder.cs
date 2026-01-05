using UnityEngine;

/// <summary>
/// Alat bantu (Dev Tool) untuk merekam timing lagu.
/// Script ini digunakan saat development untuk mencatat detik ke berapa tombol ditekan,
/// lalu hasilnya di-copy ke TutorialConductor. Tidak dipakai saat gameplay asli.
/// </summary>
public class RhythmRecorder : MonoBehaviour
{
    public AudioSource musicSource;

    void Update()
    {
        // Tekan F saat lagu berjalan untuk mencatat waktu
        if (Input.GetKeyDown(KeyCode.F))
        {
            RecordTime("F");
        }
    }

    // Menampilkan format kodingan yang siap copas ke Console Unity
    void RecordTime(string type)
    {
        float currentTime = musicSource.time;
        Debug.Log($"AddNote({currentTime}f, \"{type}\");");
    }
}