using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKRig : MonoBehaviour
{
    private Rig _weaponIKRig;
    public Rig WeaponIKRig => _weaponIKRig;
    [SerializeField] private TwoBoneIKConstraint _rightBoneIKConstraint;
    [SerializeField] private TwoBoneIKConstraint _leftBoneIKConstraint;

    private void Awake()
    {
        _weaponIKRig = GetComponent<Rig>();
    }
    public void SetIKTargets(Transform rightIKTarget, Transform leftIKTarget)
    {
        if (_rightBoneIKConstraint != null)
        {
            _rightBoneIKConstraint.data.target = rightIKTarget;
        }
        if (_leftBoneIKConstraint != null)
        {
            _leftBoneIKConstraint.data.target = leftIKTarget;
        }

    }

}
