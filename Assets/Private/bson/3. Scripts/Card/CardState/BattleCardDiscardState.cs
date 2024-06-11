using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleCardDiscardState : BattleCardState
{
    public BattleCardDiscardState(BattleCard baseCard, BattleCardStateFactory stateFactory) : base(baseCard, stateFactory)
    {
        cardUsage = ECardUsage.DisCard;
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
    }

    public override void OnDrag(PointerEventData eventData)
    {
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        _battleCard.onClickAction?.Invoke();
    }
}
