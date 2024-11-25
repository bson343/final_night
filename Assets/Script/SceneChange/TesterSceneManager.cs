using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TesterSceneManager : MonoBehaviour
{
    public GameObject navObject;
    // Start is called before the first frame update

    public void OnClickTestBattleStart()
    {
        MapPlayerTracker.Instance.SetActiveNav();
        NightSceneManager.Instance.GameLoadScene("TestBattleScene");
    }

    public void OnClickTestShopStart()
    {
        MapPlayerTracker.Instance.SetActiveNav();
        NightSceneManager.Instance.GameLoadScene("shop 1");
    }

    public void OnClickTestEventShopStart()
    {
        MapPlayerTracker.Instance.SetActiveNav();
        NightSceneManager.Instance.LoadRandomScene();
    }

    public void OnClickTestBossStart()
    {
        MapPlayerTracker.Instance.SetActiveNav();
        NightSceneManager.Instance.GameLoadScene("Boss");
    }

    public void OnClickRestStart()
    {
        MapPlayerTracker.Instance.SetActiveNav();
        NightSceneManager.Instance.GameLoadScene("Rest");
    }

    public void OnClickEliteStart()
    {
        MapPlayerTracker.Instance.SetActiveNav();
        NightSceneManager.Instance.GameLoadScene("Elite");
    }

    public void OnClickTreasureStart()
    {
        MapPlayerTracker.Instance.SetActiveNav();
        NightSceneManager.Instance.GameLoadScene("Treasure");
    }
}
