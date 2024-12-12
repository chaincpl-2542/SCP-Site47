using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CCTVButtonHandler : MonoBehaviour
{
    [FormerlySerializedAs("cctvManager")] public TabletManager tabletManager;
    public GameObject cctvUI;
    public GameObject mainScreenUI, cameraScreenUI, CCTVScreenUI;
    BatteryController batteryController;

    public void OpenMainScreen()
    {
        CloseAllUI();
        mainScreenUI.SetActive(true);
    }

    public void OpenCCTVScreen()
    {
        CloseAllUI();
        CCTVScreenUI.SetActive(true);
    }

    public void CloseAllUI()
    {
        mainScreenUI.SetActive(false);
        cameraScreenUI.SetActive(false);
        CCTVScreenUI.SetActive(false);
    }

    public void OnCameraButtonClick(int cameraIndex)
    {
        if (tabletManager == null)
        {
            Debug.LogError("CCTVManager is not assigned!");
            return;
        }
    }
}
