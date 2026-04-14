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
    [SerializeField] private float groundDistance = 0.01f;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isJumping;
    private float currentSpeed;

    private void Start()
    {
        currentSpeed = baseSpeed;
    }

    private void Update()
    {
        if (!playerController.GetMovementState())
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isJogging", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
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
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -3f;
            animator.SetBool("isGrounded", true);
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
        else if (!isGrounded)
        {
            animator.SetBool("isGrounded", false);

            if (isJumping && velocity.y < 0)
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

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        animator.SetBool("isWalking", x != 0 || z != 0);
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
        {
            currentSpeed = sprintSpeed;
            animator.SetBool("isJogging", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = baseSpeed;
            animator.SetBool("isJogging", false);
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}