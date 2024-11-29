using UnityEngine;

public class SimpleFirstPersonController : MonoBehaviour
{
    public CharacterController controller;
    public Camera playerCamera;  // Updated to use Camera type for better referencing in Inspector

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

    // Footstep variables
    public AudioSource audioSource;
    public AudioClip[] footstepClips;
    public AudioClip[] runFootstepClips;
    public float walkStepInterval = 0.5f; // Interval between walking footsteps
    public float runStepInterval = 0.3f; // Interval between running footsteps
    private float stepTimer;

    // Headbob variables
    public float headBobFrequency = 3f; // Reduced to make the bob effect smoother // Frequency of the head bob
    public float headBobAmplitude = 0.5f; // Increased to make the bob effect more noticeable // Amplitude of the head bob
    private float headBobTimer = 0f;
    private Vector3 originalCameraLocalPosition;
    public float runHeadBobFrequency = 5f;  // Increased frequency for running
    public float runHeadBobAmplitude = 0.25f;

    public float normalFOV = 60f;  // Normal field of view
    public float runFOV = 75f;  // Field of view when running
    public float fovChangeSpeed = 5f;  // Speed of FOV change
    
    // Stamina variables
    public float maxStamina = 5f; // Maximum stamina time in seconds
    private float currentStamina;
    public float staminaRegenRate = 1f; // Stamina regeneration per second
    public float staminaDepletionRate = 1f; // Stamina depletion per second when running
    public float regenDelay = 1f;

    private float regenTimer;
    public UnityEngine.UI.Image staminaBar; // Assign the UI Image bar in the Inspector

    
    private bool isRunning = false;  // Whether the player is currently running

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        stepTimer = walkStepInterval;
        currentStamina = maxStamina;
        originalCameraLocalPosition = playerCamera.transform.localPosition;
        regenTimer = regenDelay;
    }

    void Update()
    {
        if (!disablePlayerControll && !CCTVManager.Instance.isCCTVMode)
        {
            MovePlayer();
            LookAround();
            HandleHeadBob();
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
        UpdateStamina();
    }

    void MovePlayer()
    {
        // Check for running input
        isRunning = Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0 && currentStamina > 0;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float speed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);
        
        if (isRunning)
        {
            regenTimer = regenDelay;
        }
        
        // Footstep sound logic
        if (move.magnitude > 0)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0)
            {
                if (isRunning && runFootstepClips.Length > 0)
                {
                    PlayRunFootstep();
                    stepTimer = runStepInterval;
                }
                else if (footstepClips.Length > 0)
                {
                    PlayFootstep();
                    stepTimer = walkStepInterval;
                }
            }
        }
        else
        {
            stepTimer = isRunning ? runStepInterval : walkStepInterval; // Reset step timer when not moving
        }
    }

    void HandleFieldOfView()
    {
        // Change FOV smoothly based on whether the player is running
        float targetFOV = isRunning ? runFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovChangeSpeed);
    }

    void ApplyGravity()
    {
        // Apply gravity to the player's velocity (only on the Y axis)
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0)
        {
            velocity.y = -2f;  // Keep the player grounded (prevents bouncing)
        }

        // Move the player using the CharacterController, applying gravity
        controller.Move(velocity * Time.deltaTime);
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleHeadBob()
    {
        // Different head-bob parameters for walking vs running
        float currentFrequency = isRunning ? runHeadBobFrequency : headBobFrequency;
        float currentAmplitude = isRunning ? runHeadBobAmplitude : headBobAmplitude;

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            headBobTimer += Time.deltaTime * currentFrequency;
            Vector3 bobOffset = new Vector3(Mathf.Sin(headBobTimer) * currentAmplitude * 0.5f, Mathf.Sin(headBobTimer * 2) * currentAmplitude, 0);
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, originalCameraLocalPosition + bobOffset, Time.deltaTime * 5f);
        }
        else
        {
            headBobTimer = 0;
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, originalCameraLocalPosition, Time.deltaTime * 5f);
        }
    }
    
    void UpdateStamina()
    {
        if (isRunning)
        {
            currentStamina -= staminaDepletionRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
        else
        {
            if (regenTimer > 0)
            {
                regenTimer -= Time.deltaTime;
            }
            else
            {
                // Regenerate stamina if regen timer has expired
                currentStamina += staminaRegenRate * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            }
        }

        // Update the stamina bar UI
        if (staminaBar != null)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
        }
    }

    public void StartSmoothLookAt(Transform target)
    {
        disablePlayerControll = true; // Disable player control during the look-at process
        Vector3 directionToSCP = target.position - playerCamera.transform.position;
        targetRotation = Quaternion.LookRotation(directionToSCP);
        smoothLookAtSCP = true;
    }

    void SmoothLookAtSCP()
    {
        playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, targetRotation, Time.deltaTime * smoothLookSpeed);
        if (Quaternion.Angle(playerCamera.transform.rotation, targetRotation) < 0.1f)
        {
        smoothLookAtSCP = false; // Disable smooth look after reaching the target rotation
        }
    }

// Call this method to start moving the camera closer to the SCP
    public void StartMoveCloser(Vector3 targetPosition)
    {
        originalCameraPosition = playerCamera.transform.position;
        targetCameraPosition = targetPosition; // The position near SCP's face
        moveCameraCloser = true;
    }

// Smoothly moves the camera toward the target position (close to the SCP)
    void MoveCameraCloser()
    {
    // Lerp the camera position closer to the SCP over time
        playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, targetCameraPosition, Time.deltaTime * moveSpeed);

    // If the camera reaches near the target position, stop moving
        if (Vector3.Distance(playerCamera.transform.position, targetCameraPosition) < 0.1f)
        {
        moveCameraCloser = false; // Stop moving closer
        }
    }

    void PlayFootstep()
    {
        if (footstepClips.Length > 0)
        {
            int randomIndex = Random.Range(0, footstepClips.Length);
            audioSource.PlayOneShot(footstepClips[randomIndex]);
        }
    }

    void PlayRunFootstep()
    {
        if (runFootstepClips.Length > 0)
        {
        int randomIndex = Random.Range(0, runFootstepClips.Length);
        audioSource.PlayOneShot(runFootstepClips[randomIndex]);
        }
    }
}
