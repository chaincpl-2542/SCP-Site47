using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public enum PlayerState
    {
        None,
        Log
    }
    
    public float interactRange = 2f;
    public LayerMask interactableLayer;
    public GameObject interactButton;
    public PlayerState playerState = PlayerState.None;
    public static PlayerInteract Instance { get; private set; }

    // Called when the script instance is being loaded
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        Instance = this;
    }

    private void Update()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange, interactableLayer))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                InteractWithObject();
            }

            if (playerState == PlayerState.None)
            {
                interactButton.GetComponentInChildren<TextMeshProUGUI>().text = $"E";
            }
            else if (playerState == PlayerState.Log)
            {
                interactButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Esc";
            }

            interactButton.SetActive(true);
        }
        else
        {
            interactButton.SetActive(false);
            playerState = PlayerInteract.PlayerState.None;
        }
    }

    void InteractWithObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange, interactableLayer))
        {
            Debug.Log(hit.transform.name);
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.OnInteract();
            }
        }
    }
}
