using System.Collections;
using UnityEngine;

public class VokzalGuyVoiceScript : MonoBehaviour
{
    private AudioSource audio;
    [SerializeField] private AudioClip[] ChasingVoiceLines;
    [SerializeField] private AudioClip[] HitVoiceLines;
    [SerializeField] private AudioClip[] FoundVoiceLines;
    [SerializeField] private AudioClip[] LostVoiceLines;
    [SerializeField] private AudioClip[] GetShotVoiceLines;
    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    public void PlayChasingVoice() 
    {
        audio.clip = ChasingVoiceLines[Random.Range(0, ChasingVoiceLines.Length)];
        audio.Play();
        //Debug.Log("Chasing voice");
    }
    public void StartChaseTalking() 
    {
        //PlayChasingVoice();
        StartCoroutine(ChaseTalking());
        //Debug.Log("Start talking");
    }
    public void StopChaseTalking() 
    {
        StopAllCoroutines();
        //Debug.Log("Stop talking");
    }
    private IEnumerator ChaseTalking() 
    {
        int seconds = Random.Range(5, 20);
        yield return new WaitForSeconds(seconds);
        PlayChasingVoice();
        StartCoroutine(ChaseTalking());
    }
    public void PlayHitTalk() 
    {
        audio.clip = HitVoiceLines[Random.Range(0, HitVoiceLines.Length)];
        audio.Play();
        //Debug.Log("Hit voice");

    }
    public void PlayFoundTalk() 
    {
        audio.clip = FoundVoiceLines[Random.Range(0, FoundVoiceLines.Length)];
        audio.Play();
        //Debug.Log("Found voice");

    }
    public void PlayLostTalk()
    {
        audio.clip = LostVoiceLines[Random.Range(0, LostVoiceLines.Length)];
        audio.Play();
        //Debug.Log("Lost voice");
    }
    public void PlayGetShotTalk()
    {
        audio.clip = GetShotVoiceLines[Random.Range(0, GetShotVoiceLines.Length)];
        audio.Play();
        //Debug.Log("Lost voice");
    }
}
