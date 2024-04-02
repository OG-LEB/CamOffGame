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
    public void PlayOneVoice() 
    {
        audio.clip = VoiceLines[Random.Range(0, VoiceLines.Length)];
        audio.Play();
    }

    public void StartTalking() 
    {
        PlayOneVoice();
        StartCoroutine(Talking());
        Debug.Log("Start talking");
    }
    public void StopTalking() 
    {
        StopAllCoroutines();
        Debug.Log("Stop talking");
    }
    private IEnumerator Talking() 
    {
        int seconds = Random.Range(5, 20);
        yield return new WaitForSeconds(seconds);
        PlayOneVoice();
        StartCoroutine(Talking());
    }
}
