using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChargeStation : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject tabletOnCharge;
    public bool isCharging = false;

    [SerializeField] private TMP_Text batteryText;
    private float currentBattery;
    private float currentChargeRate = 10;
    private int showBattery;
    private static List<ChargeStation> chargeStations = new List<ChargeStation>();

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
        }
    }

    private void OnDestroy()
    {
        chargeStations.Remove(this);
    }

    public void OnInteract()
    {
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
