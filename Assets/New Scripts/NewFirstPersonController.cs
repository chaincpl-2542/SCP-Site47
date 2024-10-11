using UnityEngine;
namespace Peerawit
{

    public class NewFirstPersonController : MonoBehaviour
    {
        public float headBobFrequency = 1.5f;
        public float headBobAmplitude = 0.05f;
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
        public float mouseSensitivity = 100f;
        private float xRotation = 0f;

        public float walkSpeed = 5f;
        public float runSpeed = 10f;
        public float crouchSpeed = 2.5f;
        public float verticalJumpForce = 3f;
        public float directionalJumpForce = 5f;
        public float crouchHeight = 0.5f;
        private float originalHeight;
        public CharacterController controller;

        private Vector3 velocity;
        public float gravity = -9.81f;
        private bool isGrounded;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            controller = GetComponent<CharacterController>();
            originalHeight = controller.height;
            originalHeadPosition = playerCamera.transform.localPosition;
            Cursor.lockState = CursorLockMode.Locked;

            controller = GetComponent<CharacterController>();
            originalHeight = controller.height;
        }

        void Update()
        {
            HandleMouseLook();
            HandleHeadBobbing();
            HandleLeaning();
            HandleStamina();
            HandleMouseLook();

            isGrounded = controller.isGrounded;
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float speed = walkSpeed;

            if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
            {
                speed = runSpeed;
            }
            else if (Input.GetKey(KeyCode.C))
            {
                speed = crouchSpeed;
                controller.height = crouchHeight;
            }
            else
            {
                controller.height = originalHeight;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            if (isGrounded)
            {
                controller.Move(move * speed * Time.deltaTime);
                velocity.x = 0f;
                velocity.z = 0f;
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                float jumpSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
                Vector3 jumpDirection = transform.forward * jumpSpeed;
                velocity = jumpDirection * 0.5f; // Reduce the effect for a balanced jump
                velocity.y = Mathf.Sqrt(verticalJumpForce * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
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
            isSprinting = Input.GetKey(KeyCode.LeftShift) && isGrounded && stamina > 0;
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
            Vector3 targetLeanPosition = Vector3.zero;

            if (Input.GetKey(KeyCode.Q))
            {
                targetLeanAngle = -leanAngle;
                targetLeanPosition = new Vector3(-leanAngle * 0.01f, 0, 0);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                targetLeanAngle = leanAngle;
                targetLeanPosition = new Vector3(leanAngle * 0.01f, 0, 0);
            }

            currentLean = Mathf.Lerp(currentLean, targetLeanAngle, Time.deltaTime * leanSpeed);
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, originalHeadPosition + targetLeanPosition, Time.deltaTime * leanSpeed);
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, currentLean);
        }


        void HandleMouseLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }
}