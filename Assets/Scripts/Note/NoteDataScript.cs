using TMPro;
using UnityEngine;

public class NoteDataScript : MonoBehaviour
{
    private static NoteDataScript instance;
    public static NoteDataScript GetInstance() { return instance; }
    [SerializeField] private string[] NoteTextData;
    [SerializeField] private bool[] NoteStatesData;
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private Note[] Notes;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        NoteStatesData = new bool[NoteTextData.Length];
    }
    public void OpenNoteById(int id) 
    {
        //Text
        Text.text = NoteTextData[id];
        //Windows
        LevelController.GetInstance().OpenNote();
    }
    public int GetNoteId() 
    {
        int noteId = 0;
        for (int i = 0; i < NoteStatesData.Length; i++) 
        {
            if (NoteStatesData[i] == false)
            {
                NoteStatesData[i] = true;
                noteId = i;
                break;
            }
        }
        return noteId;
    }
    public void Restart() 
    {
        for (int i = 0; i < NoteStatesData.Length; i++)
        {
            NoteStatesData[i] = false;
        }
        foreach (var item in Notes)
        {
            item.Restart();
        }
    }
}
