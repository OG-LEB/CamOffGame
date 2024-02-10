using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayUIController : MonoBehaviour
{
    [Header("Objects Counter")]
    [SerializeField] private TextMeshProUGUI ObjectsCounterText;
    [SerializeField] private string ObjectCounterFullTextMessage;
    [Space]
    [Header("StaminaRun")]
    [SerializeField] private GameObject StaminaRunFullObject;
    [SerializeField] private Image StaminaRunFillImage;
    [Space]
    [Header("StaminaFlash")]
    [SerializeField] private GameObject StaminaFlashFullObject;
    [SerializeField] private Image StaminaFlashFillImage;

    #region StaminaRun
    public void TurnOnRunStaminaUI() { StaminaRunFullObject.SetActive(true); }
    public void UpdateRunStaminaFillImageValue(float value) 
    {
        StaminaRunFillImage.fillAmount = value;
        if (value == 1f)
        {
            StaminaRunFullObject.SetActive(false);
        }
    }
    #endregion
    #region StaminaFlash
    public void TurnOnFlashStaminaUI() { StaminaFlashFullObject.SetActive(true); }
    public void UpdateFlashStaminaFillImageValue(float value)
    {
        StaminaFlashFillImage.fillAmount = value;
        if (value == 1f)
        {
            StaminaFlashFullObject.SetActive(false);
        }
    }
    #endregion
    #region Objects Counter
    public void UpdateObjectsCounterData(int current, int FullAmount) 
    {
        ObjectsCounterText.text = $"Снято {current} из {FullAmount}";
    }
    public void UpdateObjectsCounterToFull() 
    {
        ObjectsCounterText.text = ObjectCounterFullTextMessage;
        LevelController.GetInstance().TurnOnEndGameTrigger();
    }
    #endregion
    public void Restart() 
    {
        StaminaRunFullObject.SetActive(false);
        StaminaFlashFullObject.SetActive(false);
    }
}
