using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{

    public float rotationSpeed = 5f; 
    public float maxRotationAngle = 30f;

    private float currentRotationX = 0f;
    private Quaternion initialRotation;

    void Start()
    {
       
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        
        float rotationInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            rotationInput = -rotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotationInput = rotationSpeed * Time.deltaTime;
        }

        
        currentRotationX = Mathf.Clamp(currentRotationX + rotationInput, -maxRotationAngle, maxRotationAngle);

        
        transform.localRotation = initialRotation * Quaternion.Euler(0f, currentRotationX, 0f);
    }
}
