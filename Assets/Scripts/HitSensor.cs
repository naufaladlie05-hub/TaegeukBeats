using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Sensor untuk mendeteksi apakah ada not yang masuk ke area pukul (Judge Line).
/// Menyimpan daftar not yang valid untuk dipukul.
/// </summary>
public class HitSensor : MonoBehaviour
{
    // List untuk menyimpan antrian not yang ada di dalam area sensor
    public List<NoteObject> notesInRange = new List<NoteObject>();

    // Jika not masuk ke trigger, tambahkan ke daftar
    void OnTriggerEnter2D(Collider2D other)
    {
        NoteObject note = other.GetComponentInParent<NoteObject>();

        if (note != null && !notesInRange.Contains(note))
        {
            notesInRange.Add(note);
        }
    }

    // Jika not keluar trigger, hapus dari daftar
    void OnTriggerExit2D(Collider2D other)
    {
        NoteObject note = other.GetComponentInParent<NoteObject>();

        if (note != null && notesInRange.Contains(note))
        {
            notesInRange.Remove(note);
        }
    }

    /// <summary>
    /// Mencari not yang valid untuk dipukul berdasarkan input pemain (F atau J).
    /// </summary>
    public NoteObject GetHittableNote(bool isTypeF)
    {
        foreach (NoteObject note in notesInRange)
        {
            // Cek apakah not aktif dan tipenya sesuai input
            if (note != null && note.gameObject.activeSelf && note.isNoteF == isTypeF)
            {
                return note;
            }
        }
        return null; // Tidak ada not yang cocok
    }
}