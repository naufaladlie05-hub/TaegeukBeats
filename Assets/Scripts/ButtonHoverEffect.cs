using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Memberikan efek membesar (scaling) pada tombol saat kursor mouse diarahkan (Hover).
/// </summary>
public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI textTarget;
    public float scaleSize = 1.2f;
    public float speed = 15f;

    private Vector3 originalScale;
    private Vector3 targetScale;

    void Start()
    {
        if (textTarget != null)
        {
            originalScale = textTarget.transform.localScale;
            targetScale = originalScale;
        }
    }

    void Update()
    {
        if (textTarget != null)
        {
            textTarget.transform.localScale = Vector3.Lerp(textTarget.transform.localScale, targetScale, Time.deltaTime * speed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * scaleSize;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
}