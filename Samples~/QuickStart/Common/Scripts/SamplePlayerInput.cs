using UnityEngine;

namespace PlayerZero.Samples
{
    /// <summary>
    /// Handles player input for movement, mouse look, sprinting, and jumping.
    /// </summary>
    public class SamplePlayerInput : MonoBehaviour
    {
        /// <summary>
        /// The change in mouse position since the last frame, used for mouse look.
        /// </summary>
        public Vector3 MouseLook { get; private set; }
        
        /// <summary>
        /// The player's movement input as a 2D vector (horizontal and vertical axes).
        /// </summary>
        public Vector2 MoveInput { get; private set; }
        
        /// <summary>
        /// Indicates whether the jump input was pressed this frame.
        /// </summary>
        [HideInInspector]
        public bool Jump;

        /// <summary>
        /// Indicates whether the sprint input is currently active.
        /// </summary>
        public bool Sprint { get; private set; }
        
        /// <summary>
        /// The mouse position from the previous frame.
        /// </summary>
        private Vector3 previousMousePosition;
        /// <summary>
        /// Indicates whether input is currently paused.
        /// </summary>
        private bool isPaused;
        
        /// <summary>
        /// Initializes the previous mouse position on start.
        /// </summary>
        
        
        private void Start()
        {
            // Initializes the previous mouse position on start.
            previousMousePosition = Input.mousePosition;
        }
        
        /// <summary>
        /// Updates input values each frame, including mouse look, movement, sprint, and jump.
        /// </summary>
        private void Update()
        {
            if (isPaused) return;
            
            var mousePosition = Input.mousePosition;
            MouseLook = mousePosition - previousMousePosition;
            previousMousePosition = mousePosition;
            var inputX = Input.GetAxis("Horizontal");
            var inputY = Input.GetAxis("Vertical");
            MoveInput = new Vector3(inputX, inputY);
            
            // clamp for diagonal movement
            if (MoveInput.magnitude > 1f)
            {
                MoveInput.Normalize();
            }

            Sprint = Input.GetKey(KeyCode.LeftShift);
            Jump = Input.GetKeyDown(KeyCode.Space);
        }
    }
}
