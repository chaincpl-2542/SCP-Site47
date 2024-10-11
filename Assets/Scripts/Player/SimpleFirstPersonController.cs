using UnityEngine;

public class SimpleFirstPersonController : MonoBehaviour
{
    public CharacterController controller;
    public Transform playerCamera;

    public float walkSpeed = 5f; // Walking speed
    public float runSpeed = 10f; // Running speed
    public float mouseSensitivity = 100f; // Sensitivity for looking around

    private float xRotation = 0f; // Keep track of vertical camera rotation

    public float gravity = -9.81f;  // Gravity force
    public float groundCheckDistance = 0.4f;  // Distance to check if player is grounded
    public LayerMask groundMask;  // Define which layers are considered "ground"

    private Vector3 velocity;  // Velocity of the player (including gravity)
    private bool isGrounded;  // To check if the player is grounded

    public Transform groundCheck; 

    public bool disablePlayerControll = false;

    private bool smoothLookAtSCP = false; // Whether the camera should smoothly look at the SCP
    private Quaternion targetRotation; // Target rotation for smooth look at SCP
    public float smoothLookSpeed = 2f; // Speed of smooth rotation

    private bool moveCameraCloser = false; // Flag for moving the camera closer to the SCP
    public float moveSpeed = 2f; // Speed of moving the camera closer
    private Vector3 originalCameraPosition; // To store the original camera position
    private Vector3 targetCameraPosition; // The target position for the camera

    void Start()
    {
        // Lock the cursor in the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!disablePlayerControll)
        {
            MovePlayer();
            LookAround();
        }

        if (smoothLookAtSCP)
        {
            SmoothLookAtSCP();
        }

        if (moveCameraCloser)
        {
            MoveCameraCloser();
        }
        ApplyGravity();
    }

    void MovePlayer()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // Keep the player "stuck" to the ground (prevents floating)
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);
    }

    void ApplyGravity()
    {
        // Apply gravity to the player's velocity (only on the Y axis)
        velocity.y += gravity * Time.deltaTime;

        // Move the player using the CharacterController, applying gravity
        controller.Move(velocity * Time.deltaTime);
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void StartSmoothLookAt(Transform target)
    {
        disablePlayerControll = true; // Disable player control during the look-at process
        Vector3 directionToSCP = target.position - playerCamera.position;
        targetRotation = Quaternion.LookRotation(directionToSCP);
        smoothLookAtSCP = true;
    }

    void SmoothLookAtSCP()
    {
        playerCamera.rotation = Quaternion.Slerp(playerCamera.rotation, targetRotation, Time.deltaTime * smoothLookSpeed);
        if (Quaternion.Angle(playerCamera.rotation, targetRotation) < 0.1f)
        {
            smoothLookAtSCP = false; // Disable smooth look after reaching the target rotation
        }
    }

    // Call this method to start moving the camera closer to the SCP
    public void StartMoveCloser(Vector3 targetPosition)
    {
        originalCameraPosition = playerCamera.position;
        targetCameraPosition = targetPosition; // The position near SCP's face
        moveCameraCloser = true;
    }

    // Smoothly moves the camera toward the target position (close to the SCP)
    void MoveCameraCloser()
    {
        // Lerp the camera position closer to the SCP over time
        playerCamera.position = Vector3.Lerp(playerCamera.position, targetCameraPosition, Time.deltaTime * moveSpeed);

        // If the camera reaches near the target position, stop moving
        if (Vector3.Distance(playerCamera.position, targetCameraPosition) < 0.1f)
        {
            moveCameraCloser = false; // Stop moving closer
        }
    }
}
