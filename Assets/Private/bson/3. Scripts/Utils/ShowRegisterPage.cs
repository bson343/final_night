using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRegisterPage : MonoBehaviour
{
    public void OpenBrowser()
    {
        string url = "http://" + ResourceManager.Instance.Config.DB_IP + ":8080" + "/api/register";
        Application.OpenURL(url);
    }
}
