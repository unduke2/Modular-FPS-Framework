using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKConstraints : MonoBehaviour
{
    [SerializeField] private TwoBoneIKConstraint _leftIKMag;
    [SerializeField] private TwoBoneIKConstraint _leftIKSlide;
    [SerializeField] private TwoBoneIKConstraint _leftIKHandle;


    public void SetIKConstraintTargets(Transform leftIKMagTarget, Transform leftIKSlideTarget, Transform LeftIKHandleTarget)
    {
        if (_leftIKMag != null)
        {
            _leftIKMag.data.target = leftIKMagTarget;
        }
        if (_leftIKSlide != null)
        {
            _leftIKSlide.data.target = leftIKSlideTarget;
        }
        if (_leftIKHandle != null)
        {
            _leftIKHandle.data.target = LeftIKHandleTarget;
        }
    }
}
