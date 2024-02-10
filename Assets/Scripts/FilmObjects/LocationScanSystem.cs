using System.Collections.Generic;
using UnityEngine;

public class LocationScanSystem : MonoBehaviour
{
    private static LocationScanSystem instance;
    public static LocationScanSystem GetInstance() { return instance; }
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private List<FilmObject> filmObjects = new List<FilmObject>();
    [SerializeField] private List<Note> Notes = new List<Note>();

    public void AddFilmObject(FilmObject obj) 
    {
        filmObjects.Add(obj);
    }
    public void RemoveFilmObject(FilmObject obj) 
    {
        filmObjects.Remove(obj);
    }
    public void AddNote(Note note)
    {
        Notes.Add(note);
    }
    public void RemoveNote(Note note)
    {
        Notes.Remove(note);
    }
    public void Scan() 
    {
        foreach (FilmObject obj in filmObjects)
        {
            obj.Scan();
        }
        foreach (Note note in Notes)
        {
            note.Scan();
        }
    }
    public void Restart() 
    {
        filmObjects.Clear();
        Notes.Clear();
    }
}
