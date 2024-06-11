using UnityEngine;
using System.Collections;
using UnityEditor;

public class CentralQuitHandler : MonoBehaviour
{
    public GamePlayDataUpdater gamePlayDataUpdater;  // Inspector에서 할당
    public TimerManager timerManager;                // Inspector에서 할당
    private bool isQuitting = false;
    private bool quitConfirmed = false;

    private void OnApplicationQuit()
    {
        if (isQuitting) return;

        isQuitting = true;
        StartCoroutine(HandleQuit());
        Application.CancelQuit(); // 애플리케이션 종료를 취소하고 코루틴 완료 후 종료
    }

    private IEnumerator HandleQuit()
    {
        if (gamePlayDataUpdater != null)
        {
            yield return StartCoroutine(gamePlayDataUpdater.SaveGameData());
        }
        else
        {
            Debug.LogError("GamePlayDataUpdater is not assigned in the Inspector");
        }

        if (timerManager != null)
        {
            yield return StartCoroutine(timerManager.SavePlayTime());
        }
        else
        {
            Debug.LogError("TimerManager is not assigned in the Inspector");
        }

        Debug.Log("데이터 보내기 완료");

        // 코루틴 완료 후 종료 플래그 설정
        quitConfirmed = true;

        QuitApplication();
    }

    private void QuitApplication()
    {
#if UNITY_EDITOR
        // 에디터에서 실행 중이면 플레이 모드를 종료
        EditorApplication.isPlaying = false;
#else
        // 빌드된 애플리케이션에서는 애플리케이션 종료
        Application.Quit();
#endif
    }
}
