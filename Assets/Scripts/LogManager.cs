using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance;

    public GameObject logParent;
    public Image imageLog;

    public Sprite logStart;
    public Sprite logTablet;
    public Sprite logDetection;
    public Sprite logFormless;
    public Sprite logTeleportation;
    public Sprite logWarningAggression;
    public Sprite logWarningAttack;
    public Sprite logWeapon;
    public Sprite logJammer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public void ShowLog(LogInteract.LogType logType)
    {
        logParent.SetActive(true);
        PlayerInteract.Instance.playerState = PlayerInteract.PlayerState.Log;
        if(logType == LogInteract.LogType.LogStart)
        {
            imageLog.sprite = logStart;
        }
        else if(logType == LogInteract.LogType.LogTablet)
        {
            imageLog.sprite = logTablet;
        }
        else if(logType == LogInteract.LogType.LogTeleportation)
        {
            imageLog.sprite = logTeleportation;
        }
        else if(logType == LogInteract.LogType.LogDetection)
        {
            imageLog.sprite = logDetection;
        }
        else if(logType == LogInteract.LogType.LogFormless)
        {
            imageLog.sprite = logFormless;
        }
        else if(logType == LogInteract.LogType.LogWarningAggression)
        {
            imageLog.sprite = logWarningAggression;
        }
        else if(logType == LogInteract.LogType.LogWarningAttack)
        {
            imageLog.sprite = logWarningAttack;
        }
        else if(logType == LogInteract.LogType.LogWeapon)
        {
            imageLog.sprite = logWeapon;
        }
        else if(logType == LogInteract.LogType.LogJammer)
        {
            imageLog.sprite = logJammer;
        }

    }

    public void CloseLog()
    {
        logParent.SetActive(false);
        PlayerInteract.Instance.playerState = PlayerInteract.PlayerState.Default;
    }

    public bool IsLogOpen()
    {
        return logParent.activeSelf;
    }
}
