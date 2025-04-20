using UnityEngine;

public class LightingController : MonoBehaviour
{
    public Material newSkybox; // Skybox ใหม่ (ใส่ None ถ้าอยากลบ)
    public float newAmbientIntensity = 0f;

    public void ApplyLightingSettings()
    {
        RenderSettings.skybox = newSkybox; // ใส่ null ถ้าจะลบ skybox
        RenderSettings.ambientIntensity = newAmbientIntensity; // ควบคุมความสว่างโดยรวม
        DynamicGI.UpdateEnvironment(); // อัปเดต GI ใหม่
    }
}
