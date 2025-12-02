using UnityEngine;
using TMPro; 

public class PopupText : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float destroyTime = 1f;

    private TextMeshPro textMesh;
    private Color textColor;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        textColor = textMesh.color;
    }

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        textColor.a -= Time.deltaTime / destroyTime;
        textMesh.color = textColor;
    }

    public void Setup(string message, Color color)
    {
        textMesh.text = message;
        textMesh.color = color;
        textColor = color; 
    }
}