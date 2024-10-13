using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

public class ConfigLoader : MonoBehaviour 
{
    private const string configFileName = "config.ini";

    [SerializeField]
    private string configPath;

    public string DB_IP { get; private set; }

    public void Awake()
    {
        configPath = Path.Combine( Application.streamingAssetsPath, configFileName);
    }

    public void Init()
    {
        INIParser ini = new INIParser();

        ini.Open(configPath);

        DB_IP = ini.ReadValue("DB", "IP", "localhost");
        Debug.Log($"Config : DB_IP = {DB_IP}");
        ini.Close();
    }
}
