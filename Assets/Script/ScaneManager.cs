using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    private const string MainSceneName = "MainScene";
    private const string GameSceneName = "GameScene";
    private const string MainMenuSceneName = "MainMenuScene";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void LoadMainScene()
    {
        LoadScene(MainSceneName);
    }

    public void LoadGameScene()
    {
        LoadScene(GameSceneName);
    }

    public void LoadMainMenuScene()
    {
        LoadScene(MainMenuSceneName);
    }

    private void LoadScene(string sceneName)
    {
      //  SceneManager.LoadScene(sceneName);
    }
}