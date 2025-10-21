using UnityEngine;


namespace PlayerZero.Samples
{
    /// <summary>
    /// Follows a target transform with a configurable offset and optionally looks at the target.
    /// Used for third-person camera control.
    /// </summary>
    public class ThirdPersonCameraFollow : MonoBehaviour
    {
        /// <summary>
        /// The target transform to follow (typically the player).
        /// </summary>
        [SerializeField] private Transform target;
        
        /// <summary>
        /// The positional offset from the target in local space.
        /// </summary>
        [SerializeField]
        private Vector3 offset = new Vector3(0, 0, -3);
        
        /// <summary>
        /// Whether the camera should look at the target each frame.
        /// </summary>
        [SerializeField]
        private bool lookAtTarget = true;
        
        private void Start()
        {
            // Checks if the target is set at startup and logs a warning if not.
            if(target == null)
            {
                Debug.LogWarning("Target is not set for ThirdPersonCamera.");
                return;
            }
        }

        /// <summary>
        /// Updates the camera's position and rotation to follow the target in LateUpdate.
        /// </summary>
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