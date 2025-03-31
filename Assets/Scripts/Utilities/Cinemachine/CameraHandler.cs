using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour {

    private CinemachineCamera _cinemachineCamera;
    private Coroutine _smoothFovCoroutine;
    public float bobFrequency = 2.0f;   // How fast the bobbing happens
    public float bobHeight = 0.05f;     // The vertical range of the bobbing
    public float swayAmount = 0.05f;    // Horizontal sway range
    private Vector3 originalPosition;
    private CinemachineBasicMultiChannelPerlin m_multiChannelPerlin;

    void Start()
    {
        _cinemachineCamera = GetComponent<CinemachineCamera>();
        //m_multiChannelPerlin = _cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        // Store the original camera position
        if (_cinemachineCamera != null)
        {

            originalPosition = _cinemachineCamera.transform.localPosition;
        }
        EventManager.OnUpdateCamera += UpdateFOV;
        //EventManager.OnCameraNoiseUpdate += UpdateNoise;
        EventManager.OnCameraRotationUpdate += UpdateCameraRotation;
    }

    void Update()
    {
        //// Apply camera bobbing when the player is moving (based on a sine wave)
        //float bobOffset = Mathf.Sin(Time.time * bobFrequency) * bobHeight;
        //float swayOffset = Mathf.Cos(Time.time * bobFrequency * 0.5f) * swayAmount;

        //// Adjust the camera's local position for head bobbing and sway
        //_cinemachineCamera.transform.localPosition = originalPosition + new Vector3(swayOffset, bobOffset, 0);
    }
    public void UpdateFOV(float fov, float transitionSpeed)
    {
        if (_smoothFovCoroutine != null)
        {
            StopCoroutine( _smoothFovCoroutine );
        } 
        _smoothFovCoroutine = StartCoroutine(SmoothFOV(fov, transitionSpeed)); 
    }

    private IEnumerator SmoothFOV(float fov, float transitionSpeed) 
    {
        while (Mathf.Abs(_cinemachineCamera.Lens.FieldOfView - fov) > Mathf.Epsilon)
        {
            _cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(_cinemachineCamera.Lens.FieldOfView, fov, transitionSpeed * Time.deltaTime);

            yield return null;
        }

        _cinemachineCamera.Lens.FieldOfView = fov;
        _smoothFovCoroutine = null;

    }


    public void UpdateCameraRotation(Quaternion rotation)
    {
        _cinemachineCamera.transform.localRotation = rotation;
    }


    //public void UpdateNoise(Vector2 gains)
    //{
    //    m_multiChannelPerlin.FrequencyGain = gains.x;
    //    m_multiChannelPerlin.AmplitudeGain = gains.y;
    //}
    //private void OnDestroy()
    //{
    //    EventManager.OnUpdateCamera -= UpdateFOV;
    //    EventManager.OnCameraNoiseUpdate -= UpdateNoise;

    //}

    private void OnDestroy()
    {
        EventManager.OnCameraRotationUpdate -= UpdateCameraRotation;
    }
}
