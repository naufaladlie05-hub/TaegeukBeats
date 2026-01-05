using UnityEngine;

/// <summary>
/// Mengatur perilaku not balok: bergerak ke kiri, deteksi jika terlewat (miss), dan animasi hancur.
/// </summary>
public class NoteObject : MonoBehaviour
{
    public float speed = 10f;
    public bool isNoteF = true; // Menentukan jenis not (Atas/Bawah)
    private bool hasReportedMiss = false;
    private bool isHit = false;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (isHit) return; // Kalau sudah kena, stop update gerakan

        // Gerakkan not ke kiri
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Cek jika not sudah melewati batas pemain (Miss)
        if (transform.position.x < -7f && !hasReportedMiss)
        {
            hasReportedMiss = true;
            if (gameManager != null) gameManager.NoteMissed();
        }

        // Hancurkan objek jika sudah jauh di luar layar
        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Fungsi untuk memicu efek visual saat not berhasil dipukul.
    /// </summary>
    public void TriggerHit()
    {
        isHit = true;
        // Matikan collider supaya tidak bisa kena hit dua kali
        if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = false;

        StartCoroutine(HitAnimation());
    }

    // Animasi not melayang ke atas dan mengecil sebelum hilang
    System.Collections.IEnumerator HitAnimation()
    {
        float timer = 0;
        float duration = 0.3f;
        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;

            transform.position = startPos + new Vector3(0, progress * 3f, 0);
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, progress);

            yield return null;
        }

        Destroy(gameObject);
    }
}