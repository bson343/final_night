using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Newtonsoft.Json;

public class GamePlayDataUpdater : MonoBehaviour
{
    private string baseUrl = "http://15.165.102.117:8080/gamesavedata";

    public void OnClickSave()
    {
        StartCoroutine(SaveGameData());
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

        string jsonGameData = JsonConvert.SerializeObject(newGameData);
        var updateData = new Dictionary<string, string>
        {
            { "gameData", jsonGameData }
        };
        string jsonBody = JsonConvert.SerializeObject(updateData);

        Debug.Log("Request URL: " + url);
        Debug.Log("Request Body: " + jsonBody);

        UnityWebRequest request = new UnityWebRequest(url, "PATCH");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
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
