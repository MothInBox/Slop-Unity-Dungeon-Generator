using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpHeight = 1.5f;
    public float gravity = -19.62f; // 2x gravity feels better for FPS

    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;
    public Transform cameraTransform; // Drag the Camera here in Inspector

    // Private
    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleGravityAndJump();
    }

    private void HandleMouseLook()
    {
        if (Mouse.current == null)
        {
            return;
        }

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        // Vertical look — clamp so you cant flip upside down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal look — rotate the whole capsule
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        Vector2 moveInput = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput.x -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput.x += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveInput.y -= 1f;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveInput.y += 1f;
        }

        if (Gamepad.current != null)
        {
            moveInput += Gamepad.current.leftStick.ReadValue();
        }

        moveInput = Vector2.ClampMagnitude(moveInput, 1f);

        // Move relative to where the capsule is facing
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        bool isSprinting = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
        if (Gamepad.current != null)
        {
            isSprinting |= Gamepad.current.rightShoulder.isPressed;
        }

        float speed = isSprinting ? sprintSpeed : walkSpeed;
        controller.Move(move * speed * Time.deltaTime);
    }

    private void HandleGravityAndJump()
    {
        isGrounded = controller.isGrounded;

        // Reset downward velocity when grounded
        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f; // small negative keeps grounded check stable

        // Jump
        bool jumpPressed = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        if (Gamepad.current != null)
        {
            jumpPressed |= Gamepad.current.buttonSouth.wasPressedThisFrame;
        }

        if (jumpPressed && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
