using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleporter : MonoBehaviour
{
    public string sceneToLoad; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Pindah ke scene: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}