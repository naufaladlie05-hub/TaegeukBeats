using UnityEngine;
using Spine.Unity; 

public class GhostBehavior : MonoBehaviour
{
    private SkeletonAnimation spineAnim;
    private bool isDead = false;

    void Start()
    {
        spineAnim = GetComponent<SkeletonAnimation>();
        PlayAnimation("idle", true);
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

        PlayAnimation("hit", false); 

        Destroy(gameObject, 0.5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && !isDead)
        {
            isDead = true;


            if (spineAnim != null)
            {
                spineAnim.timeScale = 2.0f; 
            }

            PlayAnimation("attack", false);


            PlayerController player = other.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                player.TriggerHurt();
            }

            Destroy(gameObject, 0.5f);
        }
    }
}
    
