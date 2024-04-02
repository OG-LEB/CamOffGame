using UnityEngine;
using UnityEngine.UI;

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
    //Zoom UI
    [SerializeField] private Scrollbar _zoomSlider;
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
        UpdateZoomUI();
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
        UpdateZoomUI();
    }
    public void Restart()
    {
        currentZoomValue = 60;
        _camera.fieldOfView = 60;
        UpdateZoomUI();
    }
    private void FixedUpdate()
    {

        if (!_levelController.GetPauseState())
        {
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, currentZoomValue, Time.deltaTime * ZoomTime);
        }

    }
    private void UpdateZoomUI() 
    {
        float val = currentZoomValue / 60;
        val = 1 - val;
        _zoomSlider.value = val;
    }
}
