using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System;

public class AchievementManager : MonoBehaviour
{
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newJson = "{ \"Items\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.Items;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }

    public TMP_Text clearCountText;  // UI TextMeshPro element to display clearCount
    public TMP_Text playtimeText;    // UI TextMeshPro element to display playtime
    public long userNum;

    private void Start()
    {
        userNum = UserManager.Instance.UserNum;
        StartCoroutine(GetGameDataCoroutine());
    }

    private IEnumerator GetGameDataCoroutine()
    {
        string url = "http://localhost:8080/gamesavedata/user/" + userNum; // GET 요청 URL

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            string response = www.downloadHandler.text;
            Debug.Log("Server response: " + response); // 서버 응답 로그 출력
            GameData[] gameDataArray = JsonHelper.FromJson<GameData>(response); // JSON 응답이 배열인 경우
            if (gameDataArray != null && gameDataArray.Length > 0)
            {
                GameData gameData = gameDataArray[0]; // Assuming you want to get the first item
                UserManager.Instance.SetData_ID(gameData.id); // 데이터 저장할때 사용할 아이디 유저 매니저에 저장
                Debug.Log(gameData.id);
                clearCountText.text = "게임 클리어 횟수 : "+gameData.clearCount.ToString();
                playtimeText.text = "플레이타임 : "+ FormatPlaytime(gameData.playtime);
            }
            else
            {
                Debug.Log("데이터가 존재하지 않습니다.");
            }
        }
    }

    private string FormatPlaytime(int playtimeInSeconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(playtimeInSeconds);
        return $"{time.Hours}시간 {time.Minutes}분 {time.Seconds}초";
    }

    [System.Serializable]
    private class GameData
    {
        public bool success;
        public long id;
        public int clearCount;
        public int playtime;
    }
}
