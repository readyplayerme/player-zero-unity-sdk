using UnityEngine;
using UnityEngine.Serialization;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace PlayerZero.Samples
{
    namespace StarterAssets
    {
        [RequireComponent(typeof(CharacterController))]
        public class ThirdPersonController : MonoBehaviour
        {
            
            [SerializeField]
            private ThirdPersonInput input;
            
            [Header("Player")] [Tooltip("Move speed of the character in m/s")]
            public float MoveSpeed = 2.0f;

            [Tooltip("Sprint speed of the character in m/s")]
            public float SprintSpeed = 5.335f;

            [Tooltip("How fast the character turns to face movement direction")] [Range(0.0f, 0.3f)]
            public float RotationSmoothTime = 0.12f;

            [Tooltip("Acceleration and deceleration")]
            public float SpeedChangeRate = 10.0f;
            
            [Space(10)] [Tooltip("The height the player can jump")]
            public float JumpHeight = 1.2f;

            [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
            public float Gravity = -15.0f;

            [Space(10)]
            [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
            public float JumpTimeout = 0.50f;

            [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
            public float FallTimeout = 0.15f;

            [Header("Player Grounded")]
            [Tooltip(
                "If the character is grounded or not. Not part of the CharacterController built in grounded check")]
            public bool Grounded = true;

            [Tooltip("Useful for rough ground")] public float GroundedOffset = -0.14f;

            [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
            public float GroundedRadius = 0.28f;

            [Tooltip("What layers the character uses as ground")]
            public LayerMask GroundLayers;

            [Header("Camera")]
            [Tooltip("The follow target set in the ThirdPersonCamera component that the camera will follow")]
            public GameObject CameraTarget;

            [Tooltip("How far in degrees can you move the camera up")]
            public float TopClamp = 70.0f;

            [Tooltip("How far in degrees can you move the camera down")]
            public float BottomClamp = -30.0f;

            [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
            public float CameraAngleOverride = 0.0f;
            
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

            private void Start()
            {
                cameraTargetYaw = CameraTarget.transform.rotation.eulerAngles.y;

                hasAnimator = TryGetComponent(out animator);
                controller = GetComponent<CharacterController>();
                AssignAnimationIDs();

                // reset our timeouts on start
                jumpTimeoutDelta = JumpTimeout;
                fallTimeoutDelta = FallTimeout;
            }

            private void Update()
            {
                hasAnimator = TryGetComponent(out animator);

                JumpAndGravity();
                GroundedCheck();
                Move();
            }

            private void LateUpdate()
            {
                CameraRotation();
            }

            private void AssignAnimationIDs()
            {
                animIDSpeed = Animator.StringToHash("Speed");
                animIDGrounded = Animator.StringToHash("Grounded");
                animIDJump = Animator.StringToHash("Jump");
                animIDFreeFall = Animator.StringToHash("FreeFall");
                animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            }

            private void GroundedCheck()
            {
                // set sphere position, with offset
                Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                    transform.position.z);
                Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                    QueryTriggerInteraction.Ignore);

                // update animator if using character
                if (hasAnimator)
                {
                    animator.SetBool(animIDGrounded, Grounded);
                }
            }

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
                cameraTargetPitch = ClampAngle(cameraTargetPitch, BottomClamp, TopClamp);
                
                CameraTarget.transform.rotation = Quaternion.Euler(
                    cameraTargetPitch + CameraAngleOverride,
                    cameraTargetYaw, 0.0f);
            }

            private void Move()
            {
                // set target speed based on move speed, sprint speed and if sprint is pressed
                var targetSpeed = input.Sprint ? SprintSpeed : MoveSpeed;

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
                        Time.deltaTime * SpeedChangeRate);
                
                    // round speed to 3 decimal places
                    speed = Mathf.Round(speed * 1000f) / 1000f;
                }
                else
                {
                    speed = targetSpeed;
                }
                
                animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
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
                        RotationSmoothTime);
                
                    // rotate to face input direction relative to camera position
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }


                Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

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

            private void JumpAndGravity()
            {
                if (Grounded)
                {
                    // reset the fall timeout timer
                    fallTimeoutDelta = FallTimeout;

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
                        verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    
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
                    jumpTimeoutDelta = JumpTimeout;

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
                    verticalVelocity += Gravity * Time.deltaTime;
                }
            }

            private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
            {
                if (lfAngle < -360f) lfAngle += 360f;
                if (lfAngle > 360f) lfAngle -= 360f;
                return Mathf.Clamp(lfAngle, lfMin, lfMax);
            }

            private void OnDrawGizmosSelected()
            {
                Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
                Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

                if (Grounded) Gizmos.color = transparentGreen;
                else Gizmos.color = transparentRed;

                // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
                Gizmos.DrawSphere(
                    new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                    GroundedRadius);
            }
        }
    }
}