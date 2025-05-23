using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabletManager : MonoBehaviour
{
    public GameObject tabletSCP;
    public static TabletManager Instance;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera _tabletCamera;
    [SerializeField] private Camera _nightVisionCamera;
    [SerializeField] private GameObject screenVision;
    [SerializeField] private GameObject nightVisionCamera;
    
    public bool isDisableTablet = false;
    [SerializeField] private int currentCameraIndex = -1;
    public bool isNightMode = false, isCameraMode = false, isTablet = false;
    [SerializeField] GameObject playerPostprocessing;
    [SerializeField] GameObject cctvPostprocessing;
    [SerializeField] AudioSource pickUpCamSound, changeModeSound, switchCamSound;
    [SerializeField] private BatteryController batteryController;
    CCTVButtonHandler cctvButtonHandler;
    public ChargeStation chargeStation;
    [SerializeField] private TextMeshProUGUI accessText;
    [SerializeField] private int accessLevel = 3;

    [SerializeField] GameObject textJammer;
    private bool jammerCooldown = false;
    [SerializeField] private bool _canJammer;
    [SerializeField] private Slider jammerCooldownSlider;
    [SerializeField] private float _jammerCooldownDuration = 10f;
    private float _jammerCooldownTimer = 0f;
    [SerializeField] private float jammerRange = 10f; // Jammer's effective range
    [SerializeField] private float stunDuration = 5f; // Duration of the SCP stun
    [SerializeField] private GameObject _jammerVfx;
    [SerializeField] private AudioSource _jammerSound;

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
        accessText.text = "Access Level : " + accessLevel;
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
                cctvButtonHandler = GetComponent<CCTVButtonHandler>();
                cctvButtonHandler.CloseAllUI();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ToggleNightVisionCamera();
                changeModeSound.Play();
            }
            
            if (_canJammer && Input.GetKeyDown(KeyCode.F))
            {
                UseJammer();
            }
            
        }
        UpdateJammerCooldownSlider();

        if (!chargeStation.IsAnyCharging())
        {
            if (Input.GetKeyDown(KeyCode.Q) && !isDisableTablet)
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
    
    public int GetPlayerAccessLevel()
    {
        return accessLevel;
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
            
            StartJammerCooldown();
            StartCoroutine(HideEffect());
            _jammerSound.Play();
            _jammerVfx.SetActive(true);
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
    
    private void UpdateJammerCooldownSlider()
    {
        if (jammerCooldown)
        {
            _jammerCooldownTimer += Time.deltaTime;

            // Update slider value
            float cooldownProgress = _jammerCooldownTimer / _jammerCooldownDuration;
            jammerCooldownSlider.value = Mathf.Clamp01(cooldownProgress);

            // Check if cooldown is complete
            if (_jammerCooldownTimer >= _jammerCooldownDuration)
            {
                jammerCooldown = false;
                jammerCooldownSlider.value = 1f; // Ensure slider is full
                Debug.Log("Jammer is ready to use again!");
            }
        }
        else
        {
            jammerCooldownSlider.value = 1f; // Jammer ready
        }
    }
    
    private void StartJammerCooldown()
    {
        jammerCooldown = true;
        _jammerCooldownTimer = 0f; // Reset the timer
    }
    
    private void OnDrawGizmosSelected()
    {
        // Set the Gizmo color
        Gizmos.color = Color.green;

        // Draw a wire sphere to represent the Jammer range
        Gizmos.DrawWireSphere(transform.position, jammerRange);
    }

    public void ActiveTablet()
    {
        if (!isTablet)
        {
            tabletSCP.SetActive(true);
            isTablet = true;
            isCameraMode = true;
        }
        else
        {
            tabletSCP.SetActive(false);
            isTablet = false;
            isCameraMode = false;
            
            ReturnToMainCamera();
            ReturnMainScreen();
            nightVisionCamera.SetActive(false);
            isNightMode = false;
            batteryController.ToggleNightMode(isNightMode);
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

    public void ToggleNightVisionCamera()
    {
        if (!isNightMode)
        {
            nightVisionCamera.SetActive(true);
            screenVision.SetActive(false);
            isNightMode = true;
            batteryController.ToggleNightMode(isNightMode);
            
        }
        else
        {
            ReturnToMainCamera();
            ReturnMainScreen();
            nightVisionCamera.SetActive(false);
            isNightMode = false;
            batteryController.ToggleNightMode(isNightMode);
        }
    }

    public void ToggleNightVisionCCTV()
    {
        if (!isNightMode)
        {
            isNightMode = true;
            batteryController.ToggleNightMode(isNightMode);
        }
        else
        {
            isNightMode = false;
            batteryController.ToggleNightMode(isNightMode);
        }
    }

    public void ResetMode()
    {
        isNightMode = false;
        batteryController.ToggleNightMode(isNightMode);
    }

    public void ReturnMainScreen()
    {
        ResetMode();
        screenVision.SetActive(true);
    }

    public void ReturnToMainCamera()
    {
        mainCamera.enabled=true;
        cctvPostprocessing.SetActive(false);
        playerPostprocessing.SetActive(true);
    }

    public void UpGradeAssessLevel()
    {
        accessLevel = 5;
        accessText.text = "Access Level : " + accessLevel;
    }

    private IEnumerator HideEffect()
    {
        yield return new WaitForSeconds(0.5f);
        _jammerVfx.SetActive(false);
    }

}
