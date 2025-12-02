using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public float speed = 10f;
    public bool isNoteF = true;

    private GameManager gameManager;
    private bool hasReportedMiss = false;
    private bool isHit = false; 

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (isHit) return;

        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < -7f && !hasReportedMiss)
        {
            hasReportedMiss = true;
            if (gameManager != null) gameManager.NoteMissed();
        }

        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }

    public void TriggerHit()
    {
        isHit = true;

        if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = false;

        StartCoroutine(HitAnimation());
    }

    System.Collections.IEnumerator HitAnimation()
    {
        float timer = 0;
        float duration = 0.3f; 
        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;

            transform.position = startPos + new Vector3(0, progress * 3f, 0); 

            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, progress);

            yield return null; 
        }

        Destroy(gameObject);
    }
}