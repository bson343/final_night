using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    private static PopUpManager instance;
    public static PopUpManager Instance
    {
        get { return instance; }
    }
    public GameObject Option;

    public void Awake()
    {
        Option.SetActive(false);
        DontDestroyOnLoad(this);
        instance = this;
    }
}
