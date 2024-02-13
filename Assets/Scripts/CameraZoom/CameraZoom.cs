using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private SoundController _soundController;
    [SerializeField] private Camera _camera;
    private float currentZoomValue;
    [SerializeField] private float MinValue;
    [SerializeField] private float MaxValue;
    [SerializeField] private float ZoomStep;
    [SerializeField] private float ZoomTime;
    private LevelController _levelController;
    private void Start()
    {
        _levelController = LevelController.GetInstance();
    }
    public void ZoomUp()
    {
        if (currentZoomValue > MinValue)
        {
            currentZoomValue -= ZoomStep;
            _soundController.PlayCameraZoomSound();
        }
        if (currentZoomValue < MinValue)
        {
            currentZoomValue = MinValue;
        }
    }
    public void ZoomDown()
    {
        if (currentZoomValue < MaxValue)
        {
            _soundController.PlayCameraZoomSound();
            currentZoomValue += ZoomStep;
        }
        if (currentZoomValue > MaxValue)
        {
            currentZoomValue = MaxValue;
        }
    }
    public void Restart()
    {
        currentZoomValue = 60;
        _camera.fieldOfView = 60;
    }
    private void FixedUpdate()
    {

        if (!_levelController.GetPauseState())
        {
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, currentZoomValue, Time.deltaTime * ZoomTime);
        }

    }
}
