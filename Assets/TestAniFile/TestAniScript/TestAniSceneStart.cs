using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestAniSceneStart : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnClickTestAniStart()
    {
       
            NightSceneManager.Instance.LoadScene("TestAniBattleScene");
       
        
    }
}
