using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTurnState : BaseBattleState
{
    BattleCardHolder cardHolder => _battleManager.Player.CardHolder;

    public MyTurnState(BattleManager battleManager, BattleManagerStateFactory stateFactory) : base(battleManager, stateFactory)
    {
        battleState = EBattleState.MyTurn;
    }

    // 드로우
    public override void Enter()
    {
        for(int i = 0; i < 5; i++)
        {
            cardHolder.DrawCard();
        }
    }

    public override void Exit()
    {
        _battleManager.myTurn = false;
    }

    public override void Update()
    {
        if(!_battleManager.myTurn)
        {
            _stateFactory.ChangeState(EBattleState.MyTurnEnd);
        }
    }
}
