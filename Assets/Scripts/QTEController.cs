using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class QTEController : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject qteContainer; 
    public Image timerRing;        
    public TextMeshProUGUI keyText; 

    [Header("Settings")]
    public float qteDuration = 2.0f; 

    private bool isQTEActive = false;
    private float currentTime;
    private KeyCode targetKey; 

    void Start()
    {
        qteContainer.SetActive(false);

        Invoke("StartRandomQTE", 2.0f);
    }

    void Update()
    {
        if (isQTEActive)
        {
            currentTime -= Time.deltaTime;

            timerRing.fillAmount = currentTime / qteDuration;

            if (Input.GetKeyDown(targetKey))
            {
                Success();
            }

            if (currentTime <= 0)
            {
                Fail();
            }
        }
    }

    public void StartRandomQTE()
    {
        isQTEActive = true;
        currentTime = qteDuration;
        qteContainer.SetActive(true);

        int random = Random.Range(0, 3);
        if (random == 0) SetupKey(KeyCode.F, "F");
        else if (random == 1) SetupKey(KeyCode.J, "J");
        else SetupKey(KeyCode.Space, "SPACE");
    }

    void SetupKey(KeyCode key, string text)
    {
        targetKey = key;
        keyText.text = text;
    }

    void Success()
    {
        isQTEActive = false;
        qteContainer.SetActive(false);
        Debug.Log("QTE SUKSES! (Lanjut Cutscene/Hit Musuh)");

        Invoke("StartRandomQTE", 1.5f);
    }

    void Fail()
    {
        isQTEActive = false;
        qteContainer.SetActive(false);
        Debug.Log("QTE GAGAL! (Pemain kena damage)");

        Invoke("StartRandomQTE", 1.5f);
    }
}