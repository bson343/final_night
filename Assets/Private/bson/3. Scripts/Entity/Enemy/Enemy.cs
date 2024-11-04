using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : Character, IPointerEnterHandler, IPointerExitHandler
{

   
    protected enum EEnemyGrade
    {
        common,
        Elite,
        Boss
    }

    [SerializeField] protected EEnemyGrade _enemyGrade;
    [SerializeField] private GameObject _reticle;
    
    public CharacterStat CharacterStat { get; private set; }
    public CharacterAnimation CharacterAnimation { get; private set; }
    public EnemyPattern EnemyPattern { get; private set; }

   // public CharacterIndent CharacterIndent { get; private set; }


    private void Awake()
    {

        CharacterStat = GetComponent<CharacterStat>();
        CharacterAnimation = GetComponent<CharacterAnimation>();
        EnemyPattern = GetComponent<EnemyPattern>();
       // CharacterIndent = GetComponent<CharacterIndent>();

        CharacterStat.Init(this);
        CharacterAnimation.Init(this);
     //   CharacterIndent.Init(this);
        EnemyPattern.Init(this);

        battleManager.onStartMyTurn += OnStartMyTurn;
        battleManager.onEndMyTurn += OnEndMyTurn;
        battleManager.onStartEnemyTurn += OnStartEnemyTurn;
        battleManager.onEndEnemyTurn += OnEndEnemyTurn;
        
        battleManager.onStartBattle += OnStartBattle;
        battleManager.onEndBattle += OnEndBattle;
    }
    
    protected virtual void OnStartBattle()
    {
     //   CharacterIndent.Visualize();
    }
    
    protected virtual void OnEndBattle()
    {

    }
    
    protected virtual void OnStartEnemyTurn()
    {
        
    }
    
    protected virtual void OnEndEnemyTurn()
    {
      //  CharacterIndent.UpdateIndents();
        // 의식이면 내 턴이 시작될 때 파워가 3 상승
        if (indent[(int)EIndent.Consciousness])
        {
            CharacterStat.Power += 3;
        }
    }
    
    protected virtual void OnEndMyTurn()
    {

    }
    
    protected virtual void OnStartMyTurn()
    {
        EnemyPattern.DecidePattern();
    }
    
    public void DestroyMySelf()
    {
        battleManager.onStartMyTurn -= OnStartMyTurn;
        battleManager.onEndMyTurn -= OnEndMyTurn;
        battleManager.onStartEnemyTurn -= OnStartEnemyTurn;
        battleManager.onEndEnemyTurn -= OnEndEnemyTurn;
        battleManager.onStartBattle -= OnStartBattle;
        battleManager.onEndBattle -= OnEndBattle;

        battleManager.Enemies.Remove(this);
        Destroy(gameObject);
    }

    public void LockOn()
    {
        _reticle.SetActive(true);
    }

    public void LockOff()
    {
        _reticle.SetActive(false);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        battleManager.TargetEnemy = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        battleManager.TargetEnemy = null;
    }

    public override void Dead()
    {
        Debug.Log("주겄당");
        CharacterAnimation.SetTrigger("isDead");
        
        switch (_enemyGrade)
        {
            case EEnemyGrade.common:
                battleManager.defeatCommonEnemy++;
                break;
            case EEnemyGrade.Elite:
                battleManager.defeatElite++;
                break;
            case EEnemyGrade.Boss:
                battleManager.defeatBoss++;
                break;
        }
    }

    public override void Hit(int damage, Character attacker)
    {
        Debug.Log("맞았당");
        CharacterStat.Hit(damage);

        if (!CharacterStat.IsDead)
        {
            
            CharacterAnimation.SetTrigger("isHitted");
            CharacterAnimation.SetTrigger("back");
            Debug.Log("살아있음");
        }
        

     }

    public override void Act()
    {
        Debug.Log("행동한당");

        EnemyPattern.Act();
        StartCoroutine(CharacterAnimation.CoAct(false));
       // CharacterIndent.Visualize();
    }
}
