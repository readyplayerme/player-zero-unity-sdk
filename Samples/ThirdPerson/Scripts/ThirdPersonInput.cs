using UnityEngine;

namespace PlayerZero.Samples
{
    public class ThirdPersonInput : MonoBehaviour
    {
        public Vector3 DeltaMousePosition { get; private set; }
        public Vector2 MoveInput { get; private set; }
        public bool Jump;
        public bool Sprint { get; private set; }
        
        [SerializeField] private Camera camera;

        private Vector3 previousMousePosition;
        private bool isPaused;
        
        private void Start()
        {

        }
        
        private void Update()
        {
            if (isPaused) return;
            
            var mousePosition = Input.mousePosition;
            DeltaMousePosition = mousePosition - previousMousePosition;
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