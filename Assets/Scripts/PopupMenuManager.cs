using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PopupMenuManager : MonoBehaviour
{
    public GameObject popupMenu;
    public bool isPause = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPause)
            {
                MenuOpen();
            }
            else
            {
                MenuClose();
            }
        }
        if (isPause)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                backToMainMenu();
            }
        }

    }

    public void MenuOpen()
    {
        isPause = true;
        popupMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void MenuClose()
    {
        Time.timeScale = 1;
        isPause = false;
        popupMenu.SetActive(false);
    }

    public void backToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
