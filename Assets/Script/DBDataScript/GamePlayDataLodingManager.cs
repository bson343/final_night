using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class GamePlayDataLodingManager : MonoBehaviour
{
    public static GamePlayDataLodingManager Instance { get; private set; }

    private int BasicHP = 50;
    private int BasicSP = 0;
    private int BasicGold = 99;
    private int BasicNewGamePaly = 0;
    private List<int> BasicCardDeck = new List<int> {1,2,3,4,5,6,13};
    private List<int> BasicHeroCardDeck = new List<int> { 1 };
    private const string BaseUrl = "http://localhost:8080/gamesavedata/user/";
    //private const string BaseUrl = "http://10.30.1.110:8080/gamesavedata/user/";

    string Url
    {
        get
        {
            return "http://" + ResourceManager.Instance.Config.DB_IP + ":8080" + "/gamesavedata/user/";
        }
    }

    public long userNumber;

    private bool isDataInitialized = false;

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // 씬 로드 이벤트에 메서드 등록
        NightSceneManager.Instance.SceneLoaded += OnSceneLoaded;
        Debug.Log("Scene loaded event registered.");
    }

    private void OnDestroy()
    {
        // 씬 로드 이벤트에서 메서드 등록 해제
        if (NightSceneManager.Instance != null)
        {
            NightSceneManager.Instance.SceneLoaded -= OnSceneLoaded;
            Debug.Log("Scene loaded event unregistered.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main") // 메인 씬 이름이 "Main"이라고 가정
        {
            Debug.Log("Main scene loaded.");
            userNumber = UserManager.Instance.UserNum; // UserManager에서 userNumber 가져옴
            Debug.Log($"User number: {userNumber}");

            // 초기화된 데이터를 다시 불러오지 않도록 조건 추가
            if (isDataInitialized)
            {
                Debug.Log("Initial data detected. Skipping data load.");
                return;
            }

            StartCoroutine(GetGameData(userNumber));
        }
    }

    public void InitializeData() // 데이터 초기화 시 값으로 사용된다.
    {
        isDataInitialized = true;
        UserManager.Instance.SetMaxHP(BasicHP);
        UserManager.Instance.SetCurrentHP(BasicHP);
        UserManager.Instance.SetCurrentSP(BasicSP);
        UserManager.Instance.SetGold(BasicGold);
        UserManager.Instance.SetNewGamePlay(BasicNewGamePaly);
        UserManager.Instance.SetMap(null);
        UserManager.Instance.SetCardDeckindex(BasicCardDeck);
        UserManager.Instance.SetHeroCardDeckindex(BasicHeroCardDeck);

        // 디버깅용 출력
        Debug.Log("Default HP: " + UserManager.Instance.MaxHP);
        Debug.Log("Default Gold: " + UserManager.Instance.Gold);
        Debug.Log("Default CardDeckIndex: " + string.Join(", ", UserManager.Instance.CardDeckIndex));
        Debug.Log("맵 데이터 없음");
    } 

    private IEnumerator GetGameData(long userNumber)
    {
        string url = Url + userNumber;
        Debug.Log($"Fetching game data from URL: {url}");
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log($"Game data fetched: {jsonResponse}");
            ProcessGameData(jsonResponse);
        }
        else
        {
            Debug.LogError($"Error fetching game data: {request.error}");
        }
    }

    private void ProcessGameData(string jsonResponse)
    {
        List<GameData> gameDataList = JsonConvert.DeserializeObject<List<GameData>>(jsonResponse);

        foreach (var gameData in gameDataList)
        {
            if (gameData.gameData == null)
            {
                Debug.Log("gameData is null for ID: " + gameData.Id);
                SetDefaultGameData();
            }
            else
            {
                GameDataContent gameDataContent = JsonConvert.DeserializeObject<GameDataContent>(gameData.gameData);
                if (gameDataContent.CurrentHP == 0)
                {
                    SetDefaultGameData();
                }
                else
                {
                    SetUserManagerData(gameDataContent);
                }
            }
        }
    }

    private void SetDefaultGameData()
    {
        UserManager.Instance.SetMaxHP(BasicHP);
        UserManager.Instance.SetCurrentHP(BasicHP);
        UserManager.Instance.SetCurrentSP(BasicSP);
        UserManager.Instance.SetNewGamePlay(BasicNewGamePaly);
        UserManager.Instance.SetGold(BasicGold);
        UserManager.Instance.SetMap(null);
        UserManager.Instance.SetCardDeckindex(BasicCardDeck);
        UserManager.Instance.SetHeroCardDeckindex(BasicHeroCardDeck);

        // 디버깅용 출력
        Debug.Log("Default HP: " + UserManager.Instance.MaxHP);
        Debug.Log("Default Gold: " + UserManager.Instance.Gold);
        Debug.Log("Default CardDeckIndex: " + string.Join(", ", UserManager.Instance.CardDeckIndex));
        Debug.Log("맵 데이터 없음");
    }

    private void SetUserManagerData(GameDataContent gameData)
    {
        UserManager.Instance.SetMaxHP(gameData.MaxHP);
        UserManager.Instance.SetCurrentHP(gameData.CurrentHP);
        UserManager.Instance.SetCurrentSP(gameData.CurrentSP);
        UserManager.Instance.SetNewGamePlay(gameData.NewGamePlay);
        UserManager.Instance.SetGold(gameData.Gold);
        UserManager.Instance.SetCardDeckindex(gameData.CardDeckIndex);
        UserManager.Instance.SetHeroCardDeckindex(gameData.HeroCardDeckIndex);
        UserManager.Instance.SetMap(gameData.Map);

        // 변수 값을 출력 (디버깅용)
        Debug.Log("HP: " + gameData.MaxHP);
        Debug.Log("Gold: " + gameData.Gold);
        Debug.Log("CardDeckIndex: " + string.Join(", ", gameData.CardDeckIndex));
        Debug.Log("HeroCardDeckIndex: " + string.Join(", ", gameData.HeroCardDeckIndex));
        Debug.Log("Map: " + JsonConvert.SerializeObject(gameData.Map, Formatting.Indented));
    }
}
