using UnityEngine;
using TMPro;

/// <summary>
/// Mengatur efek visual teks yang muncul saat pemain memukul not (misal: "Perfect!", "Miss!").
/// Teks akan bergerak ke atas lalu menghilang perlahan (Fade Out).
/// </summary>
public class PopupText : MonoBehaviour
{
    public TextMeshPro textMesh;
    public float moveSpeed = 3f;   // Kecepatan gerak ke atas
    public float destroyTime = 1f; // Umur objek sebelum dihancurkan
    public float fadeSpeed = 2f;   // Kecepatan menghilang

    private Color textColor;

    private void Awake()
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshPro>();
    }

    // Inisialisasi teks dan warna awal
    public void Setup(string text, Color color)
    {
        if (textMesh != null)
        {
            textMesh.text = text;

            textColor = color;
            textColor.a = 1f; // Alpha 1 artinya terlihat jelas (tidak transparan)

            textMesh.color = textColor;
        }
    }

    void Update()
    {
        // Gerakkan teks ke atas setiap frame
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        if (textMesh != null)
        {
            // Kurangi alpha pelan-pelan (efek fade out)
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = textColor;

            // Hancurkan objek jika sudah benar-benar transparan
            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}