using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;
    [Space]
    [Header("Settings")]
    [SerializeField] private AudioSource[] AmbientSounds;
    [SerializeField] private AudioSource[] GameSounds;
    [SerializeField] private AudioSource[] PlaySoundTracks;
    [SerializeField] private bool[] GameSoundPlayingStateById;
    [SerializeField] private float GameSoundsVolume = 1;
    [SerializeField] private float MusicVolume = 1;

    [Space]
    [Header("Another Sounds")]
    [SerializeField] private AudioSource UISound;
    [SerializeField] private AudioSource CollectSound;
    [SerializeField] private AudioSource GlassBreakSound;
    [SerializeField] private AudioSource NoteSound;
    [SerializeField] private AudioSource CameraZoomSound;
    [SerializeField] private AudioSource FlashDone;
    [SerializeField] private AudioSource FenceClosing;
    [Space]
    [Header("SounTrack")]
    [SerializeField] private AudioSource MainMenuSountrack;
    [SerializeField] private AudioSource ExploreSountrack;
    [SerializeField] private AudioSource ChaseSountrack;
    [SerializeField] private AudioClip Chase_1;
    [SerializeField] private AudioClip Chase_2;
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
            //item.volume = 0.2f;
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
            //item.volume = 1f;
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
        MainMenuSountrack.Play();
    }
    public void StopMainMenuSoundTrack()
    {
        MainMenuSountrack.Stop();
    }
    public void StartExploreSound()
    {
        if (!ExploreSountrack.isPlaying)
        {
            ChaseSountrack.Stop();
            ExploreSountrack.Play();
        }
        ChaseSountrack.Pause();
    }
    public void StartChaseSound()
    {
        if (!ChaseSountrack.isPlaying)
        {
            ExploreSountrack.Stop();
            ChaseSountrack.Play();
        }
    }
    public void DisablePlaySoundTrack()
    {
        ExploreSountrack.Stop();
        ChaseSountrack.Stop();
    }
    public void EndGameCutScene()
    {
        ExploreSountrack.Stop();
        ChaseSountrack.Stop();
    }
    public void ChangeGameSoundsVolumeFromSlider(float SliderValue) 
    {
        GameSoundsVolume = SliderValue;
        mixer.SetFloat("GameSoundsVolume", Mathf.Lerp(-50, 0, GameSoundsVolume));
    }
    public void ChangerMisucVolumeFromSLider(float SliderValue) 
    {
        MusicVolume = SliderValue;
        mixer.SetFloat("MusicVolume", Mathf.Lerp(-50, 0, MusicVolume));
    }
    public float GetGameSoundsVolume() { return GameSoundsVolume; }
    public float GetMusicVolume() { return MusicVolume; }
    public void PlayFlashDoneSound() 
    {
        FlashDone.Play();
    }
    public void Restart() 
    {
        foreach (var item in PlaySoundTracks)
        {
            item.pitch = 1f;
        }
    }
    public void SetupDefaultChase() 
    {
        ChaseSountrack.clip = Chase_1;
    }
    public void SetupBossFightChase()
    {
        ChaseSountrack.clip = Chase_2;
        StartChaseSound();
    }
    public void PlayFenceClosingSound() 
    {
        FenceClosing.Play();
    }
    public void StartAmbientSounds() 
    {
        foreach (var sound in AmbientSounds)
        {
            sound.Play();
        }
    }
    public void StopAmbientSounds() 
    {
        foreach (var sound in AmbientSounds)
        {
            sound.Stop();
        }
    }
}
