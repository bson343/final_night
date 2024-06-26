using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class TimerManager : MonoBehaviour
{
    private int secondsElapsed;
    private long gameDataId;
    private const string getPlayTimeUrl = "http://localhost:8080/gamesavedata/{id}/playtime";
    private const string updatePlayTimeUrl = "http://localhost:8080/gamesavedata/{id}/playtime";
    private static TimerManager instance;
    public static TimerManager Instance => instance;
    
    

    void Start()
    {   
        StartCoroutine(StartTimer());
        Debug.Log("Timer started.");
    }

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


    private IEnumerator StartTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            secondsElapsed++;
            Debug.Log("Seconds elapsed: " + secondsElapsed);
        }
    }

    public void SavePlayTimeSync()
    {
        gameDataId = UserManager.Instance.DataID;
        Debug.Log("Application is quitting. DataID: " + gameDataId + ", Elapsed seconds: " + secondsElapsed);

        string getUrl = getPlayTimeUrl.Replace("{id}", gameDataId.ToString());

        UnityWebRequest getRequest = UnityWebRequest.Get(getUrl);
        var getOperation = getRequest.SendWebRequest();

        while (!getOperation.isDone)
        {
            // 동기적으로 완료될 때까지 대기
        }

        if (getRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error getting current PlayTime: " + getRequest.error);
            return;
        }

        if (!int.TryParse(getRequest.downloadHandler.text, out int currentPlayTime))
        {
            Debug.LogError("Error parsing current PlayTime: " + getRequest.downloadHandler.text);
            return;
        }
        Debug.Log("Current playTime from server: " + currentPlayTime);

        int newPlayTime = currentPlayTime + secondsElapsed;
        Debug.Log("New playTime to be updated: " + newPlayTime);

        string patchUrl = updatePlayTimeUrl.Replace("{id}", gameDataId.ToString());

        string jsonBody = "{\"playtime\":" + newPlayTime + "}";

        UnityWebRequest patchRequest = new UnityWebRequest(patchUrl, "PATCH");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        patchRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        patchRequest.downloadHandler = new DownloadHandlerBuffer();
        patchRequest.SetRequestHeader("Content-Type", "application/json");

        var patchOperation = patchRequest.SendWebRequest();

        while (!patchOperation.isDone)
        {
            // 동기적으로 완료될 때까지 대기
        }

        if (patchRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("PlayTime updated successfully to " + newPlayTime);
        }
        else
        {
            Debug.LogError("Error updating PlayTime: " + patchRequest.error);
        }
    }


    public void SetSecondsElapsed(int time)
    {
        secondsElapsed = time;
    }
}
