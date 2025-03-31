using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private CinemachineCamera _cinemachineCamera;
    private Transform _targetTransform;
    private float _defaultFOV;
    private float _targetFOV;
    private Coroutine _smoothFOVCoroutine;

    private void Start()
    {
        _cinemachineCamera = GetComponent<CinemachineCamera>();
        _defaultFOV = _cinemachineCamera.Lens.FieldOfView;
        _targetFOV = _defaultFOV;
        _targetTransform = _cinemachineCamera.Target.TrackingTarget;
    }

    // Instead of directly modifying localRotation, update the recoil value on the Anchor.
    public void ApplyRecoil(float recoilOffset)
    {
        // Assume recoilOffset is a float representing the pitch change (in degrees).
        // Get the Anchor component attached to the target transform.
        Anchor anchor = _targetTransform.GetComponent<Anchor>();
        if (anchor != null)
        {
            // Set the recoil offset. (You could also accumulate it if needed.)
            anchor.RecoilOffset = recoilOffset;
        }
    }

    public void RecoverRecoil(float recoverySpeed)
    {
        Anchor anchor = _targetTransform.GetComponent<Anchor>();
        if (anchor != null)
        {
            // Smoothly recover recoil by lerping the recoil offset toward 0.
            anchor.RecoilOffset = Mathf.Lerp(anchor.RecoilOffset, 0f, Time.deltaTime * recoverySpeed);
        }
    }

    public void SetFOV(float targetFOV, float transitionSpeed)
    {
        if (_smoothFOVCoroutine != null)
        {
            StopCoroutine(_smoothFOVCoroutine);
            _smoothFOVCoroutine = null;
        }
        _smoothFOVCoroutine = StartCoroutine(SmoothFOVRoutine(targetFOV, transitionSpeed));
    }
    private IEnumerator SmoothFOVRoutine(float targetFOV, float transitionSpeed)
    {
        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * transitionSpeed;
            _cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(_cinemachineCamera.Lens.FieldOfView, targetFOV, elapsed);
            yield return null;
        }
        _cinemachineCamera.Lens.FieldOfView = targetFOV;
    }
}
