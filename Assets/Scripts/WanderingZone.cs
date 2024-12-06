using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingZone : MonoBehaviour
{
    public List<Transform> wanderingPositions;
    private void Awake()
    {
        // Get all child Transforms and add them to the wanderingPositions list
        wanderingPositions = new List<Transform>();
        foreach (Transform child in transform)
        {
            wanderingPositions.Add(child);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SCPController scpController = FindObjectOfType<SCPController>();
            if (scpController != null && wanderingPositions.Count > 0)
            {
                // Check if SCP is chasing the player
                if (scpController.playerInSight || scpController.playerHeard)
                {
                    Debug.Log("SCP is chasing the player, skipping teleportation.");
                    return; // Don't teleport SCP if it is chasing
                }

                // Update SCP wandering positions
                scpController.wanderingPositions = new List<Transform>(wanderingPositions);

                // Teleport SCP to a random position within the new zone
                Transform randomPosition = wanderingPositions[Random.Range(0, wanderingPositions.Count)];
                scpController.agent.Warp(randomPosition.position);
            }
        }
    }
}

