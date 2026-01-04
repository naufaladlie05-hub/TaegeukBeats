using UnityEngine;
using TMPro;

public class PopupText : MonoBehaviour
{
    public TextMeshPro textMesh;
    public float moveSpeed = 3f;
    public float destroyTime = 1f; 
    public float fadeSpeed = 2f;   

    private Color textColor;

    private void Awake()
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshPro>();
    }


    public void Setup(string text, Color color)
    {
        if (textMesh != null)
        {
            textMesh.text = text;

            textColor = color;
            textColor.a = 1f;

            textMesh.color = textColor;
        }
    }


    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        if (textMesh != null)
        {
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}