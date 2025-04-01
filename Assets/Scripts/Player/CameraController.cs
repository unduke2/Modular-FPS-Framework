using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineCamera _cinemachineCamera;
    private Transform _targetTransform;

    private float _defaultFOV;
    private float _targetFOV;

    private CameraTarget _cameraTarget;

    private Coroutine _smoothFOVCoroutine;


    private void Awake()
    {
        _cinemachineCamera = GetComponent<CinemachineCamera>();
    }


    private void Start()
    {
        if (_cinemachineCamera == null)
        {
            Debug.LogError($"{nameof(CameraController)}: No CinemachineCamera found on this GameObject");
        }

        _defaultFOV = _cinemachineCamera.Lens.FieldOfView;
        _targetFOV = _defaultFOV;

        _targetTransform = _cinemachineCamera.Target.TrackingTarget;
        _cameraTarget = _targetTransform.GetComponent<CameraTarget>();
    }


    public void ApplyRecoil(float recoilOffset)
    {
        if (_cameraTarget != null)
        {
            _cameraTarget.RecoilOffset = recoilOffset;
        }
        else
        {
            Debug.LogWarning($"{nameof(CameraController)}: No CameraTarget found on tracking target for recoil");
        }
    }


    public void RecoverRecoil(float recoverySpeed)
    {
        if (_cameraTarget != null)
        {
            _cameraTarget.RecoilOffset = Mathf.Lerp(_cameraTarget.RecoilOffset,
                0f,
                Time.deltaTime * recoverySpeed
                );
        }
    }


    public void SetFOV(float targetFOV, float transitionSpeed)
    {
        if (_smoothFOVCoroutine != null)
        {
            StopCoroutine(_smoothFOVCoroutine);
        }

        _smoothFOVCoroutine = StartCoroutine(SmoothFOVRoutine(targetFOV, transitionSpeed));
    }


    private IEnumerator SmoothFOVRoutine(float targetFOV, float transitionSpeed)
    {
        if (_cinemachineCamera == null)
        {
            yield break;
        }

        float startFOV = _cinemachineCamera.Lens.FieldOfView;
        float timeElapsed = 0f;

        while (timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime * transitionSpeed;

            _cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(startFOV,
                targetFOV,
                timeElapsed);

            yield return null;
        }
        _cinemachineCamera.Lens.FieldOfView = targetFOV;
        _smoothFOVCoroutine = null;
    }
}
