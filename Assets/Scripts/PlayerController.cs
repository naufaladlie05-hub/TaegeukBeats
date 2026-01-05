using UnityEngine;

/// <summary>
/// Menangani input tombol dari pemain (F dan J) dan mengatur animasi karakter utama.
/// </summary>
public class PlayerController : MonoBehaviour
{
    public GameManager myGameManager;
    private Animator myAnim;

    void Start()
    {
        myAnim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Input Tombol F (Hit Atas/Biru)
        if (Input.GetKeyDown(KeyCode.F))
        {
            myGameManager.CheckHit(true); // Kirim sinyal hit ke GameManager
            if (myAnim != null) myAnim.SetTrigger("hitF"); // Mainkan animasi
        }

        // Input Tombol J (Hit Bawah/Merah)
        if (Input.GetKeyDown(KeyCode.J))
        {
            myGameManager.CheckHit(false);
            if (myAnim != null) myAnim.SetTrigger("hitJ");
        }
    }

    // Dipanggil saat pemain terkena damage dari musuh
    public void TriggerHurt()
    {
        if (myAnim != null) myAnim.SetTrigger("getHit");
    }
}