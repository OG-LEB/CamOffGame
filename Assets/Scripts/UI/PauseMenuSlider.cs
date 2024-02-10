using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuSlider : MonoBehaviour
{
    [SerializeField] private PlayerCameraRotation playerCameraRotation;
    [SerializeField] private Slider SensivitySlider;
    [SerializeField] private TextMeshProUGUI SliderValueText;

    private void Start()
    {
        SensivitySlider.value = playerCameraRotation.GetCurrentSinsivity();
        SliderValueText.text = SensivitySlider.value.ToString();
    }
    public void SetSliderVale() 
    { 
        playerCameraRotation.SetCameraSensivity(SensivitySlider.value);
        SliderValueText.text = SensivitySlider.value.ToString("0.0");
    }
}
