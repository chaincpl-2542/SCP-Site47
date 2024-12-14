using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExitDoor : MonoBehaviour, IInteractable
{
    public int requiredAccessLevel = 5; // Access level required to open the door
    public AutoDoor doorExit; // Animator for the door opening/closing
    public AudioSource accessDeniedSound; // Sound to play when access is denied
    public AudioSource accessGrantedSound; // Sound to play when the door opens

    public Image _imagePowerButton;
    public TextMeshProUGUI _textAccess;

    private bool isDoorOpen = false; // Tracks the state of the door
    private bool check;

    public void OnInteract()
    {
        // Check player's access level
        int playerAccessLevel = TabletManager.Instance.GetPlayerAccessLevel();

        if (playerAccessLevel >= requiredAccessLevel)
        {
            _imagePowerButton.color = Color.green;
            _textAccess.text = "Access granted";
            _textAccess.color = Color.green;
            if (accessGrantedSound != null)
            {
                accessGrantedSound.Play();
            }
            
            // Open or close the door
            OpenDoor();
        }
        else
        {
            // Play access denied sound
            if (accessDeniedSound != null)
            {
                accessDeniedSound.Play();
            }

            if (!check)
            {
                GuideTextController.Instance.ForceShowDialogue("Looks like I'll need access level 5 to open the exit door.");
                check = true;
            }

            Debug.Log($"Access denied. Required level: {requiredAccessLevel}, Player level: {playerAccessLevel}");
        }
    }

    private void OpenDoor()
    {
        doorExit.DoorStatus(true);
        doorExit.DoorForceOpen();
    }
}

