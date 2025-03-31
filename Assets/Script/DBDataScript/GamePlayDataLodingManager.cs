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
    private int BasicGold = 99;
    private int BasicNewGamePaly = 0;
    private List<int> BasicCardDeck = new List<int> {1,2,3,4,5,6,10,14};
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
        // �̱��� ���� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // �� �ε� �̺�Ʈ�� �޼��� ���
        NightSceneManager.Instance.SceneLoaded += OnSceneLoaded;
        Debug.Log("Scene loaded event registered.");
    }

    private void OnDestroy()
    {
        // �� �ε� �̺�Ʈ���� �޼��� ��� ����
        if (NightSceneManager.Instance != null)
        {
            NightSceneManager.Instance.SceneLoaded -= OnSceneLoaded;
            Debug.Log("Scene loaded event unregistered.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //web���� ���� ��
        {
            if (isDataInitialized)
            {
                Debug.Log("Initial data detected. Skipping data load.");
                return;
            }
            else
            {
                InitializeData();
            }

            return;
        }

        if (scene.name == "Main") // ���� �� �̸��� "Main"�̶�� ����
        {
            Debug.Log("Main scene loaded.");
            userNumber = UserManager.Instance.UserNum; // UserManager���� userNumber ������
            Debug.Log($"User number: {userNumber}");

            // �ʱ�ȭ�� �����͸� �ٽ� �ҷ����� �ʵ��� ���� �߰�
            if (isDataInitialized)
            {
                Debug.Log("Initial data detected. Skipping data load.");
                return;
            }

            StartCoroutine(GetGameData(userNumber));
        }
    }

    public void InitializeData() // ������ �ʱ�ȭ �� ������ ���ȴ�.
    {
        isDataInitialized = true;
        UserManager.Instance.SetMaxHP(BasicHP);
        UserManager.Instance.SetCurrentHP(BasicHP);
        UserManager.Instance.SetGold(BasicGold);
        UserManager.Instance.SetNewGamePlay(BasicNewGamePaly);
        UserManager.Instance.SetMap(null);
        UserManager.Instance.SetCardDeckindex(BasicCardDeck);
        UserManager.Instance.SetHeroCardDeckindex(BasicHeroCardDeck);

        // ������ ���
        Debug.Log("Default HP: " + UserManager.Instance.MaxHP);
        Debug.Log("Default Gold: " + UserManager.Instance.Gold);
        Debug.Log("Default CardDeckIndex: " + string.Join(", ", UserManager.Instance.CardDeckIndex));
        Debug.Log("�� ������ ����");
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
        UserManager.Instance.SetNewGamePlay(BasicNewGamePaly);
        UserManager.Instance.SetGold(BasicGold);
        UserManager.Instance.SetMap(null);
        UserManager.Instance.SetCardDeckindex(BasicCardDeck);
        UserManager.Instance.SetHeroCardDeckindex(BasicHeroCardDeck);

        // ������ ���
        Debug.Log("Default HP: " + UserManager.Instance.MaxHP);
        Debug.Log("Default Gold: " + UserManager.Instance.Gold);
        Debug.Log("Default CardDeckIndex: " + string.Join(", ", UserManager.Instance.CardDeckIndex));
        Debug.Log("�� ������ ����");
    }

    private void SetUserManagerData(GameDataContent gameData)
    {
        UserManager.Instance.SetMaxHP(gameData.MaxHP);
        UserManager.Instance.SetCurrentHP(gameData.CurrentHP);
        UserManager.Instance.SetNewGamePlay(gameData.NewGamePlay);
        UserManager.Instance.SetGold(gameData.Gold);
        UserManager.Instance.SetCardDeckindex(gameData.CardDeckIndex);
        UserManager.Instance.SetHeroCardDeckindex(gameData.HeroCardDeckIndex);
        UserManager.Instance.SetMap(gameData.Map);

        // ���� ���� ��� (������)
        Debug.Log("HP: " + gameData.MaxHP);
        Debug.Log("Gold: " + gameData.Gold);
        Debug.Log("CardDeckIndex: " + string.Join(", ", gameData.CardDeckIndex));
        Debug.Log("HeroCardDeckIndex: " + string.Join(", ", gameData.HeroCardDeckIndex));
        Debug.Log("Map: " + JsonConvert.SerializeObject(gameData.Map, Formatting.Indented));
    }
}
