using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Mengatur tampilan cerita (Slideshow) sebelum masuk ke level tutorial.
/// </summary>
public class StoryManager : MonoBehaviour
{
    [Header("Komponen UI")]
    public Image displayImage; // Tempat menampilkan gambar cerita
    public TextMeshProUGUI promptText; // Teks instruksi (Next/Start)

    [Header("Data Cerita")]
    public Sprite[] storyPages; // Kumpulan gambar halaman cerita
    public string nextSceneName = "Level_Tutorial"; // Scene tujuan setelah cerita tamat

    private int currentIndex = 0;

    void Start()
    {
        // Mulai dari halaman pertama
        if (storyPages.Length > 0)
        {
            currentIndex = 0;
            UpdateDisplay();
        }
    }

    // Dipanggil saat pemain klik tombol "Next" di layar
    public void OnClickNext()
    {
        currentIndex++;

        // Jika masih ada halaman sisa, tampilkan. Jika habis, pindah scene.
        if (currentIndex < storyPages.Length)
        {
            UpdateDisplay();
        }
        else
        {
            SkipStory();
        }
    }

    // Memperbarui gambar dan teks sesuai halaman saat ini
    void UpdateDisplay()
    {
        if (displayImage != null)
            displayImage.sprite = storyPages[currentIndex];

        if (promptText != null)
        {
            // Ubah teks jadi "START" kalau sudah di halaman terakhir
            if (currentIndex == storyPages.Length - 1)
            {
                promptText.text = " START TUTORIAL >>>";
            }
            else
            {
                promptText.text = "Click to continue...";
            }
        }
    }

    // Pindah ke scene gameplay
    public void SkipStory()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}