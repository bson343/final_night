using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<CardData> deck = new List<CardData>();

    // 덱에 카드를 추가하는 함수
    public void AddCardToDeck(CardData card)
    {
        deck.Add(card);
    }

    // 덱에서 카드를 제거하는 함수
    public void RemoveCardFromDeck(CardData card)
    {
        deck.Remove(card);
    }
}
