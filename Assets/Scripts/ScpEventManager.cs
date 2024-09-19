using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScpEventManager : MonoBehaviour
{
    public static ScpEventManager Instance;
    private SCPTeleport sCPTeleport;
    
    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
