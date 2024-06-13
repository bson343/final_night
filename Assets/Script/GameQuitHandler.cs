using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuitHandler : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        GamePlayDataUpdater.Instance.SaveGameDataSync();
        TimerManager.Instance.SavePlayTimeSync();
    }

}
