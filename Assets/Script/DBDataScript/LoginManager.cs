using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;



public class LoginManager : MonoBehaviour
{
    public TMP_InputField userIdInput;
    public TMP_InputField passwordInput;
    public TMP_Text resultText;
    public GameObject loginpopup;

    public void OnLoginButtonClicked()
    {
        StartCoroutine(LoginCoroutine());
    }

    private IEnumerator LoginCoroutine()
    {
        //string url = "http://localhost:8080/api/login"; // 마리아 db에 로그인 요청
        string url = "http://10.30.1.110:8080/api/login"; // 마리아 db에 로그인 요청
        WWWForm form = new WWWForm();
        form.AddField("userId", userIdInput.text);
        form.AddField("userPassword", passwordInput.text);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            loginpopup.SetActive(true);
            resultText.text = "Error: 로그인의 실패하셨습니다. " + www.error;
        }
        else
        {
            string response = www.downloadHandler.text;
            LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(response);
            if (loginResponse.success)
            {
                Debug.Log("로그인 성공! UserNum: " + loginResponse.userNum);
                UserManager.Instance.SetUserNum(loginResponse.userNum); // UserManager를 통해 userNum 설정
                NightSceneManager.Instance.LoadScene("Main");
            }
            else
            {
                loginpopup.SetActive(true);
                resultText.text = "아이디 또는 비밀번호가 일치하지않습니다.";
            }
        }
    }

    [System.Serializable]
    private class LoginResponse
    {
        public bool success;
        public string message;
        public long userNum;
    }
}
