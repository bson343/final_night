using UnityEngine;
using UnityEngine.Networking; // UnityWebRequest ���
using System.Collections;   // Coroutine ���
using System.Collections.Generic; // List ���
using System.Text;          // StringBuilder ���
using Newtonsoft.Json;
using System;      // Newtonsoft.Json ���

// Node.js API ���� ������ �´� C# Ŭ���� ����
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

    // �� ��û �� ������ ó�� �ڷ�ƾ
    IEnumerator GetSheetDataCoroutine(Action completeCallback)
    {
        Debug.Log("Requesting sheet data from: " + apiUrl);

        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            // ��û ������ ���� ��ٸ���
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError ||
                request.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError($"Error fetching sheet data: {request.error}");
                Debug.LogError($"Response Code: {request.responseCode}");
            }

            // ��û ����
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Received JSON response:\n" + jsonResponse);

            try
            {
                // JSON �Ľ� (Newtonsoft.Json ���)
                SheetApiResponse apiResponse = JsonConvert.DeserializeObject<SheetApiResponse>(jsonResponse);

                if (apiResponse != null && apiResponse.sheetData != null)
                {
                    Debug.Log($"Data source: {apiResponse.source}, Modified time: {apiResponse.modifiedTime}");

                    // sheetData�� CSV�� ��ȯ
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

    // List<List<string>> �����͸� CSV ���ڿ��� ��ȯ�ϴ� �Լ�
    string ConvertToCsv(List<List<string>> data)
    {
        if (data == null || data.Count == 0)
        {
            return ""; // �����Ͱ� ������ �� ���ڿ� ��ȯ
        }

        StringBuilder csvBuilder = new StringBuilder();

        foreach (var row in data)
        {
            List<string> escapedCells = new List<string>();
            foreach (var cell in row)
            {
                escapedCells.Add(EscapeCsvCell(cell ?? "")); // null ���� �� ���ڿ��� ó��
            }
            csvBuilder.AppendLine(string.Join(",", escapedCells)); // ������ ��ǥ�� �����ϰ� �ٹٲ� �߰�
        }

        return csvBuilder.ToString();
    }

    // CSV �� ������ �̽��������ϴ� �Լ�
    string EscapeCsvCell(string cell)
    {
        bool needsQuotes = cell.Contains(",") || cell.Contains("\"") || cell.Contains("\n") || cell.Contains("\r");

        // ����ǥ(") �̽������� ( "" �� ����)
        string escapedCell = cell.Replace("\"", "\"\"");

        if (needsQuotes)
        {
            // ��ǥ, ����ǥ, �ٹٲ� ���� ���Ե� ��� ��ü ���� ����ǥ�� ���α�
            return $"\"{escapedCell}\"";
        }
        else
        {
            return escapedCell;
        }
    }
}