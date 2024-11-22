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

    public void OnInteract()
    {
        LogManager.Instance.ShowLog(logType);
    }
}
