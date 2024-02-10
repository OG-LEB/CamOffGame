using UnityEngine;

public class VapeStoreTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Guard"))
        {
            col.GetComponent<Guard>().zapravka();
        }
    }
}

