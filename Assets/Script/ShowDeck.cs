using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDeck : MonoBehaviour
{
    public GameObject DeckPopup;

    // Start is called before the first frame update

    public void OnClickDeck()
    {
        DeckPopup.SetActive(true);
    }

    public void OnClickOutDeck()
    {
        DeckPopup.SetActive(false);
    }
}