using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChargeStation : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject tabletOnCharge;
    public bool isCharging = false;

    [SerializeField] private TMP_Text batteryText;
    [SerializeField] private Image batteryImage;
    private float currentBattery;
    private float currentChargeRate = 10;
    private int showBattery;
    private static List<ChargeStation> chargeStations = new List<ChargeStation>();
    public AudioSource audioSource;

    private void Awake()
    {
        chargeStations.Add(this);
    }

    private void Update()
    {
        if (isCharging)
        {
            currentBattery = PlayerPrefs.GetFloat("Battery");
            currentBattery += currentChargeRate * Time.deltaTime;

            currentBattery = Mathf.Clamp(currentBattery, 0, 100);
            PlayerPrefs.SetFloat("Battery", currentBattery);
            showBattery = (int)currentBattery;
            batteryText.text = showBattery + "%";
            batteryImage.fillAmount = currentBattery / 100;
        }
    }

    private void OnDestroy()
    {
        chargeStations.Remove(this);
    }

    public void OnInteract()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }

        EventControl.Instance.PickupTablet();
        if (!IsAnyCharging())
        {
            tabletOnCharge.SetActive(true);
            isCharging = true;
            Debug.Log("Charging started on this station.");
        }
        else
        {
            tabletOnCharge.SetActive(false);
            isCharging = false;
            batteryText.text = "";
            Debug.Log("Another station is already charging.");
        }
    }

    public bool IsAnyCharging()
    {
        foreach (var station in chargeStations)
        {
            if (station.isCharging)
            {
                return true;
            }
        }
        return false;
    }
}
