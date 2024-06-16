using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class GamePlayDataLodingManager : MonoBehaviour
{
    private int BasicHP = 50;
    private int BasicSP = 0;
    private int BasicGold = 99;
    private List<int> BasicCardDeck = new List<int> { 1, 1, 1, 1, 1, 2, 2, 2, 2, 3 };
    private List<int> BasicHeroCardDeck = new List<int> {1 };
    private const string BaseUrl = "http://15.165.102.117:8080/gamesavedata/user/";
    // userNumber를 저장할 변수
    public long userNumber;

    // Start is called before the first frame update
    void Start()
    {
        // 여기서 userNumber를 설정할 수 있습니다.
        userNumber = UserManager.Instance.UserNum; // UserManager에서 userNumber 가져옴

        StartCoroutine(GetGameData(userNumber));
    }

    private IEnumerator GetGameData(long userNumber)
    {
        string url = BaseUrl + userNumber;
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
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
                if(gameDataContent.CurrentHP == 0)
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
            UserManager.Instance.SetGold(BasicGold);
            UserManager.Instance.SetCardDeckindex(BasicCardDeck);
            UserManager.Instance.SetHeroCardDeckindex(BasicHeroCardDeck);


            // 디버깅용 출력
            Debug.Log("Default HP: " + UserManager.Instance.MaxHP);
            Debug.Log("Default Gold: " + UserManager.Instance.Gold);
            Debug.Log("Default CardDeckIndex: " + UserManager.Instance.CardDeckIndex);
            Debug.Log("맵 데이터 없음");
    }

        private void SetUserManagerData(GameDataContent gameData)
        {
            UserManager.Instance.SetMaxHP(gameData.MaxHP);
            UserManager.Instance.SetCurrentHP(gameData.CurrentHP);
            UserManager.Instance.SetCurrentHP(gameData.CurrentSP);
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



