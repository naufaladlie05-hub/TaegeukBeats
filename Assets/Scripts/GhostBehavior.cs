using UnityEngine;
using Spine.Unity;

public class GhostBehavior : MonoBehaviour
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

        if (meshRenderer != null) meshRenderer.sortingOrder = 5; //behind/<10

        PlayAnimation(idleAnimName, true);
    }

    public void PlayAnimation(string animName, bool loop)
    {
        if (spineAnim != null)
        {
            spineAnim.AnimationState.SetAnimation(0, animName, loop);
        }
    }

    public void OnPlayerHitMe()
    {
        if (isDead) return;
        isDead = true;

        if (meshRenderer != null) meshRenderer.sortingOrder = 5; //behind/<10

        PlayAnimation(dieAnimName, false);
        Destroy(gameObject, 1.0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDead)
        {
            isDead = true;

            if (meshRenderer != null) meshRenderer.sortingOrder = 15; //depan/>10

            if (spineAnim != null)
            {
                spineAnim.timeScale = 2.0f;
            }

            PlayAnimation(attackAnimName, false);

            PlayerController player = other.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                player.TriggerHurt();
            }

            Destroy(gameObject, 0.5f);
        }
    }
}