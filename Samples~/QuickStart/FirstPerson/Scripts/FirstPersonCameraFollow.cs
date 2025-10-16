using UnityEngine;

namespace PlayerZero.Samples
{
    /// <summary>
    /// Follows a target transform, matching its position and rotation each frame for first-person camera control.
    /// </summary>
    public class FirstPersonCameraFollow : MonoBehaviour
    {
        /// <summary>
        /// The target transform to follow (typically the player).
        /// </summary>
        [SerializeField] private Transform target;


        private void Start()
        {
            // Checks if the target is set at startup and logs a warning if not.
            if (target == null)
            {
                Debug.LogWarning("Target is not set for ThirdPersonCamera.");
                return;
            }
        }

        /// <summary>
        /// Updates the camera's position and rotation to match the target in LateUpdate.
        /// </summary>
        private void LateUpdate()
        {
            if (target == null) return;

            transform.position = target.position;
            transform.rotation = target.rotation;

        }
    }
}
