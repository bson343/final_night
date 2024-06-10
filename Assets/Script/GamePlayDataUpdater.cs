using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;

public class GamePlayDataUpdater : MonoBehaviour
{
    private const string BaseUrl = "http://localhost:8080/gamesavedata/";

    private bool isQuitRequested;

    private void OnEnable()
    {
        Application.wantsToQuit += WantsToQuit;
    }

    private void OnDisable()
    {
        Application.wantsToQuit -= WantsToQuit;
    }

    private bool WantsToQuit()
    {
        // 게임 종료 요청을 처리하기 위해 false를 반환
        isQuitRequested = true;

        // 게임 데이터를 서버로 전송
        GameData gameData = new GameData
        {
            HP = UserManager.Instance.HP,
            Gold = UserManager.Instance.Gold,
            CardDeckIndex = UserManager.Instance.CardDeckIndex,
            HeroCardDeckIndex = UserManager.Instance.HeroCardDeckIndex,
            Map = UserManager.Instance.Map,
        };

        // 게임 데이터 서버로 전송
        long gameId = UserManager.Instance.DataID; // 업데이트할 게임 데이터의 ID
        StartCoroutine(UpdateGameDataAndQuit(gameId, gameData));

        return false; // 종료를 일시 중단
    }

    private IEnumerator UpdateGameDataAndQuit(long id, GameData gameData)
    {
        yield return PatchGameData(id, gameData);

        // 게임 종료 작업 수행
        isQuitRequested = false;
        Application.Quit();
    }

    public IEnumerator PatchGameData(long id, GameData gameData)
    {
        string url = BaseUrl + id;
        string jsonData = JsonConvert.SerializeObject(gameData); // 직렬화 수정

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "PATCH");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("GameData update complete! Status Code: " + request.responseCode);
            Debug.Log("Sent data: " + jsonData); // 전송된 데이터 로그
        }
    }
}
