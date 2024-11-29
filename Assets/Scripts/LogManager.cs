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

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            logParent.SetActive(false);
        }
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
    }
}
