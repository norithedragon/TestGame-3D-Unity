using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Jump and Gravity")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip footstepSoundA;
    [SerializeField] private AudioClip footstepSoundB;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField, Range(0f, 2f)] private float footstepVolume = 1f;
    [SerializeField, Range(0f, 2f)] private float jumpVolume = 1f;
    [SerializeField] private float footstepInterval = 0.45f;

    private CharacterController characterController;
    private Vector2 movementInput;

    private float verticalVelocity;
    private float footstepTimer;

    private int footstepIndex;
    private bool isGrounded;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        footstepIndex = 0;
    }

    private void Update()
    {
        CheckGround();
        HandleGravity();
        RotatePlayerWithCamera();
        MovePlayer();
        UpdateAnimations();
        HandleFootsteps();
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (!value.isPressed || !isGrounded)
        {
            return;
        }

        verticalVelocity = Mathf.Sqrt(
            jumpHeight * -2f * gravity
        );

        PlayJumpSound();
    }

    private void CheckGround()
    {
        if (groundCheck == null)
        {
            isGrounded = false;
            return;
        }

        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );
    }

    private void HandleGravity()
    {
        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
    }

    private void RotatePlayerWithCamera()
    {
        if (cameraTransform == null)
        {
            return;
        }

        Vector3 facingDirection = cameraTransform.forward;
        facingDirection.y = 0f;

        if (facingDirection.sqrMagnitude < 0.01f)
        {
            return;
        }

        facingDirection.Normalize();

        Quaternion targetRotation =
            Quaternion.LookRotation(facingDirection);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void MovePlayer()
    {
        if (cameraTransform == null)
        {
            return;
        }

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement =
            cameraForward * movementInput.y +
            cameraRight * movementInput.x;

        movement = Vector3.ClampMagnitude(movement, 1f);

        Vector3 finalMovement =
            movement * movementSpeed;

        finalMovement.y = verticalVelocity;

        characterController.Move(
            finalMovement * Time.deltaTime
        );
    }

    private void UpdateAnimations()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetFloat(
            "Speed",
            movementInput.magnitude
        );

        animator.SetBool(
            "IsGrounded",
            isGrounded
        );
    }

    private void HandleFootsteps()
    {
        bool isMoving =
            movementInput.sqrMagnitude > 0.01f;

        if (!isGrounded || !isMoving)
        {
            footstepTimer = 0f;
            return;
        }

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0f)
        {
            PlayFootstepSound();
            footstepTimer = footstepInterval;
        }
    }

    private void PlayFootstepSound()
    {
        if (audioSource == null)
        {
            return;
        }

        AudioClip selectedSound;

        if (footstepIndex == 0)
        {
            selectedSound = footstepSoundA;
            footstepIndex = 1;
        }
        else
        {
            selectedSound = footstepSoundB;
            footstepIndex = 0;
        }

        if (selectedSound == null)
        {
            return;
        }

        audioSource.PlayOneShot(
            selectedSound,
            footstepVolume
        );
    }

    private void PlayJumpSound()
    {
        if (audioSource == null || jumpSound == null)
        {
            return;
        }

        audioSource.PlayOneShot(
            jumpSound,
            jumpVolume
        );
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }
}