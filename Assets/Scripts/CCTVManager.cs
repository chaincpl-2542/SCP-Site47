using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVManager : MonoBehaviour
{
    public Camera mainCamera;
    public Camera[] cctvCameras;
    [SerializeField] private int currentCameraIndex = -1;
    
    void Start()
    {
        mainCamera.gameObject.SetActive(true);
        mainCamera.enabled = true;
        foreach (Camera cctvCamera in cctvCameras)
        {
            cctvCamera.gameObject.SetActive(false);
        }
    }

    
    void Update()
    {
        if (currentCameraIndex >= 0)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SwitchToNextCamera(-1);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                SwitchToNextCamera(1);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ReturnToMainCamera();
            }
        }
    }

    public void GetCurrentCamera()
    {
        SwitchToCamera(currentCameraIndex);
    }

    public void SwitchToCamera(int cameraIndex)
    {
        if (cameraIndex >= 0 && cameraIndex < cctvCameras.Length)
        {
            
            mainCamera.enabled = false;

            
            if (currentCameraIndex >= 0 && currentCameraIndex < cctvCameras.Length)
            {
                cctvCameras[currentCameraIndex].gameObject.SetActive(false);
            }

            
            currentCameraIndex = cameraIndex;
            cctvCameras[currentCameraIndex].gameObject.SetActive(true);
        }
    }

    public void SwitchToNextCamera(int direction)
    {
        cctvCameras[currentCameraIndex].gameObject.SetActive(false);

        currentCameraIndex = (currentCameraIndex + direction + cctvCameras.Length) % cctvCameras.Length;

        cctvCameras[currentCameraIndex].gameObject.SetActive(true);
    }

    public void ReturnToMainCamera()
    {
        if(currentCameraIndex >= 0 && cctvCameras[currentCameraIndex] != null)
        {
            cctvCameras[currentCameraIndex].gameObject.SetActive(false);
        }

        mainCamera.enabled=true;
    }

}
