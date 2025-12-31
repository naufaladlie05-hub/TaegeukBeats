using UnityEngine;

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
        if (Input.GetKeyDown(KeyCode.F))
        {
            myGameManager.CheckHit(true);

            if (myAnim != null) myAnim.SetTrigger("hitF");

        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            myGameManager.CheckHit(false);

            if (myAnim != null) myAnim.SetTrigger("hitJ");
        }

    }

    public void TriggerHurt()
    {
        if (myAnim != null) myAnim.SetTrigger("getHit");
    }
}