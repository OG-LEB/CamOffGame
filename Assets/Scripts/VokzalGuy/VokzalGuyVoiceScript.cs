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
    //private void OnTriggerEnter(Collider col)
    //{
    //    if (col.CompareTag("Player"))
    //    {
    //        PlayVoice();
    //        StartCoroutine(Talking());
    //    }
    //}
    //private void OnTriggerExit(Collider col)
    //{
    //    if (col.CompareTag("Player"))
    //    {
    //        StopAllCoroutines();
    //    }
    //}
    public void StartTalking() 
    {
        PlayVoice();
        StartCoroutine(Talking());
    }
    public void StopTalking() 
    {
        StopAllCoroutines();
    }
    private IEnumerator Talking() 
    {
        int seconds = Random.Range(5, 20);
        yield return new WaitForSeconds(seconds);
        PlayVoice();
        StartCoroutine(Talking());
        //Debug.Log("Voice off");
    }
}
