using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour
{

    public void OnClickGameStart()
    {
        Debug.Log("게임시작");
        SceneManager.LoadScene("shop");
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