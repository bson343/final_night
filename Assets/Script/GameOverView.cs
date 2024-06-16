using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverView : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gameOverPopup;
    public static int clearfloor;
    public TMP_Text clearfloorText;
    public TMP_Text PlaytimeText;
    
    private void Start()
    {
        clearfloor = 0;

        if (UserManager.Instance != null)
        {
            UserManager.Instance.OnDataChanged += SetActiveGamaOver;
        }
        else
        {
            Debug.LogError("UserManager Instance is null. Ensure UserManager is added to the scene.");
        }
    }
    public void OnClickSurrender()
    {
        UserManager.Instance.UpdateCurrentHP(0);
    }

    public void SetActiveGamaOver()
    {
        if (UserManager.Instance.CurrentHP <= 0)
        {
            clearfloorText.text = "클리어한 층 : " + clearfloor;
            gameOverPopup.SetActive(true);
        }
    }

    private string FormatPlaytime(int playtimeInSeconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(playtimeInSeconds);
        return $"{time.Hours}시간 {time.Minutes}분 {time.Seconds}초";
    }
}
