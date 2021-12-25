using UnityEngine;

namespace SimpleIkFootPlacement
{
    public class SimpleIkFootPlacement : MonoBehaviour
    {
        [SerializeField] private Vector3 footIkOffset;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float ikSpeed;
        [SerializeField] private float ikWeightSpeed;
        [SerializeField] private float rootYMin;

        private Animator _animator;

        //lerpCache
        private float _currentWeight;
        private Vector3 _currentRootPos;

        private Vector3 _lLegCurrentPos;
        private Vector3 _rLegCurrentPos;

        private Quaternion _lLegCurrentRot;
        private Quaternion _rLegCurrentRot;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _currentRootPos = transform.localPosition;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            Transform leftFootTransf = _animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            Transform rightFootTransf = _animator.GetBoneTransform(HumanBodyBones.RightFoot);

            Vector3 leftFootPos = leftFootTransf.position;
            Vector3 rightFootPos = rightFootTransf.position;

            RaycastHit lFootHit = GetHitPoint(leftFootPos + Vector3.up, leftFootPos - Vector3.up * 5);
            RaycastHit rFootHit = GetHitPoint(rightFootPos + Vector3.up, rightFootPos - Vector3.up * 5);

            leftFootPos = lFootHit.point + footIkOffset;
            rightFootPos = rFootHit.point + footIkOffset;

            Vector3 forward = transform.forward;
            Quaternion alignFootL = Quaternion.LookRotation(forward, lFootHit.normal);
            Quaternion alignFootR = Quaternion.LookRotation(forward, rFootHit.normal);

            //vertical root Pos
            var yPosOffset = -Mathf.Abs(leftFootPos.y - rightFootPos.y);
            Vector3 targetRootPos = new Vector3(0, yPosOffset < rootYMin ? rootYMin : yPosOffset, 0);

            _currentRootPos = Vector3.Lerp(_currentRootPos, targetRootPos, Time.deltaTime * ikSpeed);

            transform.localPosition = _currentRootPos;
            SetWeights(IsIkEnabled() ? 1 : 0);

            //pos
            _lLegCurrentPos = Vector3.Lerp(_lLegCurrentPos, leftFootPos, Time.deltaTime * ikSpeed);
            _rLegCurrentPos = Vector3.Lerp(_rLegCurrentPos, rightFootPos, Time.deltaTime * ikSpeed);

            _animator.SetIKPosition(AvatarIKGoal.LeftFoot, _lLegCurrentPos);
            _animator.SetIKPosition(AvatarIKGoal.RightFoot, _rLegCurrentPos);

            //rot
            _lLegCurrentRot = Quaternion.Lerp(_lLegCurrentRot, alignFootL, Time.deltaTime * ikSpeed);
            _rLegCurrentRot = Quaternion.Lerp(_rLegCurrentRot, alignFootR, Time.deltaTime * ikSpeed);

            _animator.SetIKRotation(AvatarIKGoal.LeftFoot, _lLegCurrentRot);
            _animator.SetIKRotation(AvatarIKGoal.RightFoot, _rLegCurrentRot);
        }

        private void SetWeights(int value)
        {
            _currentWeight = Mathf.Lerp(_currentWeight, value, Time.deltaTime * ikWeightSpeed);

            //pos
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, _currentWeight);
            _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, _currentWeight);
            //rot
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, _currentWeight);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, _currentWeight);
        }

        public bool IsIkEnabled()
        {
            //Put here the code to enable and disable the ik foot placement 
            //This model of ik foot placement its supposed to only be enabled if the character is in idle state (not moving)
            return true;
        }

        private RaycastHit GetHitPoint(Vector3 start, Vector3 end)
        {
            RaycastHit hit;
            Debug.DrawLine(start, end, Color.yellow);
            return Physics.Linecast(start, end, out hit, layerMask) ? hit : hit;
        }
    }
}