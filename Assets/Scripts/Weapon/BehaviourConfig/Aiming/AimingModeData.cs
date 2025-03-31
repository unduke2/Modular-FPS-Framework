using UnityEngine;
[CreateAssetMenu]
public class AimingModeData : ScriptableObject
{

    public AimingMode AimingMode;
    public Vector3 PositionOffset;
    public Vector3 RotationOffset;
    public float FieldOfView;
    public float SensitivityMultiplier;
    public float TransitionSpeed;
    public float MovementSpeedMultiplier;
    public float SpreadMultiplier;
   
}
