using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Peerawit
{

    public class NewFirstPersonController : MonoBehaviour
    {


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
        }

        void Update()
        {
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

        void HandleMouseLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
            controller.Move(velocity * Time.deltaTime);
        }
    }



}