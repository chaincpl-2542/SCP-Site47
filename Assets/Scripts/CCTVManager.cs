using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVManager : MonoBehaviour
{
    public static CCTVManager Instance;
    public Action forceNoise;
    public Action changeCamera;
    public Camera mainCamera;
    public Camera[] cctvCameras;
    public GameObject[] screenCCTV;
    public GameObject screenMain;
    public GameObject screenCamera;
    public GameObject nightVisionCamera;
    public GameObject[] nightVisionCCTV;
    [SerializeField] private int currentCameraIndex = -1;
    [SerializeField] private bool isNightMode = false, isCameraMode = false, isCCTVMode = false;
    [SerializeField] GameObject playerPostprocessing;
    [SerializeField] GameObject cctvPostprocessing;
    [SerializeField] CameraRotation[] cameraRotations;
    BatteryController batteryController;

    
    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        cctvPostprocessing.SetActive(false);
        playerPostprocessing.SetActive(true);
        mainCamera.gameObject.SetActive(true);
        mainCamera.enabled = true;
        cameraRotations[0].enabled = true;
        /*foreach (Camera cctvCamera in cctvCameras)
        {
            cctvCamera.gameObject.SetActive(false);
        }*/
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
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMainCamera();
            ReturnMainScreen();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ActivateCamera();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            ActivateCCTV();
        }

        if(isCameraMode || isCCTVMode)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (currentCameraIndex == -1)
                {
                    ToggleNightVisionCamera();
                }
                else
                {
                    ToggleNightVisionCCTV();
                }
            }
        }

    }

    public void ActivateCamera()
    {
        ResetMode();
        ResetCamRotate();
        currentCameraIndex = -1;
        CloseAllScreen();
        screenCamera.SetActive(true);
        isCameraMode = true;
        cameraRotations[0].enabled = true;
    }

    public void ActivateCCTV()
    {
        ResetMode();
        ResetCamRotate();
        currentCameraIndex = 0;
        isCCTVMode = true;
        CloseAllScreen();
        screenCCTV[currentCameraIndex].gameObject.SetActive(true);
        cameraRotations[currentCameraIndex+1].enabled = true;
    }

    public void ToggleNightVisionCamera()
    {
        CloseAllScreen();
        if (!isNightMode)
        {
            nightVisionCamera.SetActive(true);
            isNightMode = true;
            batteryController = FindObjectOfType<BatteryController>();
            batteryController.ToggleNightMode(isNightMode);
        }
        else
        {
            nightVisionCamera.SetActive(false);
            screenCamera.SetActive(true);
            isNightMode = false;
            batteryController = FindObjectOfType<BatteryController>();
            batteryController.ToggleNightMode(isNightMode);
        }
    }

    public void ToggleNightVisionCCTV()
    {
        CloseAllScreen();
        if (!isNightMode)
        {
            nightVisionCCTV[currentCameraIndex].SetActive(true);
            isNightMode = true;
            batteryController = FindObjectOfType<BatteryController>();
            batteryController.ToggleNightMode(isNightMode);
        }
        else
        {
            nightVisionCCTV[currentCameraIndex].SetActive(false);
            screenCCTV[currentCameraIndex].gameObject.SetActive(true);
            isNightMode = false;
            batteryController = FindObjectOfType<BatteryController>();
            batteryController.ToggleNightMode(isNightMode);
        }
    }

    public void CloseAllScreen()
    {
        screenMain.SetActive(false);
        screenCamera.SetActive(false);
        nightVisionCamera.SetActive(false);
        foreach (GameObject screenCCTV in screenCCTV)
        {
            screenCCTV.gameObject.SetActive(false);
        }
        foreach (GameObject nightVisionCCTV in nightVisionCCTV)
        {
            nightVisionCCTV.gameObject.SetActive(false);
        }
    }

    public void ResetMode()
    {
        isCameraMode = false;
        isCCTVMode = false;
        isNightMode = false;
        batteryController = FindObjectOfType<BatteryController>();
        batteryController.ToggleNightMode(isNightMode);
    }

    public void ResetCamRotate()
    {
        foreach (CameraRotation cameraRotation in cameraRotations)
        {
            cameraRotation.enabled = false;
        }
    }

    public void GetCurrentCamera()
    {
        SwitchToCamera(currentCameraIndex);
    }

    public void ReturnMainScreen()
    {
        currentCameraIndex = -1;
        ResetMode();
        ResetCamRotate();
        screenCamera.SetActive(false);
        foreach (GameObject screenCCTV in screenCCTV)
        {
            screenCCTV.gameObject.SetActive(false);
        }
        screenMain.SetActive(true);
        cameraRotations[0].enabled = true;
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

            changeCamera();
        }
        cctvPostprocessing.SetActive(true);
        playerPostprocessing.SetActive(false);
    }

    public void SwitchToNextCamera(int direction)
    {
        ResetCamRotate();
        screenCCTV[currentCameraIndex].gameObject.SetActive(false);
        //cctvCameras[currentCameraIndex].gameObject.SetActive(false);

        currentCameraIndex = (currentCameraIndex + direction + screenCCTV.Length) % screenCCTV.Length;

        //cctvCameras[currentCameraIndex].gameObject.SetActive(true);
        screenCCTV[currentCameraIndex].gameObject.SetActive(true);

        cameraRotations[currentCameraIndex+1].enabled = true;

        isNightMode = false;
        batteryController = FindObjectOfType<BatteryController>();
        batteryController.ToggleNightMode(isNightMode);
    }

    public void ReturnToMainCamera()
    {
        if(currentCameraIndex >= 0 && cctvCameras[currentCameraIndex] != null)
        {
            cctvCameras[currentCameraIndex].gameObject.SetActive(false);
        }

        mainCamera.enabled=true;
        cctvPostprocessing.SetActive(false);
        playerPostprocessing.SetActive(true);
    }

}
