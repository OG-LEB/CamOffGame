using UnityEngine;

public class ScaryLocationSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] ScarySounds;
    private AudioSource source;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void Play() 
    {
        source.clip = ScarySounds[Random.Range(0, ScarySounds.Length)];
        source.Play();
    }
}
