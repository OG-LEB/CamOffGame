using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource[] AmbientSounds;
    [SerializeField] private AudioSource[] GameSounds;
    [SerializeField] private AudioSource[] PlaySoundTracks;
    [SerializeField] private bool[] GameSoundPlayingStateById;
    [Header("Another Sounds")]
    [SerializeField] private AudioSource UISound;
    [SerializeField] private AudioSource CollectSound;
    [SerializeField] private AudioSource GlassBreakSound;
    [SerializeField] private AudioSource NoteSound;
    [SerializeField] private AudioSource CameraZoomSound;
    [Header("SounTrack")]
    [SerializeField] private AudioSource MainMenuSountrack;
    [SerializeField] private AudioSource ExploreSountrack;
    [SerializeField] private AudioSource ChaseSountrack;
    private void Start()
    {
        GameSoundPlayingStateById = new bool[GameSounds.Length];
    }
    public void PauseSounds()
    {
        foreach (var sound in AmbientSounds)
        {
            sound.Pause();
        }
        for (int i = 0; i < GameSounds.Length; i++)
        {
            if (GameSounds[i].isPlaying)
            {
                GameSounds[i].Pause();
                GameSoundPlayingStateById[i] = true;
            }
            else
            {
                GameSoundPlayingStateById[i] = false;
            }
        }
        foreach (var item in PlaySoundTracks)
        {
            item.volume = 0.1f;
            item.pitch = 0.85f;
        }
    }
    public void UnpauseSounds()
    {
        foreach (var sound in AmbientSounds)
        {
            sound.Play();
        }
        for (int i = 0; i < GameSounds.Length; i++)
        {
            if (GameSoundPlayingStateById[i])
            {
                GameSounds[i].Play();
            }
        }
        foreach (var item in PlaySoundTracks)
        {
            item.volume = 0.3f;
            item.pitch = 1f;
        }
    }
    public void PlayButtonSound()
    {
        UISound.Play();
    }
    public void PlayCollectSound()
    {
        CollectSound.Play();
    }
    public void PlayGlassBreakSound()
    {
        GlassBreakSound.Play();
    }
    public void PlayNoteSound()
    {
        NoteSound.Play();
    }
    public void PlayCameraZoomSound()
    {
        CameraZoomSound.Play();
    }
    public void PlayMainMenuSoundTrack()
    {
        //MainMenuSountrack.Play();
    }
    public void StopMainMenuSoundTrack()
    {
        //MainMenuSountrack.Stop();
    }
    public void StartExploreSound()
    {
        //if (!ExploreSountrack.isPlaying)
        //{
        //    ChaseSountrack.Pause();
        //    ExploreSountrack.Play();
        //}
        ChaseSountrack.Pause();
    }
    public void StartChaseSound()
    {
        if (!ChaseSountrack.isPlaying)
        {
            //    ExploreSountrack.Pause();
            ChaseSountrack.Play();
        }
    }
    public void DisablePlaySoundTrack()
    {
        //ExploreSountrack.Pause();
        //ChaseSountrack.Pause();
    }
    public void EndGameCutScene()
    {
        //ExploreSountrack.Stop();
        //ChaseSountrack.Stop();
    }
}
