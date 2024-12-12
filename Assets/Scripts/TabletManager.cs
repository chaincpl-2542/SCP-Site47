using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TabletManager : MonoBehaviour
{
    public GameObject tabletSCP;
    public static TabletManager Instance;
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
    [SerializeField] AudioSource pickUpCamSound, changeModeSound, switchCamSound;
    BatteryController batteryController;
    CCTVButtonHandler cctvButtonHandler;
    public ChargeStation chargeStation;
    [SerializeField] private TextMeshProUGUI assessText;
    [SerializeField] private int assessLevel = 1;

    [SerializeField] private bool _canJammer;
    [SerializeField] GameObject textJammer;
    private bool jammerCooldown = false;
    [SerializeField] private float jammerRange = 10f; // Jammer's effective range
    [SerializeField] private float stunDuration = 5f; // Duration of the SCP stun

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
        chargeStation = FindObjectOfType<ChargeStation>();
        PlayerPrefs.SetFloat("Battery", 100f);
    }
    
    void Update()
    {
        if (isTablet)
        {
            if (PlayerPrefs.GetFloat("Battery") <= 0)
            {
                CloseAllScreen();
                cctvButtonHandler = GetComponent<CCTVButtonHandler>();
                cctvButtonHandler.CloseAllUI();
            }
            else
            {
                if (isTablet || isCCTVMode)
                {
                    if (Input.GetKeyDown(KeyCode.R))
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
                    
                    if (_canJammer && Input.GetKeyDown(KeyCode.F))
                    {
                        UseJammer();
                    }
                }
            }
        }

        if (!chargeStation.IsAnyCharging())
        {
            if (Input.GetKeyDown(KeyCode.Q) && !isDisableTablet) // Prevent Q input when disabled
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

    public void PickJammer()
    {
        _canJammer = true; 
        textJammer.SetActive(true); 
        Debug.Log("Jammer picked up and equipped!");
    }

    private void UseJammer()
    {
        if (jammerCooldown)
        {
            Debug.Log("Jammer is on cooldown!");
            return;
        }
        
        float currentBattery = PlayerPrefs.GetFloat("Battery");

        // Check if battery is sufficient
        if (currentBattery >= 25f)
        {
            // Deduct 25% battery
            PlayerPrefs.SetFloat("Battery", currentBattery - 25f);

            TriggerJammerEffect();
            
            StartCoroutine(StartJammerCooldown(10f));
        }
        else
        {
            Debug.Log("Not enough battery to use Jammer!");
        }
    }
    
    private void TriggerJammerEffect()
    {
        // Create a temporary sphere to detect SCPs within range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, jammerRange);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("SCP")) // Ensure SCP has the correct tag
            {
                SCPController scpController = hitCollider.GetComponent<SCPController>();
                if (scpController != null)
                {
                    scpController.StunSCP(stunDuration);
                    Debug.Log("Jammer stunned the SCP!");
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Set the Gizmo color
        Gizmos.color = Color.green;

        // Draw a wire sphere to represent the Jammer range
        Gizmos.DrawWireSphere(transform.position, jammerRange);
    }
    
    private IEnumerator StartJammerCooldown(float cooldownTime)
    {
        jammerCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        jammerCooldown = false;
        Debug.Log("Jammer is ready to use again!");
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
