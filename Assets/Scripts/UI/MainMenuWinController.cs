using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuWinController : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private SavingSystem _SavingSystem;
    [Header("Settings")]
    [SerializeField] private SoundController _SoundController;
    [SerializeField] private GameObject ButtonsWin;
    [SerializeField] private GameObject SettingsWin;
    [SerializeField] private GameObject AutorsWin;
    [Space]
    [Header("Settings Win")]
    [SerializeField] private FirstPersonController playerCameraController;
    [SerializeField] private Slider SensivitySlider;
    [SerializeField] private Slider GameSoundsVolumeSlider;
    [SerializeField] private Slider MusicVolumeSlider;
    [SerializeField] private TextMeshProUGUI SensivityValueText;
    [SerializeField] private TextMeshProUGUI GameSoundsVolumeText;
    [SerializeField] private TextMeshProUGUI MusicVolumeText;

    private void Start()
    {
        OpenMainMenuWin();
    }
    public void OpenMainMenuWin() 
    {
        ButtonsWin.SetActive(true);
        SettingsWin.SetActive(false);
        AutorsWin.SetActive(false);
    }
    public void OpenSettingsWin() 
    {
        //UpdateSliderValue
        //Sensivity
        SensivitySlider.value = playerCameraController.GetMouseSensivity();
        SensivityValueText.text = SensivitySlider.value.ToString("0.00");
        //GameSoundsVolume
        GameSoundsVolumeSlider.value = _SoundController.GetGameSoundsVolume();
        GameSoundsVolumeText.text = GameSoundsVolumeSlider.value.ToString("0.0");
        //MusicVolume
        MusicVolumeSlider.value = _SoundController.GetMusicVolume();
        MusicVolumeText.text = MusicVolumeSlider.value.ToString("0.0");

        ButtonsWin.SetActive(false);
        SettingsWin.SetActive(true);
        _SoundController.PlayButtonSound();
    }
    public void CloseSettingsWin() 
    {
        ButtonsWin.SetActive(true);
        SettingsWin.SetActive(false);
        _SoundController.PlayButtonSound();
    }
    public void OpenAutorsWin() 
    {
        ButtonsWin.SetActive(false);
        SettingsWin.SetActive(false);
        AutorsWin.SetActive(true);
        _SoundController.PlayButtonSound();
    }
    public void CloseAutorsWin() 
    {
        ButtonsWin.SetActive(true);
        SettingsWin.SetActive(false);
        AutorsWin.SetActive(false);
        _SoundController.PlayButtonSound();
    }
    public void SetSensivityFromSLider()
    {
        playerCameraController.SetCameraSensivity(SensivitySlider.value);
        SensivityValueText.text = SensivitySlider.value.ToString("0.00");
        _SavingSystem.SaveMouseSensitivity(SensivitySlider.value);
    }
    public void SetGameSoundsVolumeFromSlider()
    {
        _SoundController.ChangeGameSoundsVolumeFromSlider(GameSoundsVolumeSlider.value);
        GameSoundsVolumeText.text = GameSoundsVolumeSlider.value.ToString("0.0");
        _SavingSystem.SaveGameSoundVolume(GameSoundsVolumeSlider.value);
    }
    public void SetMusicVolumeFromSlider()
    {
        _SoundController.ChangerMisucVolumeFromSLider(MusicVolumeSlider.value);
        MusicVolumeText.text = MusicVolumeSlider.value.ToString("0.0");
        _SavingSystem.SaveMusicSoundVolume(MusicVolumeSlider.value);
    }

}
