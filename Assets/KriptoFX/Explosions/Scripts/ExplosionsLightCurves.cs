using UnityEngine;
using System.Collections;

public class ExplosionsLightCurves : MonoBehaviour
{
    public AnimationCurve LightCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float GraphTimeMultiplier = 1, GraphIntensityMultiplier = 1;

    private bool canUpdate;
    private float startTime;
    private Light lightSource;

    private void Awake()
    {
        lightSource = GetComponent<Light>();
        lightSource.intensity = LightCurve.Evaluate(0);
    }

    private void OnEnable()
    {
        startTime = Time.time;
        canUpdate = true;
    }

    private void Update()
    {
        var time = Time.time - startTime;
        if (canUpdate) {
            var eval = LightCurve.Evaluate(time / GraphTimeMultiplier) * GraphIntensityMultiplier;
            lightSource.intensity = eval;
        }
        if (time >= GraphTimeMultiplier)
            canUpdate = false;
    }
}