using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataComputer : MonoBehaviour, IInteractable
{
    public EventControl eventControl;
    public Slider loadingBar;

    public List<GameObject> screenList;
    public bool isDownload;
    public TextMeshProUGUI downloadText;

    float downloadValue = 0f;
    bool check = false;
    
    void Update()
    {
        if(isDownload)
        {
            downloadValue += Time.deltaTime * 5;
            loadingBar.value = downloadValue/100;
            if(downloadValue >= 100)
            {
                downloadValue = 100;
                if(!check)
                {
                    ActiveDataRoom();
                    check = true;
                }
            }
            downloadText.text = Mathf.Round(downloadValue).ToString() + " %";
        }
    }
    public void ActiveDataRoom()
    {
        for(int i = 0; i < screenList.Count; i++)
        {
            screenList[i].SetActive(false);
        }
        eventControl.isDataRoom = true;
        eventControl.ActiveEvent(4);
        EventControl.Instance.ActiveScpTrigger();
    }

     public void OnInteract()
    {
        eventControl.HideSCP();
        isDownload = true;
    }
}
