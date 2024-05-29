using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECardUsage
{
    Battle,   // 배틀
    Check,    // 확인
    Gain,     // 얻기
    Enforce,  // 강화
    DisCard,  // 제거
    Sell,     // 판매
}
public class BattleCard : MonoBehaviour
{
    public int generateNumber;

    // 온클릭 함수
    public Action onClickAction;

    [SerializeField]
    private BattleCardController _cardController;

    private BattleCardStateFactory _CardStateFactory;
    private BattleCardHolder _cardHolder;

    public BattleCardState CurrentState => _CardStateFactory.CurrentState;
    public BattleCardHolder CardHolder => _cardHolder;
    public BattleCardController CardController => _cardController;

    public void Init(BattleCardHolder cardHolder, int generateNumber)
    {
        _CardStateFactory = new BattleCardStateFactory(this);

        _cardHolder = cardHolder;

        _cardController.Init(true, this);

        // 정렬 데이터
        this.generateNumber = generateNumber;
    }

    public void ChangeState(ECardUsage cardUsage)
    {
        _CardStateFactory.ChangeState(cardUsage);
    }
}
