using UnityEngine;

/// <summary>
/// ABSTRACTION: Base class (Induk) untuk semua jenis musuh.
/// Class ini abstrak, jadi tidak bisa dipasang langsung ke object, harus diturunkan dulu.
/// </summary>
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected float hp = 10f; // Protected agar bisa diakses oleh script anak (GhostBehavior)

    /// <summary>
    /// POLYMORPHISM: Method virtual yang bisa di-override (ditimpa/diubah) perilakunya oleh script anak.
    /// </summary>
    public virtual void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Abstract method: Fungsi 'Mati' yang WAJIB dimiliki oleh setiap musuh,
    /// tapi cara matinya (animasinya) ditentukan oleh masing-masing anak.
    /// </summary>
    protected abstract void Die();
}