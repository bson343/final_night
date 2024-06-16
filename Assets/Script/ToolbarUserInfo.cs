using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolbarUserInfo : MonoBehaviour
{
    public TMP_Text nicknameText;
    public TMP_Text GoldText;
    public TMP_Text HPText;

    
    private void Start()
    {
        if (UserManager.Instance != null)
        {
            UserManager.Instance.OnDataChanged += UpdateStausText;
            UpdateNickname();
            UpdateStausText();
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

    public void UpdateStausText()
    {
        GoldText.text = UserManager.Instance.Gold.ToString();
        HPText.text = UserManager.Instance.MaxHP.ToString()+"/" + UserManager.Instance.CurrentHP.ToString();
    }
   
    public void GameExit()
    {
        Application.Quit();
#if UNITY_EDITOR
        // 유니티 에디터에서 실행 중일 경우 에디터를 중지합니다.
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
