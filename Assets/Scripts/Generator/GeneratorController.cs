using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour
{   
    public static GeneratorController Instance;  
    [SerializeField] private EventControl eventControl;
    public bool isActive;
    [SerializeField] private Animator generatorAnim;
    public List<GameObject> activeList;
    public void ActiveGenerator()
    {
        generatorAnim.SetBool("Active",true);
        isActive = true;
        eventControl.isGeneratorRoom = true;
        StartCoroutine( ActiveObjects());
        eventControl.ActiveEvent(1);
    }

    public IEnumerator ActiveObjects()
    {
        yield return new WaitForSeconds(2);

        if(activeList.Count > 0)
        {
            for (int i = 0; i < activeList.Count; i++)
            {
                activeList[i].SetActive(true);
            }
        }
    }
}
