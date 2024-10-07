using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalConfig : GlobalSingleton<GlobalConfig>
{
    public string IP { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        INIParser ini = new INIParser();

        string path = Application.streamingAssetsPath + "/config.ini";

        ini.Open(path);

        IP = ini.ReadValue("Server", "Ip", "localhost") as string;
        ini.Close();
    }

}
