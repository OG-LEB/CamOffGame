using UnityEngine;

public class VokzalGuyPushTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerMovement>().Hit();
        }
    }
}
