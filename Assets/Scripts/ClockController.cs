using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ClockController : MonoBehaviour
{
    [SerializeField] private TMP_Text clockText;
    private float elaspedTime;
    [SerializeField] private float timeInADay = 86400f;
    [SerializeField] private float timeScale = 60.0f;
    // Start is called before the first frame update
    void Start()
    {
        elaspedTime = 20 * 3600f;
    }

    // Update is called once per frame
    void Update()
    {
        elaspedTime += Time.deltaTime * timeScale;
        elaspedTime %= timeInADay;
        UpdateClockUI();
        
    }

    void UpdateClockUI()
    {
        int hours = Mathf.FloorToInt(elaspedTime / 3600f);
        int minutes = Mathf.FloorToInt((elaspedTime - hours * 3600f) / 60f);

        string clockString = string.Format("{0:00}:{1:00}",hours,minutes);
        clockText.text = clockString+ " PM";
    }
}
