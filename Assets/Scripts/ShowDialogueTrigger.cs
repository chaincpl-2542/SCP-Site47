using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDialogueTrigger : MonoBehaviour
{
    public string message;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowMessage();
        }
    }

    public void ShowMessage()
    {
        GuideTextController.Instance.ForceShowDialogue(message);
        gameObject.SetActive(false);
    }
}
