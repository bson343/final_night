using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class start_test : MonoBehaviour
{

    public void OnClickGameStart() //시작버튼을 누르면 맵으로 이동
    {
        Debug.Log("게임시작");
        NightSceneManager.Instance.LoadScene("NodeMap Test");
    }
    public void OnClickOption()
    {
        Debug.Log("환경설정");
    }
    public void OnClickGameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}