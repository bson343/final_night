using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBGMStarter : MonoBehaviour
{
    private void Awake()
    {
        GlobalSoundManager.Instance.FadeBGM(EBGM.Menu);
    }
}
