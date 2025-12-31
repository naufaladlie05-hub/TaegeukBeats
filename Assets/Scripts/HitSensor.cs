using UnityEngine;
using System.Collections.Generic; 

public class HitSensor : MonoBehaviour
{
    public List<NoteObject> notesInRange = new List<NoteObject>();

    void OnTriggerEnter2D(Collider2D other)
    {
        NoteObject note = other.GetComponentInParent<NoteObject>();

        if (note != null && !notesInRange.Contains(note))
        {
            notesInRange.Add(note); 
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        NoteObject note = other.GetComponentInParent<NoteObject>();

        if (note != null && notesInRange.Contains(note))
        {
            notesInRange.Remove(note); 
        }
    }

    public NoteObject GetHittableNote(bool isTypeF)
    {
        foreach (NoteObject note in notesInRange)
        {
           
            if (note != null && note.gameObject.activeSelf && note.isNoteF == isTypeF)
            {
                return note; 
            }
        }
        return null; 
    }
}