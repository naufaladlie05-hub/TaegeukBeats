using UnityEngine;

public class ExplorationMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Vector3 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        movement = new Vector3(moveX, 0f, moveZ).normalized;
    }

    void FixedUpdate()
    {
        if (movement.magnitude >= 0.1f)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.fixedDeltaTime);
        }
    }
}