using UnityEngine;
using System.Collections;

public class GeneratorRoomSCPEvent : MonoBehaviour
{
    public Transform scpTransform;
    public float lookDuration = 5f; 
    public float teleportDistance = 0.5f; 
    public float scpVisibleDuration = 0.5f; 
    public Vector3 teleportOffset = new Vector3(0, -1f, 0);
    public GameObject playerCamera;
    public AudioSource jumpScareSound;
    public AudioSource glitchSound;
    private Transform playerTransform; 

    private bool eventTriggered = false;

    void OnTriggerStay(Collider other)
    {
        if (eventTriggered) return; // Ensure the event triggers only once
        if (other.CompareTag("Player") && TabletManager.Instance.isTablet)
        {
            Debug.Log("SCP Event");
            eventTriggered = true;
            playerTransform = other.transform;

            // Start the SCP event
            SimpleFirstPersonController playerController = other.transform.root.GetComponent<SimpleFirstPersonController>();
            if (playerController != null)
            {
                Debug.Log("Start SCP Event");
                StartCoroutine(HandleSCPEvent(playerController));
            }
        }
    }

    private IEnumerator HandleSCPEvent(SimpleFirstPersonController playerController)
    {
        // Disable player movement and controls
        playerController.disablePlayerControll = true;

        // Disable tablet interaction
        TabletManager.Instance.isDisableTablet = true;

        // Force the player to look at the SCP
        playerController.StartSmoothLookAt(scpTransform);
        
        Vector3 directionToPlayer = playerTransform.position - scpTransform.transform.position;
        scpTransform.rotation = Quaternion.LookRotation(directionToPlayer);
        
        yield return new WaitForSeconds(lookDuration);

        // Teleport the SCP in front of the player
        Vector3 teleportPosition = playerCamera.transform.position + playerCamera.transform.forward * teleportDistance+ teleportOffset;
        scpTransform.transform.root.position = teleportPosition;
        glitchSound.Play();
        jumpScareSound.Play();
        StartCoroutine(SCPAlwaysLookAtPlayer());

        // Wait for the SCP to remain visible
        yield return new WaitForSeconds(scpVisibleDuration);
        StopCoroutine(SCPAlwaysLookAtPlayer());

        // Hide the SCP (you can set active to false or teleport it elsewhere)
        scpTransform.transform.root.position = new Vector3(1000, 1000, 1000);// Move SCP far away or disable its renderer
        glitchSound.Play();
        // Alternatively: scpTransform.gameObject.SetActive(false);
        
        // Re-enable tablet interaction
        TabletManager.Instance.isDisableTablet = false;

        // Re-enable player movement and controls
        playerController.disablePlayerControll = false;
        
        yield return new WaitForSeconds(2);
        scpTransform.transform.root.gameObject.SetActive(false);
    }
    private IEnumerator SCPAlwaysLookAtPlayer()
    {
        while (true)
        {
            if (playerTransform != null)
            {
                // Make SCP look at the player
                scpTransform.LookAt(playerTransform);
            }
            yield return null; // Wait for the next frame
        }
    }
}
