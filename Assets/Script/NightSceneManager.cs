using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NightSceneManager : MonoBehaviour
{
    public static NightSceneManager Instance { get; private set; }
    string loadedSceneName;

    public List<string> eventScenes = new List<string>() { // 이벤트 씬 랜덤 리스트
        "EventScene1",
        "EventScene2",
        "EventScene3",
        "EventScene4",
        "EventScene5"

    };

    private void Awake() // 싱클톤 패턴 , 게임 오브젝트가 다음 씬에 가서도 안사라지게 설정
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

    public void LoadScene(string sceneName) // 씬의 이름을 입력받아서 씬을 불러온다 (로그인 , 메인화면때 사용)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }
    public void GameLoadScene(string sceneName) // 씬의 이름을 입력받아서 씬을 불러온다, (맵에서 다른 씬을 불러낼때 사용한다. 나갈수있어야하니까 창에서 추가하는 식으로)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += (AsyncOperation op) =>
        {
            loadedSceneName = sceneName;
            Debug.Log("현재 로드된 씬 이름: " + loadedSceneName);
        };
    }


    private IEnumerator LoadSceneAsync(string sceneName) // 씬의 비동기적 처리를 위해 사용 (더 빨라짐)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void LoadRandomScene() //랜덤 이벤트 씬 로드
    {
        int index = Random.Range(0, eventScenes.Count);
        string sceneToLoad = eventScenes[index];
        SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive).completed += (AsyncOperation op) =>
        {
            loadedSceneName = sceneToLoad;
            Debug.Log("현재 로드된 씬 이름: " + loadedSceneName);
        };
    }

    public void UnloadScene() // 씬 언로딩 (씬을 계속 불러오면 성능이 안좋아짐, 그래서 꺼줘야한다.)
    {
        if (!string.IsNullOrEmpty(loadedSceneName))
        {
            Debug.Log("언로드 씬 이름: " + loadedSceneName);
            SceneManager.UnloadSceneAsync(loadedSceneName);
        }
    }

    private IEnumerator UnloadSceneAsync(string sceneName)
    {
        AsyncOperation asyncUnload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);

        while (!asyncUnload.isDone)
        {
            yield return null;
        }
    }
}