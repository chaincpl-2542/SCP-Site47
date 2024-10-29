using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorSwitch : MonoBehaviour
{
    private GeneratorController generatorController;
    public void ActiveSwitch()
    {
        generatorController.ActiveGenerator();
    }
}
