using System.Collections;
using UnityEngine;

public class VokzalGuyVoiceScript : MonoBehaviour
{
    private AudioSource audio;
    [SerializeField] private AudioClip[] VoiceLines;
    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    private void PlayVoice() 
    {
        audio.clip = VoiceLines[Random.Range(0, VoiceLines.Length)];
        audio.Play();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(Talking());
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            StopAllCoroutines();
        }
    }
    private IEnumerator Talking() 
    {
        int seconds = Random.Range(3, 10);
        yield return new WaitForSeconds(seconds);
        PlayVoice();
        StartCoroutine(Talking());
        //Debug.Log("Voice off");
    }
}
