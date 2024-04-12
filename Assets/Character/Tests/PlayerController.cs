using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public Transform cameraHolder;

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

        public Rigidbody rb;

        //private InputManager inputManager;

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

        public Transform center;

        private bool grounded = true;

        private Vector2 currentVelocity;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            //inputManager = GetComponent<InputManager>();
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

        /*
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

            targetSpeed *= StaticData.msBuffMult;

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
        */
        }

        private void CameraMovement()
        {
            if (!hasAnimator)
            {
                return;
            }

           // float mouseX = inputManager.Look.x;
           // float mouseY = inputManager.Look.y;

            cameraHolder.position = cameraRoot.position;

           // xRotation -= mouseY * mouseSens * Time.smoothDeltaTime;
            xRotation = Mathf.Clamp(xRotation, upperLimit, bottomLimit);

            cameraHolder.localRotation = Quaternion.Euler(xRotation, 0, 0);
           // rb.MoveRotation(rb.rotation * Quaternion.Euler(0, mouseX * mouseSens * Time.smoothDeltaTime, 0));
        }

        private void Crouch()
        {
            if (animator != null)
            {
                //animator.SetBool(crouchHash, inputManager.Crouch);

            }
        }


        private void Jump()
        {

           // if (!hasAnimator || !inputManager.Jump)
           // {
          //      return;
          //  }

            if (grounded && isReadyToJump)
            {
                isReadyToJump = false;
                animator.SetTrigger(jumpHash);
                JumpAddForce();
                Invoke(nameof(JumpReset), 0.5f);
            }
        }

        private void JumpReset()
        {
            animator.ResetTrigger(jumpHash);
            isReadyToJump = true;
        }

        public void JumpAddForce()
        {
            rb.velocity = Vector3.zero;
          //  rb.velocity = new Vector3(rb.velocity.x, jumpFactor * StaticData.jumpBuffMult, rb.velocity.z);
            animator.ResetTrigger(jumpHash);
        }

        private void SampleGround()
        {
            if (!hasAnimator)
            {
                return;
            }

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

        private void OnTriggerStay(Collider other)
        {
            if ((groundLayer & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
            {
                grounded = true;
            }

            //SampleGround();
        }

        private void OnTriggerExit(Collider other)
        {
            if ((groundLayer & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
            {
                grounded = false;
            }

            //SampleGround();
        }
}
