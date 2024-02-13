using UnityEngine;
using UnityEngine.SceneManagement;

public class DesignerNoclipMovement : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private PlayUIController playUIController;
    //[SerializeField] private CameraFlashScript _cameraFlash;
    //[SerializeField] private CameraShootingScript _cameraShootingScript;
    //private LocationScanSystem _locationScanSystem;
    private PlayerCameraRotation _cameraRotationScript;
    //[SerializeField] private Transform VokzalGuyTransform;
    //private LevelController _levelController;
    [Header("Movement Settings")]
    [SerializeField] private float CurrentSpeed;
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float SprintSpeed;
    [SerializeField] private Transform Cameraaa;
    [Space]
    //[Header("PushBack")]
    //[SerializeField] private float PushBackForce;
    //[SerializeField] private Transform CameraTransform;
    float Yaxes = 0;
    [Header("Stamina")]
    [SerializeField] private bool Sprint;
    [SerializeField] private float StaminaStepValue = 0.01f;
    [SerializeField] private float StaminaRegenerationValue = 0.005f;
    [SerializeField] private float StaminaMinValueSoSprint = 0.3f;
    [SerializeField] private float StaminaValue = 1f;
    //[Header("MouseSounds")]
    //[SerializeField] private AudioSource LeftMouseSound;
    //[SerializeField] private AudioSource RightMouseSound;
    [SerializeField] private GameObject NormalMovement;


    private CharacterController characterController;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        _cameraRotationScript = GetComponent<PlayerCameraRotation>();
        //_locationScanSystem = LocationScanSystem.GetInstance();
        //_levelController = LevelController.GetInstance();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;

        Vector3 moveDirection = new Vector3(horizontal, Yaxes * 2, vertical);

        //Debug.Log(moveDirection);

        //characterController.Move(moveDirection * CurrentSpeed * Time.deltaTime);
        transform.Translate(moveDirection * CurrentSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Yaxes -= 1;
        }
        else if (Input.GetKeyUp(KeyCode.Q)) 
        {
            Yaxes += 1;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Yaxes += 1;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            Yaxes -= 1;
        }
        //else if (Input.GetKeyDown(KeyCode.Q) && Input.GetKeyDown(KeyCode.E))
        //{
        //    Yaxes = 0;
        //}
        else
        {
        }

        //Camera Shake
        if ((Mathf.Abs(horizontal) + Mathf.Abs(vertical)) > 0)
        {
            _cameraRotationScript.StartShake();
        }
        else
        {
            _cameraRotationScript.StopShake();
        }

        //Sprint
        if (Input.GetKeyDown(KeyCode.LeftShift) && vertical > 0 && StaminaValue >= StaminaMinValueSoSprint && (Mathf.Abs(horizontal) + Mathf.Abs(vertical)) > 0)
        {
            Sprint = true;
            _cameraRotationScript.StartRun();
            playUIController.TurnOnRunStaminaUI();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || vertical <= 0)
        {
            Sprint = false;
            _cameraRotationScript.StopRun();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("Switch");
            NormalMovement.SetActive(true);
            gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene(0);
        }

    }
    private void FixedUpdate()
    {
        if (Sprint)
        {
            CurrentSpeed = SprintSpeed;
            //StaminaValue -= StaminaStepValue;
            //if (StaminaValue <= 0f)
            //{
            //    Sprint = false;
            //    StaminaValue = 0f;
            //    _cameraRotationScript.StopRun();
            //}
        }
        if (!Sprint)
        {
            CurrentSpeed = WalkSpeed;
            //if (StaminaValue < 1f)
            //{
            //    StaminaValue += StaminaRegenerationValue;
            //}
            //else
            //{
            //    if (StaminaValue > 1f)
            //    {
            //        StaminaValue = 1f;
            //    }
            //}
        }
        //playUIController.UpdateRunStaminaFillImageValue(StaminaValue);
    }

    //public void Hit()
    //{
    //    Vector3 pushDirection = VokzalGuyTransform.right * 1 + VokzalGuyTransform.forward * PushBackForce;
    //    characterController.Move(pushDirection);
    //}
    //public void RestartStamina()
    //{
    //    StaminaValue = 1;
    //    Debug.Log("stamina");
    //}
}

