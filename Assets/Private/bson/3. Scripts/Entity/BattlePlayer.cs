using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BattlePlayer : Character
{
    public Action onDead;
    
    public PlayerStat PlayerStat { get; private set; }
    
    
    public List<BattleCard> CardDeck;

    public BattleCardHolder CardHolder;

    private BattleCardGenerator _cardGenerator => ServiceLocator.instance.GetService<BattleCardGenerator>();
    public CharacterAnimation CharacterAnimation { get; private set; }

    public void init()
    {
        PlayerStat = GetComponent<PlayerStat>();
        CharacterAnimation = GetComponent<CharacterAnimation>();

        PlayerStat.Init(this);
        CharacterAnimation.Init(this);
        
        battleManager.onStartMyTurn += (() => PlayerStat.CurrentOrb = PlayerStat.MaxOrb);
        battleManager.onStartBattle += (() => OnStartBattle());
        battleManager.onEndBattle += (() => OnEndBattle());
    }
    
    public void OnStartBattle()
    {
        CardHolder.StartBattle(CardDeck);
    }

    public void OnEndBattle()
    {
        CardHolder.EndBattle(CardDeck);
    }

    public void ResumeBattle()
    {
        CardHolder.ResumeBattle(CardDeck);
    }
    
    public void AddCard(BattleCard card)
    {
        CardDeck.Add(card);
    }

    public override void Dead()
    {
        Debug.Log("주겄당");

        onDead?.Invoke();
        CharacterAnimation.SetTrigger("isDead");
    }

    public override void Hit(int damage, Character attacker)
    {
        Debug.Log("맞았당");
        PlayerStat.Hit(damage);

        if (!PlayerStat.IsDead)
        {
            CharacterAnimation.SetTrigger("isHitted_P");
            CharacterAnimation.SetTrigger("back");

        }
            
    }

    public override void Act()
    {
        
    }
}
