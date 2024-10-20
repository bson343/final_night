using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Newtonsoft.Json;

public class GamePlayDataUpdater : MonoBehaviour
{
    //private string baseUrl = "http://localhost:8080/gamesavedata";

    private string baseUrl;

    private static GamePlayDataUpdater instance;
    public static GamePlayDataUpdater Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        baseUrl = "http://" + ResourceManager.Instance.Config.DB_IP + ":8080" + "/gamesavedata";
    }

    public void SaveGameDataSync()
    {
        long gameId = UserManager.Instance.DataID;
        GameDataContent newGameData = new GameDataContent
        {
            MaxHP = UserManager.Instance.MaxHP,
            CurrentHP = UserManager.Instance.CurrentHP,
            Gold = UserManager.Instance.Gold,
            NewGamePlay = UserManager.Instance.NewGamePlay,
            CardDeckIndex = UserManager.Instance.CardDeckIndex,
            HeroCardDeckIndex = UserManager.Instance.HeroCardDeckIndex,
            Map = UserManager.Instance.Map,
        };
        
        string url = $"{baseUrl}/{gameId}/gamedata";

        string jsonGameData = JsonConvert.SerializeObject(newGameData);
        var updateData = new Dictionary<string, string>
        {
            { "gameData", jsonGameData }
        };
        string jsonBody = JsonConvert.SerializeObject(updateData);

        Debug.Log("Request URL: " + url);
        Debug.Log("Request Body: " + jsonBody);

        using (UnityWebRequest request = new UnityWebRequest(url, "PATCH"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                // 동기적으로 완료될 때까지 대기
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("GameData updated successfully!");
            }
            else
            {
                Debug.LogError($"Error updating GameData: {request.error}");
            }
        }
    }

}
