using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ES3Internal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadGame();
        }
    }

    public void RestartGame()
    {
        StartCoroutine(DelayLoadscene(1,3));
    }

    public IEnumerator DelayLoadscene(int num,float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(num);
    }

    public void EndGame()
    {
        StartCoroutine(DelayLoadscene(0,0));
    }

    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void SaveGame()
    {
        ES3AutoSaveMgr.Current.Save();
        Debug.Log("SaveGame");
    }

    public void LoadGame()
    {
        ES3AutoSaveMgr.Current.Load();
        Debug.Log("LoadGame");
    }
}
