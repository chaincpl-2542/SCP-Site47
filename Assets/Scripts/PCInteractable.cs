using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCInteractable : MonoBehaviour
{
    public GameObject cctvUI;
    public Camera mainCamera;
    private bool isUIOpen = false;

    public void Start()
    {

        cctvUI.SetActive(false);
    }

    void Update()
    {

        if (!isUIOpen && Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    ShowCCTVUI();
                }
            }
        }


        if (isUIOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCCTVUI();
        }
    }

    void ShowCCTVUI()
    {
        cctvUI.GetComponentInParent<CCTVManager>().GetCurrentCamera();
        cctvUI.SetActive(true);
        isUIOpen = true;
    }

    void CloseCCTVUI()
    {
        cctvUI.SetActive(false);
        isUIOpen = false;
    }
}