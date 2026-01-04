using UnityEngine;
using UnityEngine.EventSystems; 

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Setting")] 
    public float hoverScale = 1.2f; 
    public float speed = 15f; 

    private Vector3 originalScale;  
    private Vector3 targetScale;    

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * speed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale; 
    }

    void OnDisable()
    {
        transform.localScale = originalScale;
        targetScale = originalScale;
    }
}