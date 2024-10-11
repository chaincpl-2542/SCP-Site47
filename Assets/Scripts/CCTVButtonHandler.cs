using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.SceneView;

public class CCTVButtonHandler : MonoBehaviour
{
    public CCTVManager cctvManager;
    public GameObject cctvUI;
    public GameObject mainScreenUI, cameraScreenUI, CCTVScreenUI;

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAllUI();
            mainScreenUI.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CloseAllUI();
            cameraScreenUI.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            CloseAllUI();
            CCTVScreenUI.SetActive(true);
        }

    }

    public void CloseAllUI()
    {
        mainScreenUI.SetActive(false);
        cameraScreenUI.SetActive(false);
        CCTVScreenUI.SetActive(false);
    }

    public void OnCameraButtonClick(int cameraIndex)
    {
        if (cctvManager == null)
        {
            Debug.LogError("CCTVManager is not assigned!");
            return;
        }

        
        cctvManager.SwitchToCamera(cameraIndex);

        
        //cctvUI.SetActive(false);
    }
}
