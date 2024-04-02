using System.Diagnostics;
using UnityEngine;

public class DawgPoster : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;
    private AudioSource source;
    private int lastSoundId = 0;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void PlayDawg() 
    {
        int soundId = Random.Range(0, sounds.Length);
        if (soundId == lastSoundId)
        {
            soundId++;
            if (soundId >= sounds.Length)
            {
                soundId = 0;
            }
        }
        source.clip = sounds[soundId];
        lastSoundId = soundId;
        source.Play();
    }
}
