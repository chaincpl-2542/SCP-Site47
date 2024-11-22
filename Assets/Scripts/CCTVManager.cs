using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CCTVManager : MonoBehaviour
{
    public GameObject tabletSCP;
    public static CCTVManager Instance;
    public Action forceNoise;
    public Action changeCamera;
    public Camera mainCamera;
    public Camera[] cctvCameras;
    public GameObject[] screenCCTV;
    public GameObject screenVision;
    public GameObject nightVisionCamera;
    public GameObject[] nightVisionCCTV;
    public bool isDisableTablet = false;
    [SerializeField] private int currentCameraIndex = -1;
    public bool isNightMode = false, isCameraMode = false, isCCTVMode = false, isTablet = false;
    [SerializeField] GameObject playerPostprocessing;
    [SerializeField] GameObject cctvPostprocessing;
    [SerializeField] CameraRotation[] cameraRotations;
    [SerializeField] AudioSource pickUpCamSound, changeModeSound, switchCamSound;
    BatteryController batteryController;
    CCTVButtonHandler cctvButtonHandler;
    public ChargeStation chargeStation;
    [SerializeField] private TextMeshProUGUI assessText;
    [SerializeField] private int assessLevel = 1;

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
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
        chargeStation = FindObjectOfType<ChargeStation>();
        PlayerPrefs.SetFloat("Battery", 100f);
    }
    
    void Update()
    {
        if (isTablet)
        {
            if (PlayerPrefs.GetFloat("Battery")<=0)
            {
                CloseAllScreen();
                cctvButtonHandler = GetComponent<CCTVButtonHandler>();
                cctvButtonHandler.CloseAllUI();
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ReturnToMainCamera();
                    ReturnMainScreen();

                    ActiveTablet();
                    cctvButtonHandler = GetComponent<CCTVButtonHandler>();
                    cctvButtonHandler.OpenMainScreen();
                }

                if (isTablet || isCCTVMode)
                {
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        if (currentCameraIndex == -1)
                        {
                            ToggleNightVisionCamera();
                            changeModeSound.Play();
                        }
                        else
                        {
                            ToggleNightVisionCCTV();
                            changeModeSound.Play();
                        }
                    }
                } 
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                UpGradeAssessLevel();
            }
            
        }

        if (!chargeStation.IsAnyCharging())
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                ActiveTablet();
                cctvButtonHandler = GetComponent<CCTVButtonHandler>();
                cctvButtonHandler.OpenMainScreen();
            }
        }

        if (chargeStation.IsAnyCharging())
        {
            DisActiveTablet();
        }
    }

    public void ActiveTablet()
    {
        if (!isTablet)
        {
            tabletSCP.SetActive(true);
            isTablet = true;
            isCameraMode = true;
            currentCameraIndex = -1;
            CloseAllScreen();
            ReturnToMainCamera();
            ReturnMainScreen();
        }
        else
        {
            tabletSCP.SetActive(false);
            isTablet = false;
            isCameraMode = false;
            currentCameraIndex = -1;
        }
        pickUpCamSound.Play();
    }

    public void DisActiveTablet()
    {
        tabletSCP.SetActive(false);
        isTablet = false;
        isCameraMode = false;
        currentCameraIndex = -1;
    }

    public void ActivateCCTV()
    {
        ResetMode();
        currentCameraIndex = 0;
        isCCTVMode = true;
        CloseAllScreen();
        screenVision.SetActive(false);
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
            ReturnToMainCamera();
            ReturnMainScreen();
            nightVisionCamera.SetActive(false);
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
        screenVision.SetActive(false);
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
        isCCTVMode = false;
        isNightMode = false;
        batteryController = FindObjectOfType<BatteryController>();
        batteryController.ToggleNightMode(isNightMode);
    }

    public void GetCurrentCamera()
    {
        SwitchToCamera(currentCameraIndex);
    }

    public void ReturnMainScreen()
    {
        currentCameraIndex = -1;
        ResetMode();
        screenVision.SetActive(true);
        foreach (GameObject screenCCTV in screenCCTV)
        {
            screenCCTV.gameObject.SetActive(false);
        }
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
        screenCCTV[currentCameraIndex].gameObject.SetActive(false);

        currentCameraIndex = (currentCameraIndex + direction + screenCCTV.Length) % screenCCTV.Length;

        screenCCTV[currentCameraIndex].gameObject.SetActive(true);

        cameraRotations[currentCameraIndex].enabled = true;

        isNightMode = false;
        batteryController = FindObjectOfType<BatteryController>();
        batteryController.ToggleNightMode(isNightMode);
        foreach(GameObject screenNight in nightVisionCCTV)
        {
            screenNight.SetActive(false);
        }
    }

    public void ReturnToMainCamera()
    {
        currentCameraIndex = -1;
        if(currentCameraIndex >= 0 && cctvCameras[currentCameraIndex] != null)
        {
            cctvCameras[currentCameraIndex].gameObject.SetActive(false);
        }

        mainCamera.enabled=true;
        cctvPostprocessing.SetActive(false);
        playerPostprocessing.SetActive(true);
    }

    public void UpGradeAssessLevel()
    {
        assessLevel++;
        assessText.text = "Assess Level : " + assessLevel;
    }

}
