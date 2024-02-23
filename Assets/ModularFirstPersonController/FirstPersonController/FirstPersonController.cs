using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonController : MonoBehaviour
{
    //private Rigidbody rb;
    private CharacterController characterController;

    private LevelController levelController;

    #region Camera Movement Variables

    public Camera playerCamera;

    public float fov = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;

    // Internal Variables
    [Space]
    [Header("MouseValues")]
    [SerializeField] private float yaw = 0.0f;
    [SerializeField] private float pitch = 0.0f;

    #endregion

    #region Movement Variables

    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;

    // Internal Variables
    private bool isWalking = false;

    #region Sprint

    public bool enableSprint = true;
    public bool unlimitedSprint = false;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public float sprintSpeed = 7f;
    [SerializeField] private float sprintStamina;
    private float sprintStaminaMax = 1f;
    [SerializeField] private float sprintStaminaMin = 0.3f;
    [SerializeField] private float sprintStaminaStep;
    [SerializeField] private float sprintStaminaRegenerationStep;
    [SerializeField] private bool CanSprint;

    private float GravityForce = -9.81f;
    private float VectorYVelocity = 0f;

    [SerializeField] private Transform VokzalGuyTransform;
    [SerializeField] private float PushBackForce;

    private bool isSprinting = false;

    private PlayerMovementSound _MovementSound;
    private bool startedWalkSound = false;
    private bool playedStepSound = false;

    #endregion

    #region Jump

    public bool enableJump = true;
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 5f;

    // Internal Variables
    private bool isGrounded = false;

    #endregion

    #region Crouch

    public bool enableCrouch = true;
    public bool holdToCrouch = true;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public float crouchHeight = .75f;
    public float speedReduction = .5f;

    // Internal Variables
    private bool isCrouched = false;
    private Vector3 originalScale;

    #endregion
    #endregion

    #region Head Bob

    public bool enableHeadBob = true;
    public Transform joint;
    public float bobSpeed = 10f;
    public Vector3 bobAmount = new Vector3(.15f, .05f, 0f);

    // Internal Variables
    private Vector3 jointOriginalPos;
    private float timer = 0;

    #endregion

    //UI
    [SerializeField] private PlayUIController PlayUI;

    public void ResetMouse()
    {
        Input.ResetInputAxes();
        yaw = 0.0f;
        pitch = 0.0f;
    }
    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();

        //crosshairObject = GetComponentInChildren<Image>();

        // Set internal variables
        playerCamera.fieldOfView = fov;
        originalScale = transform.localScale;
        jointOriginalPos = joint.localPosition;

        //if (!unlimitedSprint)
        //{
        //    sprintRemaining = sprintDuration;
        //    sprintCooldownReset = sprintCooldown;
        //}
        sprintStamina = sprintStaminaMax;
        CanSprint = true;
    }
    private void Start()
    {
        levelController = LevelController.GetInstance();
        _MovementSound = GetComponent<PlayerMovementSound>();
    }

    float camRotation;

    private void Update()
    {
        if (!levelController.GetPauseState())
        {
            cameraCanMove = true;
            playerCanMove = true;

            #region Camera

            // Control camera movement
            if (cameraCanMove)
            {
                yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;

                if (!invertCamera)
                {
                    pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
                }
                else
                {
                    // Inverted Y
                    pitch += mouseSensitivity * Input.GetAxis("Mouse Y");
                }

                // Clamp pitch between lookAngle
                pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

                transform.localEulerAngles = new Vector3(0, yaw, 0);
                playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
            }

            #endregion

            #region Sprint
            if (sprintStamina < sprintStaminaMax && !isSprinting)
            {
                sprintStamina += sprintStaminaRegenerationStep;
                PlayUI.TurnOnRunStaminaUI();
                //Debug.Log("Regeneration");
                PlayUI.UpdateRunStaminaFillImageValue(sprintStamina);
                if (sprintStamina >= sprintStaminaMin)
                {
                    CanSprint = true;
                }
                if (sprintStamina >= sprintStaminaMax)
                {
                    sprintStamina = sprintStaminaMax;
                    PlayUI.UpdateRunStaminaFillImageValue(sprintStamina);
                }
            }
            #endregion


            CheckGround();

            if (enableHeadBob)
            {
                HeadBob();
            }

            //WalkSound
            //if (isWalking)
            //{
            //    if (!startedWalkSound)
            //    {
            //        _MovementSound.StartSound();
            //        startedWalkSound = true;
            //    }

            //    if (isSprinting)
            //    {
            //        //Sprint
            //        _MovementSound.UpdateWalkBool(false);
            //    }
            //    else
            //    {
            //        //WalkSituation
            //        _MovementSound.UpdateWalkBool(true);
            //    }
            //}
            //else
            //{
            //    //Stop
            //    _MovementSound.StopSound();
            //    startedWalkSound = false;
            //}
        }
        else
        {
            cameraCanMove = false;
            playerCanMove = false;
        }
    }

    void FixedUpdate()
    {
        #region Movement

        if (playerCanMove)
        {
            // Calculate how fast we should be moving

            //Gravity
            if (isGrounded && VectorYVelocity < 0)
            {
                VectorYVelocity = -1f;
            }
            else
            {
                VectorYVelocity += GravityForce * Time.deltaTime;
            }

            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), VectorYVelocity, Input.GetAxis("Vertical"));

            // Checks if player is walking and isGrounded
            // Will allow head bob
            //if (targetVelocity.x != 0 || targetVelocity.z != 0 && isGrounded)
            if (targetVelocity.x != 0 || targetVelocity.z != 0 )
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            // All movement calculations shile sprint is active
            if (enableSprint && Input.GetKey(sprintKey) && CanSprint)
            {
                targetVelocity = transform.TransformDirection(targetVelocity) * sprintSpeed;
                //Debug.Log("Sprint");
                // Apply a force that attempts to reach our target velocity
                //Vector3 velocity = rb.velocity;
                Vector3 velocity = characterController.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                // Player is only moving when valocity change != 0
                // Makes sure fov change only happens during movement
                if (velocityChange.x != 0 || velocityChange.z != 0)
                {
                    isSprinting = true;

                    //if (hideBarWhenFull && !unlimitedSprint)
                    //{
                    //    sprintBarCG.alpha += 5 * Time.deltaTime;
                    //}
                }
                if (sprintStamina > 0)
                {
                    sprintStamina -= sprintStaminaStep;
                    //Debug.Log("sprining stamina");
                    PlayUI.TurnOnRunStaminaUI();
                    PlayUI.UpdateRunStaminaFillImageValue(sprintStamina);
                    if (sprintStamina <= 0)
                    {
                        sprintStamina = 0;
                        CanSprint = false;
                    }
                }

                //rb.AddForce(velocityChange, ForceMode.VelocityChange);
                characterController.Move(targetVelocity * Time.deltaTime);
            }
            // All movement calculations while walking
            else
            {
                isSprinting = false;

                if (sprintStamina < sprintStaminaMin)
                {
                    CanSprint = false;
                }

                targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

                // Apply a force that attempts to reach our target velocity
                //Vector3 velocity = rb.velocity;
                Vector3 velocity = characterController.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                //rb.AddForce(velocityChange, ForceMode.VelocityChange);
                characterController.Move(targetVelocity * Time.deltaTime);
            }
        }

        #endregion
    }

    // Sets isGrounded based on a raycast sent straigth down from the player object
    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = .75f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void HeadBob()
    {
        if (isWalking)
        {
            // Calculates HeadBob speed during sprint
            if (isSprinting)
            {
                timer += Time.deltaTime * (bobSpeed + sprintSpeed);
            }
            // Calculates HeadBob speed during walking
            else
            {
                timer += Time.deltaTime * bobSpeed;
            }
            // Applies HeadBob movement
            joint.localPosition = new Vector3(jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x, jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y, jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z);
            
            //Step sounds
            float val = Mathf.Sin(timer) ;
            if (!playedStepSound && val < -0.9f)
            {
                //Debug.Log("Play Sound");
                _MovementSound.PlayStepSound();
                playedStepSound = true;
            }
            else if (playedStepSound && val > -0.9f)
            {
                playedStepSound = false;
            }
        }
        else
        {
            // Resets when play stops moving
            timer = 0;
            joint.localPosition = new Vector3(Mathf.Lerp(joint.localPosition.x, jointOriginalPos.x, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.y, jointOriginalPos.y, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.z, jointOriginalPos.z, Time.deltaTime * bobSpeed));
        }
    }
    public void RestartStamina()
    {
        sprintStamina = sprintStaminaMax;
    }
    public void Hit()
    {
        Vector3 pushDirection = VokzalGuyTransform.right * 1 + VokzalGuyTransform.forward * PushBackForce;
        characterController.Move(pushDirection);
    }
}

