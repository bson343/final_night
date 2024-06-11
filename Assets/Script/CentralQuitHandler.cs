using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CentralQuitHandler : MonoBehaviour
{
    private bool isQuitting = false;
    private bool quitConfirmed = false;

    private void OnEnable()
    {
        Application.quitting += OnQuitting;
    }

    private void OnDisable()
    {
        Application.quitting -= OnQuitting;
    }

    private void OnQuitting()
    {
        if (isQuitting) return;

        isQuitting = true;
        StartCoroutine(HandleQuit());
    }

    private IEnumerator HandleQuit()
    {
        // GamePlayDataUpdater와 TimerManager에서 데이터를 저장하는 코루틴 호출
        yield return StartCoroutine(GamePlayDataUpdater.Instance.SaveGameData());
        yield return StartCoroutine(TimerManager.Instance.SavePlayTime());
        Debug.Log("데이터 보내기 완료");

        // 코루틴 완료 후 종료 플래그 설정
        quitConfirmed = true;
    }

    private void Update()
    {
        if (isQuitting && quitConfirmed)
        {
            QuitApplication();
        }
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
