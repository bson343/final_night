using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BattlePlayer : Character
{
    public Action onDead;
    public GameObject parentObject;

    public PlayerStat PlayerStat { get; private set; }


    public List<BattleCard> CardDeck;

    public BattleCardHolder CardHolder;

    // 플레이어의 CharacterIndent 참조
    public CharacterIndent CharacterIndent { get; private set; }


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

    private void Awake()
    {
        // CharacterIndent 컴포넌트 초기화
        CharacterIndent = GetComponent<CharacterIndent>();

        if (CharacterIndent != null)
        {
            CharacterIndent.Init(this);  // Init() 메서드를 호출해 초기화
        }
        else
        {
            Debug.LogError("CharacterIndent 컴포넌트를 찾을 수 없습니다!");
        }
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
        Transform childTransform = parentObject.transform.Find("UnitRoot"); // "B"는 자식 오브젝트 이름

        if (childTransform != null)
        {
            Animator childAnimator = childTransform.GetComponent<Animator>();
            if (childAnimator != null)
            {
                // Animator의 트리거 활성화
                childAnimator.SetTrigger("isDead"); // Animator의 파라미터 이름
            }
            else
            {
                Debug.LogWarning("Animator 컴포넌트가 B 오브젝트에 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("B 오브젝트를 찾을 수 없습니다.");
        }

    }
    public void attack()
    {
        Transform childTransform = parentObject.transform.Find("UnitRoot"); // "B"는 자식 오브젝트 이름

        if (childTransform != null)
        {
            Animator childAnimator = childTransform.GetComponent<Animator>();
            if (childAnimator != null)
            {
                // Animator의 트리거 활성화
                childAnimator.SetTrigger("attack"); // Animator의 파라미터 이름
            }
            else
            {
                Debug.LogWarning("Animator 컴포넌트가 B 오브젝트에 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("B 오브젝트를 찾을 수 없습니다.");
        }
    }

    public override void Hit(int damage, Character attacker)
    {
        Debug.Log("맞았당");
        PlayerStat.Hit(damage);

        /*if (!PlayerStat.IsDead)
        {
            Transform childTransform = parentObject.transform.Find("UnitRoot"); // "B"는 자식 오브젝트 이름

            if (childTransform != null)
            {
                Animator childAnimator = childTransform.GetComponent<Animator>();
                if (childAnimator != null)
                {
                    // Animator의 트리거 활성화
                    childAnimator.SetTrigger("isHitted"); // Animator의 파라미터 이름
                }
                else
                {
                    Debug.LogWarning("Animator 컴포넌트가 B 오브젝트에 없습니다.");
                }
            }
            else
            {
                Debug.LogWarning("B 오브젝트를 찾을 수 없습니다.");
            }


        }*/

    }

    public override void Act()
    {

    }
}
