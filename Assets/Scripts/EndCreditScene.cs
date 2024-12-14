using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCreditScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WiatForEndCredit());
    }

    private IEnumerator WiatForEndCredit()
    {
        yield return new WaitForSeconds(36f);
        SceneManager.LoadScene(0);
    }
}
