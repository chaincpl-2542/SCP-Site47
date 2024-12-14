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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RestartGame()
    {
        StartCoroutine(RestartGame(3));
    }

    public IEnumerator DelayLoadscene(int num,float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(num);
    }
    public IEnumerator RestartGame(float num)
    {
        yield return new WaitForSeconds(num);
        LoadGame();
    }

    public void EndGame()
    {
        StartCoroutine(DelayLoadscene(2,2));
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
