using UnityEngine;


namespace PlayerZero.Samples
{
    public class ThirdPersonCameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField]
        private Vector3 offset = new Vector3(0, 0, -3);
        [SerializeField]
        private bool lookAtTarget = true;
        
        private void Start()
        {
            if(target == null)
            {
                Debug.LogWarning("Target is not set for ThirdPersonCamera.");
                return;
            }
        }

        private void LateUpdate()
        {
            if (target == null) return;
            
            transform.position = target.TransformPoint(offset);
            if (lookAtTarget)
            {
                transform.LookAt(target.position);
            }
        }
    }
}