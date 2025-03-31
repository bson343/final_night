using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Networking;


//�����Ϳ����� ����, ������ ���忡���� ��ġ������ ������ ��
public class GoogleSheetManager : MonoBehaviour
{
    const string URL = "https://docs.google.com/spreadsheets/d/10kOEcZANw9pYBE6ju4jobe9-vZLg1cqpljTt5gepUqk/export?format=csv";
    public string RawDataCSV { get; private set; }

    public void Init(Action completeCallback)
    {
        StartCoroutine(loadGoogleSheet(completeCallback));
    }

    

    IEnumerator loadGoogleSheet(Action completeCallback)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);

        //TODO: �ش� ������ ���� ������ �н�
        yield return www.SendWebRequest();

        RawDataCSV = www.downloadHandler.text;

        completeCallback?.Invoke();

#if UNITY_EDITOR
        const string CSV_FOLDER_PATH = "CSV";
        const string CSV_FOLDER_OLD_PATH = "OLD";
        const string CARD_INFO_FILE = "CardInfo.csv";

        string csvPath = Path.Combine(Application.dataPath, "Resources", CSV_FOLDER_PATH);
        string csvOldPath = Path.Combine(csvPath, CSV_FOLDER_OLD_PATH);
        if (!Directory.Exists(csvPath))
        {
            Debug.LogWarning("CSV folder does not exist.");
            yield return null;
        }

        string[] files = Directory.GetFiles(csvOldPath, "*.csv");

        if (files.Length > 3)
        {
            int deletedCount = 0;

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                if (fileName != CARD_INFO_FILE)
                {
                    File.Delete(file);
                    deletedCount++;
                    Debug.Log($"Deleted: {fileName}");
                }
            }

            Debug.Log($"Cleanup complete. Deleted {deletedCount} files.");

            UnityEditor.AssetDatabase.Refresh();
        }

        string oldFilePath = Path.Combine(csvPath, CARD_INFO_FILE);

        if (File.Exists(oldFilePath))
        {
            string timestamp = DateTime.Now.ToString("MM-dd-HH-mm-ss");
            string newFileName = $"CardInfo_{timestamp}.csv";
            string newFilePath = Path.Combine(csvPath, CSV_FOLDER_OLD_PATH, newFileName);

            File.Move(oldFilePath, newFilePath);
            Debug.Log($"Renamed old file to: {newFileName}");
        }

        // ���ο� CardInfo.csv ���� ���� �� ������ �ۼ�
        File.WriteAllText(oldFilePath, RawDataCSV);
        Debug.Log("Created new CardInfo.csv with updated data");
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
