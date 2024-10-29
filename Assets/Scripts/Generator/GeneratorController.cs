using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour
{
    #region Value
    [SerializeField] private bool isActive;
    [SerializeField] private Animator generatorAnim;
    #endregion

    #region Reference
    public bool _isActive;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _isActive = isActive;
    }

    public void ActiveGenerator()
    {
        generatorAnim.SetBool("Active",true);
        isActive = true;
    }
}
