using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TimerManager : MonoBehaviour
{
    private int secondsElapsed;
    private long gameDataId;
    private const string getPlayTimeUrl = "http://localhost:8080/gamesavedata/{id}/playtime";
    private const string updatePlayTimeUrl = "http://localhost:8080/gamesavedata/{id}/playtime";

    void Start()
    {
        secondsElapsed = 0;
        StartCoroutine(StartTimer());
        Debug.Log("Timer started.");
    }

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
        gameDataId = UserManager.Instance.DataID;
        Debug.Log("Application is quitting. DataID: " + gameDataId + ", Elapsed seconds: " + secondsElapsed);
        UpdatePlayTimeSync(gameDataId, secondsElapsed);
        return true; // Return true to allow quitting
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

    private void UpdatePlayTimeSync(long id, int additionalPlayTime)
    {
        Debug.Log("Starting UpdatePlayTime with ID: " + id + " and additional playtime: " + additionalPlayTime);

        string getUrl = getPlayTimeUrl.Replace("{id}", id.ToString());

        UnityWebRequest getRequest = UnityWebRequest.Get(getUrl);
        getRequest.SendWebRequest();

        while (!getRequest.isDone) { }

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

        int newPlayTime = currentPlayTime + additionalPlayTime;
        Debug.Log("New playTime to be updated: " + newPlayTime);

        string patchUrl = updatePlayTimeUrl.Replace("{id}", id.ToString());

        string jsonBody = "{\"playtime\":" + newPlayTime + "}";

        UnityWebRequest patchRequest = UnityWebRequest.Put(patchUrl, jsonBody);
        patchRequest.method = "PATCH";
        patchRequest.SetRequestHeader("Content-Type", "application/json");
        patchRequest.SendWebRequest();

        while (!patchRequest.isDone) { }

        if (patchRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("PlayTime updated successfully to " + newPlayTime);
        }
        else
        {
            Debug.LogError("Error updating PlayTime: " + patchRequest.error);
        }
    }
}
