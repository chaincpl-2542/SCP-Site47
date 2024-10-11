using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryController : MonoBehaviour
{
    public float maxBattery = 100f;
    private float currentBattery;

    public float drainRate = 0.2f;
    public float nightModeDrainRate = 2f;
    private float currentDrainRate;

    public bool isNightMode = false;

    public Image batteryBar;

    void Start()
    {
        currentBattery = maxBattery;
        currentDrainRate = drainRate;
    }

    void Update()
    {
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
    }

    public void ToggleNightMode(bool nightModeStatus)
    {
        isNightMode = nightModeStatus;
    }
}
