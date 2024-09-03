using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public List<Camera> cameras;
    private int currentCameraIndex = 0;

    void Start()
    {
        
        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].gameObject.SetActive(i == currentCameraIndex);
        }
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchCamera(1);
        }

        
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchCamera(-1);
        }
    }

    void SwitchCamera(int direction)
    {
       
        cameras[currentCameraIndex].gameObject.SetActive(false);

        
        currentCameraIndex += direction;

        
        if (currentCameraIndex >= cameras.Count)
        {
            currentCameraIndex = 0; 
        }
        else if (currentCameraIndex < 0)
        {
            currentCameraIndex = cameras.Count - 1; 
        }

       
        cameras[currentCameraIndex].gameObject.SetActive(true);
    }
}
