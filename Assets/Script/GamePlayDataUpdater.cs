using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GamePlayDataUpdater : MonoBehaviour
{
    public static GamePlayDataUpdater Instance { get; private set; }
    private string baseUrl = "http://localhost:8080/gamesavedata";

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

    public IEnumerator SaveGameData()
    {
        long gameId = UserManager.Instance.DataID; // 업데이트할 게임 데이터의 ID
        GameDataContent newGameData = new GameDataContent
        {
            MaxHP = UserManager.Instance.MaxHP,
            CurrentHP = UserManager.Instance.CurrentHP,
            Gold = UserManager.Instance.Gold,
            CardDeckIndex = UserManager.Instance.CardDeckIndex,
            HeroCardDeckIndex = UserManager.Instance.HeroCardDeckIndex,
            Map = UserManager.Instance.Map,
        };

        yield return StartCoroutine(UpdateGameDataCoroutine(gameId, newGameData));
    }

    private IEnumerator UpdateGameDataCoroutine(long id, GameDataContent newGameData)
    {
        string url = $"{baseUrl}/{id}/gamedata";

        string jsonGameData = JsonUtility.ToJson(newGameData);
        Dictionary<string, string> updateData = new Dictionary<string, string>
        {
            { "gameData", jsonGameData }
        };
        string jsonBody = JsonUtility.ToJson(updateData);

        UnityWebRequest request = new UnityWebRequest(url, "PATCH");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

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
