using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] notePrefabs; 
    public float spawnInterval = 1f; 

    float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > spawnInterval)
        {
            SpawnNote(); 
            timer = 0;   
        }
    }

    void SpawnNote()
    {
        int randomIndex = Random.Range(0, notePrefabs.Length);

        Instantiate(notePrefabs[randomIndex], transform.position, Quaternion.identity);
    }
}