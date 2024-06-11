using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisableMainCameraInBattleSecene : MonoBehaviour
{
    void Start()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("MainCamera");

        foreach (var n in temp)
        {
            if (n.scene.name != "NodeMap1")
            {
                Camera disableCamera = n.GetComponent<Camera>();

                disableCamera.enabled = false;

            }
        }

    }

    
}
