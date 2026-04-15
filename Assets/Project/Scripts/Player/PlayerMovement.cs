using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float gravity = -12f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float groundDistance = 0.3f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float turnSpeed = 120f;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isJumping;
    private float currentSpeed;
    private Transform cameraTarget;

    private void Start()
    {
        currentSpeed = baseSpeed;
        cameraTarget = playerController.GetCameraTarget();
    }

    private void Update()
    {
        if (!playerController.GetMovementState())
        {
            animator.SetFloat("speed", 0f);
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            animator.SetBool("isGrounded", true);
            return;
        }

        HandleGroundCheck();
        HandleMovement();
        HandleJump();
        HandleSprint();
        ApplyGravity();
    }

    private void HandleGroundCheck()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -3f;
            isJumping = false;
            animator.SetBool("isGrounded", true);
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
        else if (!isGrounded)
        {
            animator.SetBool("isGrounded", false);

            if (velocity.y < -2f)
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", true);
                isJumping = false;
            }
        }
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 cameraForward = cameraTarget.forward;
        Vector3 cameraRight = cameraTarget.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 move = (cameraForward * z + cameraRight * x).normalized;

        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                720f * Time.deltaTime
            );
        }

        float speedValue = move.magnitude * (currentSpeed / sprintSpeed);
        animator.SetFloat("speed", speedValue, 0.1f, Time.deltaTime);
        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("isJumping", true);
            isJumping = true;
        }
    }

    private void HandleSprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            currentSpeed = sprintSpeed;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            currentSpeed = baseSpeed;
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}