using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneSwitcher : MonoBehaviour
{
    public string sceneName;
    public string targetScene;

    private void InitializeData()
    {
        GamePlayDataLodingManager dataLodingManager = FindObjectOfType<GamePlayDataLodingManager>();
        if (dataLodingManager != null)
        {
            dataLodingManager.InitializeData();
        }
        else
        {
            Debug.LogError("GamePlayDataLodingManager not found!");
        }
    }

    public void SwitchScene()
    {
        InitializeData();
        Debug.Log("Scene switching to Main.");
        NightSceneManager.Instance.LoadScene("Main");
    }
}
