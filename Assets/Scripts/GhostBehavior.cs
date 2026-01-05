using UnityEngine;
using Spine.Unity;

/// <summary>
/// INHERITANCE: Script ini adalah turunan (anak) dari EnemyBase.
/// Mengatur perilaku spesifik musuh hantu, termasuk animasi Spine.
/// </summary>
public class GhostBehavior : EnemyBase
{
    private SkeletonAnimation spineAnim;
    private MeshRenderer meshRenderer;
    private bool isDead = false;

    [Header("Animation Settings")]
    [SpineAnimation] public string idleAnimName = "idle";
    [SpineAnimation] public string hitAnimName = "hit";
    [SpineAnimation] public string attackAnimName = "attack";
    [SpineAnimation] public string dieAnimName = "die";

    void Start()
    {
        spineAnim = GetComponent<SkeletonAnimation>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Mengatur layer sorting agar hantu ada di belakang karakter lain saat spawn
        if (meshRenderer != null) meshRenderer.sortingOrder = 5;

        PlayAnimation(idleAnimName, true);
    }

    // Helper untuk memutar animasi Spine dengan aman
    public void PlayAnimation(string animName, bool loop)
    {
        if (spineAnim != null)
        {
            spineAnim.AnimationState.SetAnimation(0, animName, loop);
        }
    }

    /// <summary>
    /// POLYMORPHISM: Meng-override fungsi Die() milik bapaknya (EnemyBase).
    /// Disini kita tentukan animasi spesifik saat hantu mati.
    /// </summary>
    protected override void Die()
    {
        // Pastikan hantu ada di belakang saat mati
        if (GetComponent<MeshRenderer>() != null)
            GetComponent<MeshRenderer>().sortingOrder = 5;

        PlayAnimation(dieAnimName, false);
        Destroy(gameObject, 1.0f);
    }

    // Dipanggil oleh GameManager saat pemain berhasil memukul not
    public void OnPlayerHitMe()
    {
        Die(); // Langsung panggil logika mati
    }

    // Logika jika hantu menabrak pemain (Game Over / Damage)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDead)
        {
            isDead = true;

            // Pindah layer ke depan saat menyerang
            if (meshRenderer != null) meshRenderer.sortingOrder = 15;

            // Efek slow motion/freeze frame sedikit
            if (spineAnim != null)
            {
                spineAnim.timeScale = 2.0f;
            }

            PlayAnimation(attackAnimName, false);

            // Sakiti pemain
            PlayerController player = other.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                player.TriggerHurt();
            }

            Destroy(gameObject, 0.5f);
        }
    }
}