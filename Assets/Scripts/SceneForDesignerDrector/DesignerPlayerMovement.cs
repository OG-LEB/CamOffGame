using UnityEngine;
using UnityEngine.SceneManagement;

public class DesignerPlayerMovement : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private PlayUIController playUIController;
    [SerializeField] private CameraFlashScript _cameraFlash;
    [SerializeField] private CameraShootingScript _cameraShootingScript;
    //private LocationScanSystem _locationScanSystem;
    private PlayerCameraRotation _cameraRotationScript;
    //[SerializeField] private Transform VokzalGuyTransform;
    //private LevelController _levelController;
    [Header("Movement Settings")]
    [SerializeField] private float CurrentSpeed;
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float SprintSpeed;
    [Space]
    //[Header("PushBack")]
    //[SerializeField] private float PushBackForce;
    //[SerializeField] private Transform CameraTransform;
    [Space]
    [Header("Gravitation")]
    [SerializeField] private float GravityForce = -9.81f;
    [SerializeField] private Transform GroundCheckPoint;
    private float VectorYVelocity = 0f;
    private float GroundCheckDistance = 0.4f;
    public LayerMask GroundCheckLayrMask;
    private bool IsGrounded;
    [SerializeField] private float JumpForce;
    [Header("Stamina")]
    [SerializeField] private bool Sprint;
    [SerializeField] private float StaminaStepValue = 0.01f;
    [SerializeField] private float StaminaRegenerationValue = 0.005f;
    [SerializeField] private float StaminaMinValueSoSprint = 0.3f;
    [SerializeField] private float StaminaValue = 1f;
    //[Header("MouseSounds")]
    //[SerializeField] private AudioSource LeftMouseSound;
    //[SerializeField] private AudioSource RightMouseSound;
    [SerializeField] private GameObject Noclip;

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
        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;

        characterController.Move(moveDirection * CurrentSpeed * Time.deltaTime);

        //Gravitation


        IsGrounded = Physics.CheckSphere(GroundCheckPoint.position, GroundCheckDistance, GroundCheckLayrMask);
        if (IsGrounded && VectorYVelocity < 0)
        {
            VectorYVelocity = -1f;
        }
        else
        {
            VectorYVelocity += GravityForce * Time.deltaTime;
            characterController.Move(new Vector3(0f, VectorYVelocity * Time.deltaTime, 0f));
        }
        //if (Input.GetKeyDown(KeyCode.Space) && IsGrounded && !LevelController.GetInstance().GetPauseState())
        //{
        //    VectorYVelocity = Mathf.Sqrt(JumpForce * -2f * GravityForce);
        //}

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
            Noclip.SetActive(true);
            gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene(0);
        }

        //if (Input.GetKeyDown(KeyCode.Mouse0) && !LevelController.GetInstance().GetPauseState())
        //{
        //    LeftMouseSound.Play();
        //    _cameraFlash.Flash();
        //    _cameraShootingScript.Shoot();
        //}
        //if (Input.GetKeyDown(KeyCode.Mouse1) && !LevelController.GetInstance().GetPauseState())
        //{
        //    RightMouseSound.Play();
        //    _locationScanSystem.Scan();
        //}

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
        playUIController.UpdateRunStaminaFillImageValue(StaminaValue);
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
