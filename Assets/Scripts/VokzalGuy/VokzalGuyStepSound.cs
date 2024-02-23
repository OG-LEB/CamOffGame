using UnityEngine;

public class VokzalGuyStepSound : MonoBehaviour
{
    [SerializeField] private AudioSource StepAudioSource;
    [SerializeField] private AudioClip[] StepSounds;
    private int lastSoundId;
    public void PlaySound() 
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
}
