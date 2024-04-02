using UnityEngine;

public class CameraShootingScript : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private Transform Camera;
    [SerializeField] private float ShootDistance;
    [SerializeField] private PlayUIController _PlayUIController;
    private LevelController _LevelController;
    [SerializeField] private CameraFlashScript _cameraFlash;
    [SerializeField] private LocationScanSystem _locationScanSystem;
    //Flash Stamina
    [Header("FlashStamina")]
    [SerializeField] private float StaminaValue;
    [SerializeField] private bool CanFlash;
    [SerializeField] private float FlashRegenerationSpeed;
    [Space]
    [Header("Sounds")]
    [SerializeField] private AudioSource LeftMouseSound;
    [SerializeField] private AudioSource RightMouseSound;
    private void Start()
    {
        _LevelController = LevelController.GetInstance();
    }
    private void Update()
    {
        if (!_LevelController.GetPauseState())
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                LeftMouseSound.Play();
                _cameraFlash.Flash();
                Shoot();
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                RightMouseSound.Play();
                _locationScanSystem.Scan();
            }
        }
    }
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
            if (hit.transform.CompareTag("dawg"))
            {
                hit.transform.GetComponent<DawgPoster>().PlayDawg();
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
