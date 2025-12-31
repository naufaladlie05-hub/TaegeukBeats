using UnityEngine;

public class RhythmRecorder : MonoBehaviour
{
    public AudioSource musicSource;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RecordTime("F");
        }
    }

    void RecordTime(string type)
    {
        float currentTime = musicSource.time;
        Debug.Log($"AddNote({currentTime}f, \"{type}\");");
    }
}