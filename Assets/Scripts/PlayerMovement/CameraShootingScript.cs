using UnityEngine;

public class CameraShootingScript : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private Transform Camera;
    [SerializeField] private float ShootDistance;
    [SerializeField] private PlayUIController _PlayUIController;
    //Flash Stamina
    [Header("FlashStamina")]
    [SerializeField] private float StaminaValue;
    [SerializeField] private bool CanFlash;
    [SerializeField] private float FlashRegenerationSpeed;
    public void Shoot() 
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.position, Camera.forward, out hit, ShootDistance)) 
        {
            if (hit.transform.CompareTag("VokzalGuy") && CanFlash)
            {
                hit.transform.GetComponent<VokzalGuyScript>().GetShot();
                StaminaValue = 0;
                CanFlash = false;
                _PlayUIController.TurnOnFlashStaminaUI();
            }
            if (hit.transform.CompareTag("FilmObject"))
            {
                hit.transform.GetComponent<FilmObject>().Film();
            }
        }
    }
    private void FixedUpdate()
    {
        if (!CanFlash) 
        {
            StaminaValue += FlashRegenerationSpeed;
            if (StaminaValue >= 1)
            {
                CanFlash = true;
                StaminaValue = 1;
            }
            _PlayUIController.UpdateFlashStaminaFillImageValue(StaminaValue);
        }
    }
    public void Restart()
    {
        StaminaValue = 1;
        CanFlash = true;
        _PlayUIController.Restart();
    }
}
