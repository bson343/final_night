using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 텍스트를 업데이트하기 위해 필요

public class TimerManager : MonoBehaviour
{
    
    private int secondsElapsed;

    void Start()
    {
        secondsElapsed = 0;
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1초 대기
            secondsElapsed++;
        }
    }
}
