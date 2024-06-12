using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TimerManager : MonoBehaviour
{
    private int secondsElapsed;
    private long gameDataId;
    private const string getPlayTimeUrl = "http://15.165.102.117:8080/gamesavedata/{id}/playtime";
    private const string updatePlayTimeUrl = "http://15.165.102.117:8080/gamesavedata/{id}/playtime";

    void Start()
    {
        secondsElapsed = 0;
        StartCoroutine(StartTimer());
        Debug.Log("Timer started.");
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

    public IEnumerator SavePlayTime()
    {
        gameDataId = UserManager.Instance.DataID;
        Debug.Log("Application is quitting. DataID: " + gameDataId + ", Elapsed seconds: " + secondsElapsed);

        string getUrl = getPlayTimeUrl.Replace("{id}", gameDataId.ToString());

        UnityWebRequest getRequest = UnityWebRequest.Get(getUrl);
        yield return getRequest.SendWebRequest();

        if (getRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error getting current PlayTime: " + getRequest.error);
            yield break;
        }

        if (!int.TryParse(getRequest.downloadHandler.text, out int currentPlayTime))
        {
            Debug.LogError("Error parsing current PlayTime: " + getRequest.downloadHandler.text);
            yield break;
        }
        Debug.Log("Current playTime from server: " + currentPlayTime);

        int newPlayTime = currentPlayTime + secondsElapsed;
        Debug.Log("New playTime to be updated: " + newPlayTime);

        string patchUrl = updatePlayTimeUrl.Replace("{id}", gameDataId.ToString());

        string jsonBody = "{\"playtime\":" + newPlayTime + "}";

        UnityWebRequest patchRequest = UnityWebRequest.Put(patchUrl, jsonBody);
        patchRequest.method = "PATCH";
        patchRequest.SetRequestHeader("Content-Type", "application/json");
        yield return patchRequest.SendWebRequest();

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
