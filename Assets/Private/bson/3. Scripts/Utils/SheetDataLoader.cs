using UnityEngine;
using UnityEngine.Networking; // UnityWebRequest 사용
using System.Collections;   // Coroutine 사용
using System.Collections.Generic; // List 사용
using System.Text;          // StringBuilder 사용
using Newtonsoft.Json;
using System;      // Newtonsoft.Json 사용

// Node.js API 응답 구조에 맞는 C# 클래스 정의
[System.Serializable]
public class SheetApiResponse
{
    public string source;
    public string spreadsheetId;
    public string modifiedTime;
    public List<List<string>> sheetData;
}

public class SheetDataLoader : MonoBehaviour
{
    public string apiUrl = "http://localhost:3000/card-info";

    public string RawDataCSV { get; private set; }

    public void Init(Action completeCallback, string url)
    {
        apiUrl = url;

        StartCoroutine(GetSheetDataCoroutine(completeCallback));
    }

    // 웹 요청 및 데이터 처리 코루틴
    IEnumerator GetSheetDataCoroutine(Action completeCallback)
    {
        Debug.Log("Requesting sheet data from: " + apiUrl);

        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            // 요청 보내고 응답 기다리기
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError ||
                request.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError($"Error fetching sheet data: {request.error}");
                Debug.LogError($"Response Code: {request.responseCode}");
            }

            // 요청 성공
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Received JSON response:\n" + jsonResponse);

            try
            {
                // JSON 파싱 (Newtonsoft.Json 사용)
                SheetApiResponse apiResponse = JsonConvert.DeserializeObject<SheetApiResponse>(jsonResponse);

                if (apiResponse != null && apiResponse.sheetData != null)
                {
                    Debug.Log($"Data source: {apiResponse.source}, Modified time: {apiResponse.modifiedTime}");

                    // sheetData를 CSV로 변환
                    RawDataCSV = ConvertToCsv(apiResponse.sheetData);
                    Debug.Log("Successfully converted sheetData to CSV.");
                    completeCallback?.Invoke();

                }
                else
                {
                    Debug.LogError("Failed to parse JSON or sheetData is null.");
                }
            }
            catch (JsonException jsonEx)
            {
                Debug.LogError($"JSON Parsing Error: {jsonEx.Message}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"An unexpected error occurred: {ex.Message}");
            }

        }
    }

    // List<List<string>> 데이터를 CSV 문자열로 변환하는 함수
    string ConvertToCsv(List<List<string>> data)
    {
        if (data == null || data.Count == 0)
        {
            return ""; // 데이터가 없으면 빈 문자열 반환
        }

        StringBuilder csvBuilder = new StringBuilder();

        foreach (var row in data)
        {
            List<string> escapedCells = new List<string>();
            foreach (var cell in row)
            {
                escapedCells.Add(EscapeCsvCell(cell ?? "")); // null 셀은 빈 문자열로 처리
            }
            csvBuilder.AppendLine(string.Join(",", escapedCells)); // 셀들을 쉼표로 연결하고 줄바꿈 추가
        }

        return csvBuilder.ToString();
    }

    // CSV 셀 내용을 이스케이프하는 함수
    string EscapeCsvCell(string cell)
    {
        bool needsQuotes = cell.Contains(",") || cell.Contains("\"") || cell.Contains("\n") || cell.Contains("\r");

        // 따옴표(") 이스케이프 ( "" 로 변경)
        string escapedCell = cell.Replace("\"", "\"\"");

        if (needsQuotes)
        {
            // 쉼표, 따옴표, 줄바꿈 등이 포함된 경우 전체 셀을 따옴표로 감싸기
            return $"\"{escapedCell}\"";
        }
        else
        {
            return escapedCell;
        }
    }
}