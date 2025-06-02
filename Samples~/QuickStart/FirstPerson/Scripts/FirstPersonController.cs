using UnityEngine;

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
	public class FirstPersonController : MonoBehaviour
	{
		[SerializeField] private FirstPersonInput input;
		
		[Header("Player")]
		[SerializeField][Tooltip("Move speed of the character in m/s")]
		private float moveSpeed = 4.0f;
		[SerializeField][Tooltip("Sprint speed of the character in m/s")]
		private float sprintSpeed = 6.0f;
		[SerializeField][Tooltip("Acceleration and deceleration")]
		private float speedChangeRate = 10.0f;

		[Space(10)]
		[SerializeField][Tooltip("The height the player can jump")]
		private float jumpHeight = 1.2f;
		[SerializeField][Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		private float gravity = -15.0f;

		[Space(10)]
		[SerializeField][Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		private float jumpTimeout = 0.1f;
		[SerializeField][Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		private float fallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[SerializeField][Tooltip("Useful for rough ground")]
		private float groundedOffset = -0.14f;
		[SerializeField][Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		private float groundedRadius = 0.5f;
		[SerializeField][Tooltip("What layers the character uses as ground")]
		private LayerMask groundLayers;

		[Header("Camera")]
		[SerializeField][Tooltip("The follow target set in the Camera that the camera will follow")]
		private GameObject cameraTarget;
		[SerializeField][Tooltip("How far in degrees can you move the camera up")]
		private float topClamp = 70.0f;
		[SerializeField][Tooltip("How far in degrees can you move the camera down")]
		private float bottomClamp = -70.0f;
		[SerializeField][Tooltip("Mouse sensitivity multiplier")]
		private float lookSensitivity = 1.0f;
		[SerializeField][Tooltip("Time it takes to reach the target rotation")]
		private float lookSmoothTime = 0.01f;
		
		private Vector2 currentMouseDelta;
		private Vector2 currentMouseDeltaVelocity;

		// cinemachine
		private float cameraTargetPitch;

		// player
		private float speed;
		private float rotationVelocity;
		private float verticalVelocity;
		private float terminalVelocity = 53.0f;

		// timeout deltatime
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
		private float animationBlend;
		private bool hasAnimator;

		private void Awake()
		{
			// get a reference to our main camera
			if (mainCamera == null)
			{
				mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			controller = GetComponent<CharacterController>();
			hasAnimator = TryGetComponent(out animator);
			AssignAnimationIDs();
			
			// reset our timeouts on start
			jumpTimeoutDelta = jumpTimeout;
			fallTimeoutDelta = fallTimeout;
		}
		
		private void AssignAnimationIDs()
		{
			animIDSpeed = Animator.StringToHash("Speed");
			animIDGrounded = Animator.StringToHash("Grounded");
			animIDJump = Animator.StringToHash("Jump");
			animIDFreeFall = Animator.StringToHash("FreeFall");
			animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
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

		private void GroundedCheck()
		{
			// set sphere position, with offset
			var spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
			
			if (hasAnimator)
			{
				animator.SetBool(animIDGrounded, Grounded);
			}
		}

		private void CameraRotation()
		{
			if (input.MouseLook.sqrMagnitude >= threshold)
			{
				// Smooth the mouse input
				currentMouseDelta = Vector2.SmoothDamp(
					currentMouseDelta, 
					input.MouseLook, 
					ref currentMouseDeltaVelocity, 
					lookSmoothTime
				);

				rotationVelocity = currentMouseDelta.x * lookSensitivity;
				cameraTargetPitch += -currentMouseDelta.y * lookSensitivity;

				cameraTargetPitch = ClampAngle(cameraTargetPitch, bottomClamp, topClamp);

				cameraTarget.transform.localRotation = Quaternion.Euler(cameraTargetPitch, 0.0f, 0.0f);
				transform.Rotate(Vector3.up * rotationVelocity);
			}
		}


		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			var targetSpeed = input.Sprint ? sprintSpeed : moveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (input.MoveInput == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			var currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

			var speedOffset = 0.1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * speedChangeRate);

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
				// move
				inputDirection = transform.right * input.MoveInput.x + transform.forward * input.MoveInput.y;
			}

			// move the player
			controller.Move(inputDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
			
			// update animator if using character
			if (hasAnimator)
			{
				animator.SetFloat(animIDSpeed, animationBlend);
				animator.SetFloat(animIDMotionSpeed, 1);
			}
		}

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

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			var transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			var transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundedRadius);
		}
	}
}