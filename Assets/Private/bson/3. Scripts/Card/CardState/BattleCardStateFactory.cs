using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCardStateFactory
{
    private BattleCard _card;
    private Dictionary<ECardUsage, BattleCardState> _dicState = new Dictionary<ECardUsage, BattleCardState>();

    private BattleCardState _currentState;

    public BattleCardState CurrentState => _currentState;

    public BattleCardStateFactory(BattleCard battleCard)
    {
        _card = battleCard;

        _dicState[ECardUsage.Battle] = new BattleCardBattleState(_card, this);
        _dicState[ECardUsage.Gain] = new BattleCardGainState(_card, this);

        // 가장 처음은 배틀상태로 초기화
        ChangeState(ECardUsage.Battle);
    }

    public void ChangeState(ECardUsage cardState)
    {
        BattleCardState newState = GetState(cardState);

        if (_currentState != null)
        {
            _currentState.Exit();
        }

        _currentState = newState;

        if (_currentState != null)
        {
            _currentState.Enter();
        }

        Debug.Log(_currentState + "로 상태가 전환되었습니다.");
    }

    public BattleCardState GetState(ECardUsage cardState)
    {
        if (!_dicState.ContainsKey(cardState))
        {
            Debug.Log("잘못된 키 입력입니다.");
            return null;
        }

        return _dicState[cardState];
    }
}
