using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCInteractable : MonoBehaviour
{
    public GameObject cctvUI;           
    public Camera playerCamera;         
    public float interactRange = 3f;    
    private bool isUIOpen = false;
    private FirstPersonController playerMovement; 

    void Start()
    {
        cctvUI.SetActive(false); 
        playerMovement = FindObjectOfType<FirstPersonController>(); 
    }

    void Update()
    {
        
        if (!isUIOpen && Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactRange))
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

        
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
    }

    void CloseCCTVUI()
    {
        cctvUI.SetActive(false);
        isUIOpen = false;

        
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }
}
