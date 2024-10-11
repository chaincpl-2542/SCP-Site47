using UnityEngine;

namespace Peerawit
{
    public class FirstPersonController : MonoBehaviour
    {
        public float headBobFrequency = 2.5f;
        public float headBobAmplitude = 0.15f;
        private float headBobTimer = 0f;
        private Vector3 originalHeadPosition;

        public float stamina = 100f;
        public float maxStamina = 100f;
        public float staminaDrain = 10f;
        public float staminaRecovery = 5f;
        private bool isSprinting;

        public float leanAngle = 15f;
        public float leanSpeed = 5f;
        private float currentLean = 0f;

        public Camera playerCamera;
        public float normalFOV = 60f;
        public float sprintFOV = 75f;
        public float fovChangeSpeed = 5f;
        public float mouseSensitivity = 100f;
        private float xRotation = 0f;

        public float walkSpeed = 5f;
        public float runSpeed = 10f;
        public float crouchSpeed = 2.5f;

        public float crouchHeight = 0.5f;
        private float originalHeight;
        public CharacterController controller;

        private Vector3 velocity;
        public float gravity = -9.81f;
        private bool isGrounded;
        private bool isCrouching = false;
        public bool disablePlayerControll = false;
        private Vector3 originalCameraPosition; 
        private Vector3 targetCameraPosition;
            
        private bool smoothLookAtSCP = false;
        private Quaternion targetRotation;
        public float smoothLookSpeed = 2f;
        private bool moveCameraCloser = false;
        public float moveSpeed = 2f;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            controller = GetComponent<CharacterController>();
            originalHeight = controller.height;
            originalHeadPosition = playerCamera.transform.localPosition;
        }

        void Update()
        {
            if(!disablePlayerControll)
            {
                HandleMouseLook();
                HandleFieldOfView();
                HandleStamina();
                HandleHeadBobbing();
                HandleLeaning();
            }

            // Movement handling
            isGrounded = controller.isGrounded;
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float speed = walkSpeed;

            if (Input.GetKey(KeyCode.LeftShift) && isGrounded && Input.GetAxis("Vertical") > 0 && stamina > 0)
            {
                speed = runSpeed;
                isSprinting = true;
            }
            else
            {
                isSprinting = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                isCrouching = !isCrouching;
                controller.height = isCrouching ? crouchHeight : originalHeight;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            if (isGrounded)
            {
                controller.Move(move * speed * Time.deltaTime);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        void FixedUpdate()
        {
            HandleMomentum(); // Move momentum handling to FixedUpdate
        }

        void HandleMouseLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            float strafe = Input.GetAxis("Horizontal");
            float tiltAngle = Mathf.Clamp(-mouseX * 0.5f, -10f, 10f);
            float strafeTiltAngle = Mathf.Clamp(strafe * 5f, -5f, 5f);

            //Quaternion targetRotation = Quaternion.Euler(xRotation, 0f, tiltAngle + strafeTiltAngle + currentLean);
            //playerCamera.transform.localRotation = Quaternion.Slerp(playerCamera.transform.localRotation, targetRotation, Time.deltaTime * 5f);

            transform.Rotate(Vector3.up * mouseX);
        }

        void HandleHeadBobbing()
        {
            if (isGrounded && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
            {
                headBobTimer += Time.deltaTime * headBobFrequency;
                Vector3 bobOffset = new Vector3(Mathf.Sin(headBobTimer) * headBobAmplitude * 0.5f, Mathf.Sin(headBobTimer * 2) * headBobAmplitude, 0);
                playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, originalHeadPosition + bobOffset, Time.deltaTime * 5f);
            }
            else
            {
                headBobTimer = 0;
                playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, originalHeadPosition, Time.deltaTime * 5f);
            }
        }

        void HandleStamina()
        {
            isSprinting = Input.GetKey(KeyCode.LeftShift) && isGrounded && stamina > 0 && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") > 0);
            if (isSprinting)
            {
                stamina -= staminaDrain * Time.deltaTime;
                stamina = Mathf.Clamp(stamina, 0, maxStamina);
            }
            else
            {
                stamina += staminaRecovery * Time.deltaTime;
                stamina = Mathf.Clamp(stamina, 0, maxStamina);
            }
        }

        void HandleLeaning()
        {
            float targetLeanAngle = 0f;
            Vector3 targetLeanPosition = originalHeadPosition;

            if (Input.GetKey(KeyCode.Q))
            {
                targetLeanAngle = leanAngle;
                targetLeanPosition += new Vector3(-0.5f, 0, 0);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                targetLeanAngle = -leanAngle;
                targetLeanPosition += new Vector3(0.5f, 0, 0);
            }

            currentLean = Mathf.Lerp(currentLean, targetLeanAngle, Time.deltaTime * leanSpeed);
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, targetLeanPosition, Time.deltaTime * leanSpeed);
        }

        void HandleFieldOfView()
        {
            float targetFOV = isSprinting ? sprintFOV : normalFOV;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovChangeSpeed);
        }

        void HandleMomentum()
        {
            if (isGrounded && velocity.magnitude > 0.1f && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                velocity.x = Mathf.Lerp(velocity.x, 0, Time.fixedDeltaTime * 3f);
                velocity.z = Mathf.Lerp(velocity.z, 0, Time.fixedDeltaTime * 3f);
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
    }
}
