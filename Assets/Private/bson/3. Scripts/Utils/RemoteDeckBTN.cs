using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RemoteDeckBTN : MonoBehaviour
{
    public static RemoteDeckBTN Instance { get; private set; }

    [SerializeField]
    private Button deckBTN;

    private void Awake()
    {
        deckBTN.interactable = false;

        Instance = this;
        DontDestroyOnLoad(this);
    }

    public bool setInteractable(bool b)
    {
        return deckBTN.interactable = b;
    }
}
