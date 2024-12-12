using UnityEngine;

public class JammerInteract : MonoBehaviour, IInteractable
{
    public AudioSource pickupSound; // Optional: Add a pickup sound
    public GameObject highlight;   // Optional: Highlight effect for the Jammer

    public void OnInteract()
    {
        // Play pickup sound if available
        if (pickupSound != null)
        {
            pickupSound.Play();
        }

        // Enable Jammer in TabletManager
        TabletManager tabletManager = TabletManager.Instance;
        if (tabletManager != null)
        {
            tabletManager.PickJammer();
            gameObject.SetActive(false);
        }

        // Disable highlight and destroy the Jammer object
        if (highlight != null)
        {
            highlight.SetActive(false);
        }

        Destroy(gameObject); // Remove the Jammer object from the scene
        Debug.Log("Jammer picked up!");
    }
}
