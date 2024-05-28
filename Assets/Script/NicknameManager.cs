using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using TMPro;
using UnityEngine;
using System;



public class NicknameManager : MonoBehaviour
{
    public TMP_Text nicknameText;
    public long userNum;
    private void Start()
    {
            userNum = UserManager.Instance.UserNum;
        StartCoroutine(GetNicknameCoroutine());
    }

    private IEnumerator GetNicknameCoroutine()
    {
        string url = "http://localhost:8080/api/nickname?userNum=" + userNum; // GET 요청으로 변경

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
            NicknameResponse nicknameResponse = JsonUtility.FromJson<NicknameResponse>(response);
            if (nicknameResponse.success)
            {
                UserManager.Instance.SetUserNickname(nicknameResponse.nickname); // UserManager를 통해 userNickname 설정
                nicknameText.text = nicknameResponse.nickname;
            }
            else
            {
                Debug.Log("닉네임의 값이 존재하지 않습니다.");
            }
        }
    }

    [System.Serializable]
    private class NicknameResponse
    {
        public bool success;
        public string nickname;
    }
}