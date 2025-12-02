using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager myGameManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            myGameManager.CheckHit(true);

            transform.position = new Vector3(-5.5f, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            myGameManager.CheckHit(false);

            transform.position = new Vector3(-5.5f, 0, 0);
        }

        if (Input.GetKeyUp(KeyCode.F) || Input.GetKeyUp(KeyCode.J))
        {
            transform.position = new Vector3(-6f, 0, 0);
        }
    }
}