using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorSwitch : MonoBehaviour, IInteractable
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    [SerializeField] private GeneratorController generatorController;
    public void OnInteract()
    {
        animator.SetBool("On",true);
        generatorController.ActiveGenerator();
    }
}
