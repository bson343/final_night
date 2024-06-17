using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NightSceneManager : MonoBehaviour
{
    public static NightSceneManager Instance { get; private set; }
    string loadedSceneName;

    public delegate void SceneLoadedHandler(Scene scene, LoadSceneMode mode);
    public event SceneLoadedHandler SceneLoaded;

    public List<string> eventScenes = new List<string>() {
        "EventScene1",
        "EventScene2",
        "EventScene3",
        "EventScene4",
        "EventScene5"
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void GameLoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += (AsyncOperation op) =>
        {
            loadedSceneName = sceneName;
            Debug.Log("현재 로드된 씬 이름: " + loadedSceneName);
            SceneLoaded?.Invoke(SceneManager.GetSceneByName(sceneName), LoadSceneMode.Additive);
        };
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        loadedSceneName = sceneName;
        SceneLoaded?.Invoke(SceneManager.GetSceneByName(sceneName), LoadSceneMode.Single);
    }

    public void LoadRandomScene()
    {
        int index = Random.Range(0, eventScenes.Count);
        string sceneToLoad = eventScenes[index];
        SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive).completed += (AsyncOperation op) =>
        {
            loadedSceneName = sceneToLoad;
            Debug.Log("현재 로드된 씬 이름: " + loadedSceneName);
            SceneLoaded?.Invoke(SceneManager.GetSceneByName(sceneToLoad), LoadSceneMode.Additive);
        };
    }

    public void UnloadScene()
    {
        if (!string.IsNullOrEmpty(loadedSceneName))
        {
            Debug.Log("언로드 씬 이름: " + loadedSceneName);
            GlobalSoundManager.Instance.FadeBGM(EBGM.Menu);
            SceneManager.UnloadSceneAsync(loadedSceneName);
        }
    }

    private IEnumerator UnloadSceneAsync(string sceneName)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

        while (!asyncUnload.isDone)
        {
            yield return null;
        }
    }
}
