using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private float _pitchSensitivity = 1f;
    [SerializeField] private float _minPitch = -60f;
    [SerializeField] private float _maxPitch = 60f;

    private float _currentPitch;
    // This value will be set by the CameraController's recoil methods.
    public float RecoilOffset { get; set; } = 0f;

    private void Update()
    {
        // Combine the base pitch (from player look) with the recoil offset.
        float combinedPitch = _currentPitch + RecoilOffset;
        transform.localRotation = Quaternion.Euler(combinedPitch, 0f, 0f);
    }

    // Called from your look input handler.
    public void UpdatePitch(float lookInputY)
    {
        _currentPitch -= lookInputY * _pitchSensitivity * Time.deltaTime;
        _currentPitch = Mathf.Clamp(_currentPitch, _minPitch, _maxPitch);
    }
}
