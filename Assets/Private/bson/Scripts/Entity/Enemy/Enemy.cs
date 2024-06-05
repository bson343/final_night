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

    private void Awake()
    {
        CharacterStat = GetComponent<CharacterStat>();
        
        CharacterStat.Init(this);
        
        battleManager.onStartMyTurn += OnStartMyTurn;
        battleManager.onEndMyTurn += OnEndMyTurn;
        battleManager.onStartEnemyTurn += OnStartEnemyTurn;
        battleManager.onEndEnemyTurn += OnEndEnemyTurn;
        
        battleManager.onStartBattle += OnStartBattle;
        battleManager.onEndBattle += OnEndBattle;
    }
    
    protected virtual void OnStartBattle()
    {
        
    }
    
    protected virtual void OnEndBattle()
    {

    }
    
    protected virtual void OnStartEnemyTurn()
    {
        
    }
    
    protected virtual void OnEndEnemyTurn()
    {
        
    }
    
    protected virtual void OnEndMyTurn()
    {

    }
    
    protected virtual void OnStartMyTurn()
    {
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
    }

    public override void Hit(int damage, Character attacker)
    {
        Debug.Log("맞았당");
        CharacterStat.Hit(damage);
    }

    public override void Act()
    {
        Debug.Log("행동한당");
    }
}
