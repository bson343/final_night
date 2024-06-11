using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempPlayer : MonoBehaviour
{
    public List<BattleCard> myCards;
    public BattleCardHolder cardHolder;


    public void Init()
    {
        
    }

    public void OnStartBattle()
    {
        cardHolder.StartBattle(myCards);
    }

    public void OnEndBattle()
    {
        cardHolder.EndBattle(myCards);
    }

    public void ResumeBattle()
    {
        cardHolder.ResumeBattle(myCards);
    }


    // 플레이어의 카드를 더해준다.
    public void AddCard(BattleCard card)
    {
        myCards.Add(card);
    }

    // 플레이어의 카드를 제거한다.
    public void RemoveCard(BattleCard card)
    {

    }

}
