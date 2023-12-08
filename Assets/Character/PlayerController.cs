using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Walker2.Manager;

namespace Walker2.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField]
        private float AnimationBlendSpeed = 8.9f;

        [Header("Camera")]
        [SerializeField]
        private float mouseSens = 25;

        [SerializeField]
        private Transform cameraRoot;

        [SerializeField]
        private Transform camera;

        [SerializeField]
        private float upperLimit = -40;

        [SerializeField]
        private float bottomLimit = 70;

        [Header("Movement")]
        [SerializeField]
        private float jumpFactor = 150;

        [SerializeField]
        private float distanceToGround = 0.8f;

        [SerializeField]
        private LayerMask groundLayer;       

        private Rigidbody rb;

        private InputManager inputManager;

        private Animator animator;
        private bool hasAnimator;

        private int xVelocityHash;
        private int yVelocityHash;
        private int jumpHash;
        private int groundHash;
        private int fallingHash;
        private int crouchHash;

        private bool isReadyToJump = true;

        private float xRotation;

        private const float crouchSpeed = 2;
        private const float walkSpeed = 4;
        private const float runSpeed = 6;

        private bool grounded = true;

        private Vector2 currentVelocity;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            inputManager = GetComponent<InputManager>();
            hasAnimator = TryGetComponent<Animator>(out animator);

            xVelocityHash = Animator.StringToHash("XVelocity");
            yVelocityHash = Animator.StringToHash("YVelocity");
            jumpHash = Animator.StringToHash("Jump");
            fallingHash = Animator.StringToHash("Falling");
            groundHash = Animator.StringToHash("Grounded");
            crouchHash = Animator.StringToHash("Crouch");
        }

        private void FixedUpdate()
        {
            SampleGround();
            Move();
            Jump();
            Crouch();
        }

        private void LateUpdate()
        {
            CameraMovement();
        }

        private void Move()
        {
            if (!hasAnimator)
            {
                return;
            }

            float targetSpeed = 0;

            if (inputManager.Run)
            {
                targetSpeed = runSpeed;
            }
            else
            {
                targetSpeed = walkSpeed;
            }

            if (inputManager.Crouch)
            {
                targetSpeed = crouchSpeed;
            }           

            if (inputManager.Move == Vector2.zero)
            {
                targetSpeed = 0f;
            }

            currentVelocity.x = Mathf.Lerp(currentVelocity.x, inputManager.Move.x * targetSpeed, AnimationBlendSpeed * Time.fixedDeltaTime);
            currentVelocity.y = Mathf.Lerp(currentVelocity.y, inputManager.Move.y * targetSpeed, AnimationBlendSpeed * Time.fixedDeltaTime);

            float xVelocityDiff = currentVelocity.x - rb.velocity.x;
            float zVelocityDiff = currentVelocity.y - rb.velocity.z;

            rb.AddForce(transform.TransformVector(new Vector3(xVelocityDiff, 0, zVelocityDiff)), ForceMode.VelocityChange);

            animator.SetFloat(xVelocityHash, currentVelocity.x);
            animator.SetFloat(yVelocityHash, currentVelocity.y);
        }

        private void CameraMovement()
        {
            if (!hasAnimator)
            {
                return;
            }

            float mouseX = inputManager.Look.x;
            float mouseY = inputManager.Look.y;

            camera.position = cameraRoot.position;

            xRotation -= mouseY * mouseSens * Time.smoothDeltaTime;
            xRotation = Mathf.Clamp(xRotation, upperLimit, bottomLimit);

            camera.localRotation = Quaternion.Euler(xRotation, 0, 0);
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, mouseX * mouseSens * Time.smoothDeltaTime, 0));
        }

        private void Crouch()
        {
            animator.SetBool(crouchHash, inputManager.Crouch);
        }


        private void Jump()
        {
            
            if (!hasAnimator || !inputManager.Jump)
            {
                return;
            }

            if (grounded && isReadyToJump)
            {
                isReadyToJump = false;
                animator.SetTrigger(jumpHash);
                Invoke(nameof(JumpReset), 0.5f);
            }
        }

        private void JumpReset()
        {
            animator.ResetTrigger(jumpHash);
            isReadyToJump = true;
        }

        public void AddJumpForce()
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector3.up * jumpFactor, ForceMode.Impulse);
            animator.ResetTrigger(jumpHash);        
        }

        private void SampleGround()
        {
            if (!hasAnimator)
            {
                return;
            }

            grounded = Physics.Raycast(rb.worldCenterOfMass, Vector3.down, distanceToGround, groundLayer);

            if (!grounded)
            {
                animator.ResetTrigger(jumpHash);
            }
            SetGroundingAnimation();
            
        }

        private void SetGroundingAnimation()
        {
            animator.SetBool(fallingHash, !grounded);
            animator.SetBool(groundHash, grounded);
        }

        private void OnDrawGizmos()
        {
            if (rb != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(rb.worldCenterOfMass, new Vector3(rb.worldCenterOfMass.x, rb.worldCenterOfMass.y - distanceToGround, rb.worldCenterOfMass.z));
            }
        }
    }
}
