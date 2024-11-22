using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogInteract : MonoBehaviour, IInteractable
{
    public enum LogType
{
    LogStart,
    LogTablet
    
}
    public LogType logType;
    public AudioSource audioSource;

    public void OnInteract()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }

        LogManager.Instance.ShowLog(logType);
    }
}
