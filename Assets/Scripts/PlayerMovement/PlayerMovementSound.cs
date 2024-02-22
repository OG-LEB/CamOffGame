using System.Collections;
using UnityEngine;

public class PlayerMovementSound : MonoBehaviour
{
    [SerializeField] private AudioSource StepAudioSource;
    [SerializeField] private AudioClip[] StepSounds;
    private int lastSoundId;
    private float currentSoundTime;
    [SerializeField] private float walkSoundTime;
    [SerializeField] private float sprintSoundTime;
    private bool isWalk;

    private void Start()
    {
        lastSoundId = Random.Range(0, StepSounds.Length);
        StepAudioSource.clip = StepSounds[lastSoundId];
    }
    public void UpdateWalkBool(bool value){ isWalk = value; }
    public void PlayStepSound() 
    {
        int currentId = Random.Range(0, StepSounds.Length);
        if (currentId == lastSoundId)
        {
            currentId++;
            if (currentId >= StepSounds.Length)
            {
                currentId = 0;
            }
        }
        StepAudioSource.clip = StepSounds[currentId];
        lastSoundId = currentId;
        float pitch = Random.Range(85, 115) / 100f;
        StepAudioSource.pitch = pitch;
        StepAudioSource.Play();
    }
    //public void StartSound() 
    //{
    //    StartCoroutine(MoveSounds());
    //}
    //public void StopSound() 
    //{
    //    StopAllCoroutines();
    //}
    //private IEnumerator MoveSounds() 
    //{
    //    if (isWalk)
    //    {
    //        yield return new WaitForSeconds(walkSoundTime);
    //    }
    //    else
    //    {
    //        yield return new WaitForSeconds(sprintSoundTime);
    //    }
    //    PlayStepSound();
    //    StartCoroutine(MoveSounds());
    //}
}
