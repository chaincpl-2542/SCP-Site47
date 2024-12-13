using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuideTextController : MonoBehaviour
{
    public enum GuideEnum
    {
        None,
        PickupTablet,
        UseTablet,
        Sprint,
        CameraSCP
    }
    public GameObject guideText;

    public static GuideTextController Instance { get; private set; }

    // Called when the script instance is being loaded
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        Instance = this;
    }

    public void ShowGuide(GuideEnum guide)
    {
        guideText.SetActive(false);
        switch (guide)
        {
            case GuideEnum.PickupTablet:
                guideText.GetComponent<TextMeshProUGUI>().text = "I must pick up the tablet.";
                break;
            case GuideEnum.UseTablet:
                guideText.GetComponent<TextMeshProUGUI>().text = "Press 'Q' to use the tablet.";
                break;
            case GuideEnum.Sprint:
                guideText.GetComponent<TextMeshProUGUI>().text = "Hold 'W' and 'Shift' to sprint.";
                break;
            case GuideEnum.CameraSCP:
                guideText.GetComponent<TextMeshProUGUI>().text =
                    "I hear something, but I can’t see it. I should use the tablet to check.";
                break;
        }
        guideText.SetActive(true);
    }

    public void ForceShowDialogue(string dialogue)
    {
        guideText.SetActive(false);
        guideText.GetComponent<TextMeshProUGUI>().text = dialogue;
        guideText.SetActive(true);
    }
}
