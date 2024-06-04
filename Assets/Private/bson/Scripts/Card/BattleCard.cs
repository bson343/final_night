using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECardUsage
{
    Battle,   // 배틀
    DisCard,  // 제거
    Gain,     // 얻기
}
public class BattleCard : MonoBehaviour
{
    public int generateNumber;
    public ECardType cardType;
    public int cost;
    public string cardName;

    // 온클릭 함수
    public Action onClickAction;

    [SerializeField]
    private BattleCardController _cardController;

    private BattleCardStateFactory _CardStateFactory;
    private BattleCardHolder _cardHolder;
    private CardData _cardData;

    public CardData CardData => _cardData;
    public BattleCardState CurrentState => _CardStateFactory.CurrentState;
    public BattleCardHolder CardHolder => _cardHolder;
    public BattleCardController CardController => _cardController;
    
    private BattleManager battleManager => ServiceLocator.Instance.GetService<BattleManager>();

    public void Init(BattleCardHolder cardHolder, CardData cardData, int generateNumber)
    {
        _CardStateFactory = new BattleCardStateFactory(this);

        _cardHolder = cardHolder;
        _cardData = cardData;

        _cardController.Init(_cardData.isBezierCurve, this);

        // 정렬 데이터
        this.generateNumber = generateNumber;
        cardType = _cardData.cardType;
        cost = _cardData.cost;
        cardName = _cardData.cardName;
    }

    public void ChangeState(ECardUsage cardUsage)
    {
        _CardStateFactory.ChangeState(cardUsage);
    }
    
    public void UseCard()
    {
        if (TryUseCard())
        {
            _cardData.useEffect.ForEach(useEffect => useEffect?.Invoke());

            if(_cardData.isExtinction)
            {
                // 소멸 카드면 소멸
                _cardHolder.Extinction(this);
            }
            else
            {
                // 카드 버림
                _cardHolder.DiscardCard(this);
            }
        }
    }
    
    private bool TryUseCard()
    {
        if (battleManager.Player.PlayerStat.CurrentOrb >= _cardData.cost)
        {
            battleManager.Player.PlayerStat.CurrentOrb -= _cardData.cost;
            return true;
        }
        else
        {
            _cardController.SetActiveRaycast(true);
            return false;
        }
    }
    
    public void Discard()
    {
        // 내 카드에서 제거함
        battleManager.Player.CardDeck.Remove(this);
    }
}
