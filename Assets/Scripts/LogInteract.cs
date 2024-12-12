using UnityEngine;

public class LogInteract : MonoBehaviour, IInteractable
{
    public enum LogType
{
    LogStart,
    LogTablet,
    LogDetection,
    LogFormless,
    LogTeleportation,
    LogWarningAttack,
    LogWarningAggression,
    LogWeapon
    
}
    public LogType logType;
    public AudioSource audioSource;
    public GameObject highlight;

    public void OnInteract()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }

        LogManager.Instance.ShowLog(logType);
        highlight.SetActive(false);
    }
}
