using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private SoundController _soundController;
    [SerializeField] private SavingSystem _SavingSystem;
    [Space]
    [Header("UI Elements")]
    //Windows
    [SerializeField] private GameObject MainWindow;
    [SerializeField] private GameObject SettingsWindow;
    [Space]
    [Header("Settings")]
    [SerializeField] private FirstPersonController playerCameraController;
    [SerializeField] private Slider SensivitySlider;
    [SerializeField] private Slider GameSoundsVolumeSlider;
    [SerializeField] private Slider MusicVolumeSlider;
    [SerializeField] private TextMeshProUGUI SensivityValueText;
    [SerializeField] private TextMeshProUGUI GameSoundsVolumeText;
    [SerializeField] private TextMeshProUGUI MusicVolumeText;
    private void Start()
    {
        SensivityValueText.text = SensivitySlider.value.ToString("0.00");
        GameSoundsVolumeText.text = GameSoundsVolumeSlider.value.ToString("0.0");
        MusicVolumeText.text = MusicVolumeSlider.value.ToString("0.0");
    }
    //Main
    public void OpenPauseWindow()
    {
        MainWindow.SetActive(true);
        SettingsWindow.SetActive(false);
    }
    public void OpenSettingsWindow()
    {
        //UpdateSliderValue
        //Sensivity
        SensivitySlider.value = playerCameraController.GetMouseSensivity();
        SensivityValueText.text = SensivitySlider.value.ToString("0.00");
        //GameSoundsVolume
        GameSoundsVolumeSlider.value = _soundController.GetGameSoundsVolume();
        GameSoundsVolumeText.text = GameSoundsVolumeSlider.value.ToString("0.0");
        //MusicVolume 
        MusicVolumeSlider.value = _soundController.GetMusicVolume();
        MusicVolumeText.text = MusicVolumeSlider.value.ToString("0.0");

        MainWindow.SetActive(false);
        SettingsWindow.SetActive(true);
        _soundController.PlayButtonSound();
        
    }
    public void CloseSettingsWindow()
    {
        OpenPauseWindow();
        _soundController.PlayButtonSound();
    }
    //Settings
    public void SetSensivityFromSLider()
    {
        playerCameraController.SetCameraSensivity(SensivitySlider.value);
        SensivityValueText.text = SensivitySlider.value.ToString("0.00");
        _SavingSystem.SaveMouseSensitivity(SensivitySlider.value);
    }
    public void SetGameSoundsVolumeFromSlider()
    {
        _soundController.ChangeGameSoundsVolumeFromSlider(GameSoundsVolumeSlider.value);
        GameSoundsVolumeText.text = GameSoundsVolumeSlider.value.ToString("0.0");
        _SavingSystem.SaveGameSoundVolume(GameSoundsVolumeSlider.value);
    }
    public void SetMusicVolumeFromSlider() 
    {
        _soundController.ChangerMisucVolumeFromSLider(MusicVolumeSlider.value);
        MusicVolumeText.text = MusicVolumeSlider.value.ToString("0.0");
        _SavingSystem.SaveMusicSoundVolume(MusicVolumeSlider.value);
    }
}
