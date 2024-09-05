using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVButtonHandler : MonoBehaviour
{
    public CCTVManager cctvManager;
    public GameObject cctvUI;

    
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
