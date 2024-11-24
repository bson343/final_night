using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : Character, IPointerEnterHandler, IPointerExitHandler
{
   /* public bool isFadeIn; // true=FadeIn, false=FadeOut
    public GameObject a;*/
    protected enum EEnemyGrade
    {
        common,
        Elite,
        Boss
    }

    public override void attack()
    {
        
    }
    public override void magic()
    {

    }

    [SerializeField] protected EEnemyGrade _enemyGrade;
    [SerializeField] private GameObject _reticle;
    
    public CharacterStat CharacterStat { get; private set; }
    public CharacterAnimation CharacterAnimation { get; private set; }
    public EnemyPattern EnemyPattern { get; private set; }
    protected BattleManager battleManager => ServiceLocator.Instance.GetService<BattleManager>();
    protected BattlePlayer player => battleManager.Player;
    public CharacterIndent CharacterIndent { get; private set; }

    private static bool isSubscribedToEndMyTurn = false;


    private void Awake()
    {

        CharacterStat = GetComponent<CharacterStat>();
        CharacterAnimation = GetComponent<CharacterAnimation>();
        EnemyPattern = GetComponent<EnemyPattern>();
        CharacterIndent = GetComponent<CharacterIndent>();

        CharacterStat.Init(this);
        CharacterAnimation.Init(this);
        CharacterIndent.Init(this);
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
        CharacterIndent.Visualize();
    }
    
    protected virtual void OnEndBattle()
    {

    }
    
    protected virtual void OnStartEnemyTurn()
    {
        CharacterIndent.BurnDamageUpDate(); // 화상데미지
        isSubscribedToEndMyTurn = false; // 적의 턴이 시작되면 다시 초기화
    }
    
    protected virtual void OnEndEnemyTurn()
    {
        CharacterIndent.UpdateIndents();
        // 의식이면 내 턴이 시작될 때 파워가 3 상승
        /*if (indent[(int)EIndent.Consciousness])
        {
            CharacterStat.Power += 3;
        }*/
    }
    
    protected virtual void OnEndMyTurn()
    {
        if (isSubscribedToEndMyTurn)
        {
            return; // 이미 처리된 경우 중복 실행 방지
        }
        isSubscribedToEndMyTurn = true;

        player.CharacterIndent.UpdateIndents();
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
   /* IEnumerator CoFadeOut()
    {
        float elapsedTime = 0f; // 누적 경과 시간
        float fadedTime = 2f; // 총 소요 시간
        Debug.Log("받음");
        while (elapsedTime <= fadedTime)
        {
            a.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 1f, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            Debug.Log("Fade Out 중...");
            yield return null;
        }

        Debug.Log("Fade Out 끝");

        yield break;
    }*/
    public override void Dead()
    {
        ;
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
        // 약화 상태면 공격력 25퍼 감소
        if (attacker.indent[(int)EIndent.Weakening] == true)
        {
            damage = Mathf.RoundToInt((float)damage * 0.75f);
        }
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
        CharacterIndent.Visualize();
    }
}
