using System;
using System.IO;
using UnityEngine;

public class LogFileManager : MonoBehaviour
{
    private static LogFileManager _instance;

    private string logFilePath;
    private StreamWriter logWriter;

    public static LogFileManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject logManager = new GameObject("LogFileManager");
                _instance = logManager.AddComponent<LogFileManager>();
                DontDestroyOnLoad(logManager);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);  // 씬이 전환되어도 LogFileManager는 파괴되지 않음
            InitializeLogFile();
        }
        else if (_instance != this)
        {
            // 이미 인스턴스가 존재할 경우 새로 생성된 오브젝트를 파괴
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 로그 파일 초기화 및 파일 열기
    /// </summary>
    private void InitializeLogFile()
    {
        logFilePath = Path.Combine(Application.persistentDataPath, "game_log.txt");

        // 로그 파일을 연다 (없으면 생성)
        logWriter = new StreamWriter(logFilePath, true); // true는 append 모드
        logWriter.AutoFlush = true; // 자동으로 버퍼를 비움

        // 유니티 로그 메시지를 파일에 기록하는 콜백 등록
        Application.logMessageReceived += HandleLog;

        Debug.Log("Log system initialized. Log file at: " + logFilePath);
    }

    /// <summary>
    /// 유니티 로그 메시지를 파일에 기록하는 함수
    /// </summary>
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string logEntry = $"{DateTime.Now}: [{type}] {logString}";

        if (type == LogType.Exception || type == LogType.Error)
        {
            logEntry += $"\nStackTrace: {stackTrace}";
        }

        // 로그 내용을 파일에 기록
        logWriter.WriteLine(logEntry);
    }

    private void OnDestroy()
    {
        // 로그 콜백 해제
        Application.logMessageReceived -= HandleLog;

        // 로그 파일 닫기
        if (logWriter != null)
        {
            logWriter.Close();
            logWriter = null;
        }
    }

    private void OnApplicationQuit()
    {
        // 게임 종료 시 로그 파일을 닫음
        if (logWriter != null)
        {
            logWriter.Close();
            logWriter = null;
        }
    }
}
