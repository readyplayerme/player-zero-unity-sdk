using UnityEngine;

namespace PlayerZero.Samples
{
    /// <summary>
    /// Handles third-person character movement, camera rotation, jumping, gravity, and animation.
    /// Requires a <see cref="CharacterController"/> component.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonController : MonoBehaviour
    {
        /// <summary>
        /// Reference to the player input handler.
        /// </summary>
        [SerializeField]
        private SamplePlayerInput input;
        
        /// <summary>
        /// Movement speed in meters per second.
        /// </summary>
        [Header("Player")] 
        [SerializeField][Tooltip("Move speed of the character in m/s")]
        private float moveSpeed = 2.0f;

        /// <summary>
        /// Sprint speed in meters per second.
        /// </summary>
        [SerializeField][Tooltip("Sprint speed of the character in m/s")]
        private float sprintSpeed = 5.335f;

        /// <summary>
        /// How fast the character turns to face movement direction.
        /// </summary>
        [SerializeField][Tooltip("How fast the character turns to face movement direction")] [Range(0.0f, 0.3f)]
        private float rotationSmoothTime = 0.12f;

        /// <summary>
        /// Acceleration and deceleration rate.
        /// </summary>
        [SerializeField][Tooltip("Acceleration and deceleration")]
        private float speedChangeRate = 10.0f;
        
        /// <summary>
        /// Jump height in meters.
        /// </summary>
        [SerializeField][Space(10)] [Tooltip("The height the player can jump")]
        private float jumpHeight = 1.2f;

        /// <summary>
        /// Gravity value used for the character.
        /// </summary>
        [SerializeField][Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        private float gravity = -15.0f;

        /// <summary>
        /// Time before the player can jump again.
        /// </summary>
        [Space(10)]
        [SerializeField][Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        private float jumpTimeout = 0.50f;

        /// <summary>
        /// Time before entering the fall state.
        /// </summary>
        [SerializeField][Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        private float fallTimeout = 0.15f;

        /// <summary>
        /// Indicates if the character is grounded.
        /// </summary>
        [Header("Player Grounded")]
        [Tooltip(
            "If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        /// <summary>
        /// Offset for the grounded check sphere.
        /// </summary>
        [SerializeField][Tooltip("Useful for rough ground")] private float GroundedOffset = -0.14f;

        /// <summary>
        /// Radius for the grounded check sphere.
        /// </summary>
        [SerializeField][Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        private float groundedRadius = 0.28f;

        /// <summary>
        /// Layers considered as ground.
        /// </summary>
        [SerializeField][Tooltip("What layers the character uses as ground")]
        private LayerMask groundLayers;

        /// <summary>
        /// Camera target GameObject for rotation.
        /// </summary>
        [Header("Camera")]
        [SerializeField][Tooltip("The follow target set in the ThirdPersonCamera component that the camera will follow")]
        private GameObject cameraTarget;

        /// <summary>
        /// Maximum upward camera angle in degrees.
        /// </summary>
        [SerializeField][Tooltip("How far in degrees can you move the camera up")]
        private float topClamp = 70.0f;

        /// <summary>
        /// Maximum downward camera angle in degrees.
        /// </summary>
        [SerializeField][Tooltip("How far in degrees can you move the camera down")]
        private float bottomClamp = -30.0f;

        /// <summary>
        /// Additional degrees to override the camera. Useful for fine tuning camera position when locked.
        /// </summary>
        [SerializeField][Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        private float cameraAngleOverride = 0.0f;
        
        private float cameraTargetYaw;
        private float cameraTargetPitch;
        
        private float speed;
        private float animationBlend;
        private float targetRotation = 0.0f;
        private float rotationVelocity;
        private float verticalVelocity;
        private float terminalVelocity = 53.0f;
        
        private float jumpTimeoutDelta;
        private float fallTimeoutDelta;
        
        private int animIDSpeed;
        private int animIDGrounded;
        private int animIDJump;
        private int animIDFreeFall;
        private int animIDMotionSpeed;
        
        private Animator animator;
        private CharacterController controller;

        private GameObject mainCamera;

        private const float threshold = 0.01f;

        private bool hasAnimator;


        private void Awake()
        {
            // get a reference to our main camera
            if (mainCamera == null)
            {
                mainCamera = Camera.main.gameObject;
            }
        }

        /// <summary>
        /// Initializes components and animation IDs on start.
        /// </summary>
        private void Start()
        {
            cameraTargetYaw = cameraTarget.transform.rotation.eulerAngles.y;

            hasAnimator = TryGetComponent(out animator);
            controller = GetComponent<CharacterController>();
            AssignAnimationIDs();

            // reset our timeouts on start
            jumpTimeoutDelta = jumpTimeout;
            fallTimeoutDelta = fallTimeout;
        }

        /// <summary>
        /// Updates movement, gravity, and animation each frame.
        /// </summary>
        private void Update()
        {
            hasAnimator = TryGetComponent(out animator);

            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        /// <summary>
        /// Updates camera rotation each frame.
        /// </summary>
        private void LateUpdate()
        {
            CameraRotation();
        }

        /// <summary>
        /// Assigns animator parameter IDs.
        /// </summary>
        private void AssignAnimationIDs()
        {
            animIDSpeed = Animator.StringToHash("Speed");
            animIDGrounded = Animator.StringToHash("Grounded");
            animIDJump = Animator.StringToHash("Jump");
            animIDFreeFall = Animator.StringToHash("FreeFall");
            animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        /// <summary>
        /// Checks if the character is grounded.
        /// </summary>
        private void GroundedCheck()
        {
            // set sphere position, with offset
            var spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (hasAnimator)
            {
                animator.SetBool(animIDGrounded, Grounded);
            }
        }

        /// <summary>
        /// Handles camera rotation based on mouse input.
        /// </summary>
        private void CameraRotation()
        {
            // if there is an input and camera position is not 
            if (input.MouseLook.sqrMagnitude >= threshold)
            {
                cameraTargetYaw += input.MouseLook.x;
                cameraTargetPitch += -input.MouseLook.y;
            }

            // clamp our rotations so our values are limited 360 degrees
            cameraTargetYaw = ClampAngle(cameraTargetYaw, float.MinValue, float.MaxValue);
            cameraTargetPitch = ClampAngle(cameraTargetPitch, bottomClamp, topClamp);
            
            cameraTarget.transform.rotation = Quaternion.Euler(
                cameraTargetPitch + cameraAngleOverride,
                cameraTargetYaw, 0.0f);
        }

        /// <summary>
        /// Handles player movement and animation.
        /// </summary>
        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            var targetSpeed = input.Sprint ? sprintSpeed : moveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (input.MoveInput == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            var currentHorizontalSpeed =  new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;
  
            var speedOffset = 0.1f;
            var inputMagnitude = 1f;
            
            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * speedChangeRate);
            
                // round speed to 3 decimal places
                speed = Mathf.Round(speed * 1000f) / 1000f;
            }
            else
            {
                speed = targetSpeed;
            }
            
            animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;
            
            // normalise input direction
            Vector3 inputDirection = new Vector3(input.MoveInput.x, 0.0f, input.MoveInput.y).normalized;
            
            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (input.MoveInput != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                    rotationSmoothTime);
            
                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            var targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

            // move the player
            controller.Move(targetDirection.normalized * (speed * Time.deltaTime) +
                             new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (hasAnimator)
            {
                animator.SetFloat(animIDSpeed, animationBlend);
                animator.SetFloat(animIDMotionSpeed, inputMagnitude);
            }
        }

        /// <summary>
        /// Handles jumping and gravity.
        /// </summary>
        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                fallTimeoutDelta = fallTimeout;

                // update animator if using character
                if (hasAnimator)
                {
                    animator.SetBool(animIDJump, false);
                    animator.SetBool(animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (verticalVelocity < 0.0f)
                {
                    verticalVelocity = -2f;
                }

                // Jump
                if (input.Jump && jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                
                    // update animator if using character
                    if (hasAnimator)
                    {
                        animator.SetBool(animIDJump, true);
                    }
                }

                // jump timeout
                if (jumpTimeoutDelta >= 0.0f)
                {
                    jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                jumpTimeoutDelta = jumpTimeout;

                // fall timeout
                if (fallTimeoutDelta >= 0.0f)
                {
                    fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (hasAnimator)
                    {
                        animator.SetBool(animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                input.Jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (verticalVelocity < terminalVelocity)
            {
                verticalVelocity += gravity * Time.deltaTime;
            }
        }

        /// <summary>
        /// Clamps an angle between minimum and maximum values.
        /// </summary>
        /// <param name="lfAngle">Angle to clamp.</param>
        /// <param name="lfMin">Minimum value.</param>
        /// <param name="lfMax">Maximum value.</param>
        /// <returns>Clamped angle.</returns>
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        /// <summary>
        /// Draws gizmos for the grounded check sphere in the editor.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            var transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            var transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                groundedRadius);
        }
    }
}