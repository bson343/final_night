using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BattleCardState
{
    protected BattleCard _battleCard;
    protected BattleCardStateFactory _stateFactory;
    public ECardUsage cardUsage;


    public BattleCardState(BattleCard baseCard, BattleCardStateFactory stateFactory)
    {
        _battleCard = baseCard;
        _stateFactory = stateFactory;
    }

    public abstract void Enter();


    public abstract void OnBeginDrag(PointerEventData eventData);
    public abstract void OnDrag(PointerEventData eventData);
    public abstract void OnEndDrag(PointerEventData eventData);
    public abstract void OnPointerEnter(PointerEventData eventData);
    public abstract void OnPointerClick(PointerEventData eventData);
    public abstract void OnPointerExit(PointerEventData eventData);


    public abstract void Exit();
}
