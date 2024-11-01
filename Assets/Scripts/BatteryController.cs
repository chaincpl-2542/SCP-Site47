using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BatteryController : MonoBehaviour
{
    public float maxBattery = 100f;
    private float currentBattery;

    public float drainRate = 0.2f;
    public float nightModeDrainRate = 2f;
    private float currentDrainRate;

    public bool isNightMode = false, isBatteryDrain = false;

    public Image batteryBar;

    public TMP_Text batteryPercentage;

    void Start()
    {
        currentBattery = maxBattery;
        currentDrainRate = drainRate;
    }

    void Update()
    {
        currentBattery = PlayerPrefs.GetFloat("Battery");

        if (isNightMode)
        {
            currentDrainRate = nightModeDrainRate;
        }
        else
        {
            currentDrainRate = drainRate;
        }

        currentBattery -= currentDrainRate * Time.deltaTime;

        currentBattery = Mathf.Clamp(currentBattery, 0, maxBattery);

        batteryBar.fillAmount = currentBattery / maxBattery;

        int totalBattery = (int)currentBattery;

        batteryPercentage.text = totalBattery.ToString() + "%";

        if(currentBattery <= 0)
        {
            isBatteryDrain = true;
        }
        else
        {
            isBatteryDrain = false;
        }

        PlayerPrefs.SetFloat("Battery", currentBattery);
    }

    public void ToggleNightMode(bool nightModeStatus)
    {
        isNightMode = nightModeStatus;
    }
}
