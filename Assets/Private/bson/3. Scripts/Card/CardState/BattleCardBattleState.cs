using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleCardBattleState : BattleCardState
{
    public static BattlePlayer Instance;
    private bool _isDrag = false;
    private BattlePlayer ani;
    public GameObject a;

    void Awake()
    {
        GameObject parentObject = BattlePlayer.Instance.parentObject;
       
        ani = BattlePlayer.Instance; // 싱글턴을 통해 BattlePlayer 인스턴스 가져오기
        if (ani == null)
        {
            Debug.LogError("BattlePlayer 인스턴스가 null입니다. BattlePlayer가 초기화되었는지 확인하세요.");
        }
        Debug.LogWarning(ani.parentObject.name);
        if (ani.parentObject == null)
        {
            Debug.LogWarning("부모object = null");
        }
        
    }
    public BattleCardBattleState(BattleCard battleCard, BattleCardStateFactory stateFactory) : base(battleCard, stateFactory)
    {
        cardUsage = ECardUsage.Battle;
    }

    public override void Enter()
    {
        
        _isDrag = false;
    }

    public override void Exit()
    {

    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        _isDrag = true;
        
        GlobalSoundManager.Instance.PlaySE(ESE.CardSelect);

        _battleCard.CardHolder.selectedCard = _battleCard;

        _battleCard.CardController.StopAllCoroutine();

        _battleCard.transform.SetAsLastSibling();

        if (_battleCard.CardController.IsBezierCurve)
        {
            _battleCard.CardHolder.BezierCurve.gameObject.SetActive(true);
            _battleCard.CardHolder.MoveCenter(_battleCard);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (_battleCard.CardController.IsBezierCurve)
        {
            _battleCard.CardHolder.BezierCurve.p0.position = _battleCard.transform.position;
            _battleCard.CardHolder.BezierCurve.p2.position = eventData.position;
        }
        else
        {
            _battleCard.transform.position = eventData.position;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        _isDrag = false;

        if (_battleCard.CardController.IsBezierCurve) // 타겟팅
        {
            if (battleManager.TargetEnemy != null)
            {
                _battleCard.CardController.SetActiveRaycast(false);
                _battleCard.UseCard();
                
                    BattlePlayer.Instance.attack();
                    Debug.Log("Attack animation triggered.");
                
            }
            
            battleManager.TargetEnemy = null;
            _battleCard.CardHolder.BezierCurve.gameObject.SetActive(false);
        }
        else
        {
            // 사용가능(코스트 등등)이거나 사용범위에 있을 때만 사용
            // 사용 범위 y값이 300이상 -> 이는 해상도에 따라 바꾸는 로직이 필요
            if (eventData.position.y > 300f)
            {
                _battleCard.CardController.SetActiveRaycast(false);
                _battleCard.UseCard();
                BattlePlayer.Instance.magic();
            }
        }

        // 할거 다 하고 null처리
        _battleCard.CardHolder.selectedCard = null;
        // 재정렬 후 베지어 곡선 비활성화
        _battleCard.CardHolder.Relocation();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (_isDrag)
            return;

        _battleCard.CardHolder.OverCard(_battleCard);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (_isDrag)
            return;

        _battleCard.CardHolder.Relocation();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        _battleCard.onClickAction?.Invoke();
    }
}
