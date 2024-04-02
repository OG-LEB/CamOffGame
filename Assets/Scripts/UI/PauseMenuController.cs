using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private SoundController _soundController;
    [Space]
    [Header("UI Elements")]
    //Buttons
    [SerializeField] private GameObject ContinueBtn;
    [SerializeField] private GameObject SettingsBtn;
    [SerializeField] private GameObject RestartBtn;
    //Windows
    [SerializeField] private GameObject SettingsWindow;
    [Space]
    [Header("Settings")]
    [SerializeField] private FirstPersonController playerCameraController;
    [SerializeField] private Slider SensivitySlider;
    [SerializeField] private TextMeshProUGUI SliderValueText;

    private void Start()
    {
        SliderValueText.text = SensivitySlider.value.ToString("0.00");
    }
    //Main
    public void OpenPauseWindow() 
    {
        ContinueBtn.SetActive(true);
        SettingsBtn.SetActive(true);
        RestartBtn.SetActive(true);
        SettingsWindow.SetActive(false);
    }
    public void OpenSettingsWindow() 
    {
        ContinueBtn.SetActive(false);
        SettingsBtn.SetActive(false);
        RestartBtn.SetActive(false);
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
        SliderValueText.text = SensivitySlider.value.ToString("0.00");
    }
}
