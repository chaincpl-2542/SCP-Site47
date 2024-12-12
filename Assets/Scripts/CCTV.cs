using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    [SerializeField] private GameObject  cctvCamera;
    [SerializeField] private GameObject cctvCameraNoise;
    [SerializeField] private float miniNoiseTime = 0.2f;
    [SerializeField] private float normalNoiseTime = 0.5f;
    private float noiseTimer;

    private void Start() 
    {
        TabletManager.Instance.forceNoise += ForceNoise;
        TabletManager.Instance.changeCamera += ChangeCameraNoise;
    }

    private void OnDisable() 
    {
        TabletManager.Instance.forceNoise -= ForceNoise;
        TabletManager.Instance.changeCamera -= ChangeCameraNoise;
    }

    private void Update() 
    {
        if(noiseTimer > 0)
        {
            noiseTimer -= Time.deltaTime;
            cctvCameraNoise.SetActive(true);
        }
        else
            cctvCameraNoise.SetActive(false);

    }

    public void ChangeCameraNoise()
    {
        noiseTimer = miniNoiseTime;
    }

    public void ForceNoise()
    {
        noiseTimer = normalNoiseTime;
    }

}
