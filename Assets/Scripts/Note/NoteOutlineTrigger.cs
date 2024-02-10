using UnityEngine;

public class NoteOutlineTrigger : MonoBehaviour
{
    [SerializeField] private Note MyNote;
    private LocationScanSystem locationScanSystem;
    private void Start()
    {
        locationScanSystem = LocationScanSystem.GetInstance();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            locationScanSystem.AddNote(MyNote);
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            locationScanSystem.RemoveNote(MyNote);
        }
    }

}
