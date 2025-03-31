using System.Collections;
using UnityEngine;
public class TransformComponent : BaseComponent<TransformConfig>
{
    private Coroutine _smoothTransitionCoroutine;
    private Coroutine _smoothRecoilCoroutine;
    private Transform _weaponTargetTransform;

    private Coroutine _smoothKickbackCoroutine;
    private Vector3 _kickbackOffset = Vector3.zero;
    public override void Initialize(TransformConfig config, Weapon weapon)
    {
        base.Initialize(config, weapon);


    }
    public void ApplyKickback(Vector3 targetKickbackOffset, float transitionSpeed)
    {
        if (_smoothKickbackCoroutine != null)
        {
            StopCoroutine(_smoothKickbackCoroutine);
        }
        _smoothKickbackCoroutine = StartCoroutine(SmoothKickbackRoutine(targetKickbackOffset, transitionSpeed));
    }

    // Coroutine to smoothly transition the kickback offset.
    private IEnumerator SmoothKickbackRoutine(Vector3 targetKickback, float transitionSpeed)
    {
        Vector3 startKickback = _kickbackOffset;
        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * transitionSpeed;
            _weaponTargetTransform.localPosition = Vector3.Lerp(_weaponTargetTransform.transform.localPosition, targetKickback, elapsed);
            yield return null;
        }
        _weaponTargetTransform.localPosition = targetKickback;
        _smoothKickbackCoroutine = null;
    }





    public void ApplyAimingOffsets(AimingModeData aimingModeData)
    {
        if (_smoothTransitionCoroutine != null)
        {
            StopCoroutine(_smoothTransitionCoroutine);

        }
        _smoothTransitionCoroutine = StartCoroutine(SmoothTransformRoutine(
            aimingModeData.PositionOffset,
            aimingModeData.RotationOffset,
            aimingModeData.TransitionSpeed
        ));
    }
    private IEnumerator SmoothTransformRoutine(Vector3 positionOffset, Vector3 rotationOffset, float transitionSpeed)
    {
        Vector3 startPosition = _weaponTargetTransform.localPosition;
        Quaternion startRotation = _weaponTargetTransform.localRotation;
        Vector3 baseRotationVector = new Vector3(0f, -90f, -80f);
        Quaternion targetRotation = Quaternion.Euler(baseRotationVector + rotationOffset);
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * transitionSpeed;

            _weaponTargetTransform.localPosition = Vector3.Lerp(startPosition, positionOffset, elapsed);
            _weaponTargetTransform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsed);

            yield return null;
        }

        _weaponTargetTransform.localPosition = positionOffset;
        _weaponTargetTransform.localRotation = targetRotation;

        _smoothTransitionCoroutine = null;
    }
    public void SetWeaponTargetTransform(Transform transform)
    {
      
        _weaponTargetTransform = transform;
        if (_weaponTargetTransform != null)
        {
            Debug.Log("Set weapon transform");
        }
    }

}

