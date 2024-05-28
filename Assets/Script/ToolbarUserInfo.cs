using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolbarUserInfo : MonoBehaviour
{
    public TMP_Text nicknameText;

    private void Start()
    {
        if (UserManager.Instance != null)
        {
            UpdateNickname();
        }
        else
        {
            Debug.LogError("UserManager Instance is null. Ensure UserManager is added to the scene.");
        }
    }

    private void UpdateNickname()
    {
        string nickname = UserManager.Instance.UserNickname;
        if (!string.IsNullOrEmpty(nickname))
        {
            nicknameText.text = nickname;
        }
        else
        {
            Debug.LogWarning("UserNickname is not set.");
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
