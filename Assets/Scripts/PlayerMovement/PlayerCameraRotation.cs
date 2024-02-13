using UnityEngine;

public class PlayerCameraRotation : MonoBehaviour
{
    //Links
    private PlayerMovementSound _sound;
    private LevelController _levelController;
    [Header("Settings")]
    [SerializeField] private float Sensitivity = 250f;
    [SerializeField] private Transform cameraTransform;
    float yRotation = 0f;
    [Header("Walk Shake settings")]
    [SerializeField] private bool cameraShake;
    [SerializeField] private float currentShakeValue;
    [SerializeField] private float currentShakeCornerAngle;
    [SerializeField] private float WalkShakeCornerAngle;
    [SerializeField] private float RunShakeCornerAngle;
    [SerializeField] private float currentShakeValueStep;
    [SerializeField] private float WalkShakeValueStep;
    [SerializeField] private float RunShakeValueStep;
    [SerializeField] private float ToIdleStepValue;
    private bool shakeValueGoesUp = true;
    private void Start()
    {
        StopRun();
        _sound = GetComponent<PlayerMovementSound>();
        _levelController = LevelController.GetInstance();
    }
    private void Update()
    {

        if (!_levelController.GetPauseState())
        {
            float valueX = Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
            float valueY = Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;
            //Debug.Log($"{valueX}, {valueY}");
            transform.Rotate(Vector3.up * valueX);

            yRotation -= valueY;
            yRotation = Mathf.Clamp(yRotation, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(yRotation * 1f, 0f, currentShakeValue);
        }
    }
    private void FixedUpdate()
    {
        CameraWalkShake();
    }
    public void SetCameraSensivity(float value)
    {
        Sensitivity = value;
    }
    public float GetCurrentSinsivity() { return Sensitivity; }
    private void CameraWalkShake()
    {
        if (cameraShake)
        {
            if (shakeValueGoesUp)
            {
                currentShakeValue += currentShakeValueStep;
                if (currentShakeValue >= currentShakeCornerAngle)
                {
                    shakeValueGoesUp = false;
                    _sound.PlayStepSound();
                }
            }
            else
            {
                currentShakeValue -= currentShakeValueStep;
                if (currentShakeValue <= -currentShakeCornerAngle)
                {
                    shakeValueGoesUp = true;
                    _sound.PlayStepSound();
                }
            }
        }
        else
        {
            if (currentShakeValue > 0)
            {
                currentShakeValue -= ToIdleStepValue;
            }
            else if (currentShakeValue < 0)
            {
                currentShakeValue += ToIdleStepValue;
            }
            if (currentShakeValue > -0.05f && currentShakeValue < 0.05f)
            {
                currentShakeValue = 0;
            }
        }
    }
    public void StartShake() { cameraShake = true; }
    public void StopShake() { cameraShake = false; }
    public void StartRun()
    {
        currentShakeCornerAngle = RunShakeCornerAngle;
        currentShakeValueStep = RunShakeValueStep;
    }
    public void StopRun()
    {
        currentShakeCornerAngle = WalkShakeCornerAngle;
        currentShakeValueStep = WalkShakeValueStep;
    }
    public void ResetAxis()
    {
        Input.ResetInputAxes();
        yRotation = 0f;
    }
}
