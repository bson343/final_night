using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public enum ECardUsage
{
    Battle,   // 배틀
    DisCard,  // 제거
    Gain,     // 얻기
}
public class BattleCard : MonoBehaviour
{
    //Deprecated start
    public int generateNumber;
    public ECardType cardType;
    public int cost;
    public int cardID;
    public string cardName;
    //Deprecated end

    public int CardId;

    // 온클릭 함수
    public Action onClickAction;

    [SerializeField]
    private BattleCardController _cardController;

    private BattleCardStateFactory _CardStateFactory;
    private BattleCardHolder _cardHolder;
    private BattleCardData _currentCardData;

    //public BattleCardData CardData => _currentCardData;
    public BattleCardState CurrentState => _CardStateFactory.CurrentState;
    public BattleCardHolder CardHolder => _cardHolder;
    public BattleCardController CardController => _cardController;
    
    private BattleManager battleManager => ServiceLocator.Instance.GetService<BattleManager>();
    Dictionary<decimal, BattleCardData> CardDataMap => ResourceManager.Instance.CardDataMap;
    Dictionary<string, Sprite> CardSpriteMap => ResourceManager.Instance.CardSpriteMap;

    //Deprecated
    public void Init(BattleCardHolder cardHolder, BattleCardData cardData, int generateNumber)
    {
        Assert.IsTrue(false, "Deprecated Func, Use \'public void Init(BattleCardHolder cardHolder, int cardId)\'");
        _CardStateFactory = new BattleCardStateFactory(this);

        _cardHolder = cardHolder;
        _currentCardData = cardData;

        _cardController.Init(_currentCardData.isBezierCurve, this);

        // 정렬 데이터
        this.generateNumber = generateNumber;
        cardType = _currentCardData.cardType;
        cost = _currentCardData.cost;
        cardName = _currentCardData.cardName;
        cardID = _currentCardData.id;

        Image cardImage = GetComponent<Image>();
        cardImage.sprite = _currentCardData.cardImage;
    }

    public void Init(BattleCardHolder cardHolder, int cardId)
    {
        _CardStateFactory = new BattleCardStateFactory(this);
        _cardHolder = cardHolder;

        //_currentCardData = ResourceManager.Instance.CardDataMap[cardId];
        updateCardResource(cardId);

        _cardController.Init(_currentCardData.isBezierCurve, this);

        // 정렬 데이터
        this.generateNumber = -1;
        cardType = _currentCardData.cardType;
        cost = _currentCardData.cost;
        cardName = _currentCardData.cardName;
    }

    public void ChangeState(ECardUsage cardUsage)
    {
        _CardStateFactory.ChangeState(cardUsage);
    }
    
    public void UseCard()
    {
        if (TryUseCard())
        {
            //_cardData.useEffect.ForEach(useEffect => useEffect?.Invoke());
            foreach (var useCard in _currentCardData.effects)
            {
                battleManager.CardEffectTable[useCard]?.Invoke();
            }

            if(_currentCardData.isExtinction)
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
        if (battleManager.Player.PlayerStat.CurrentOrb >= _currentCardData.cost)
        {
            battleManager.Player.PlayerStat.CurrentOrb -= _currentCardData.cost;
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

    private void updateCardResource(int cardId)
    {
        this._currentCardData = CardDataMap[cardId];

        transform.Find("background").GetComponent<Image>().sprite = CardSpriteMap[_currentCardData.getSpritePath("background")];

        transform.Find("icon").GetComponent<Image>().sprite = CardSpriteMap[_currentCardData.getSpritePath("icon")];

        Transform goName = transform.Find("name");
        goName.GetComponent<Image>().sprite = CardSpriteMap[_currentCardData.getSpritePath("name")];
        goName.GetChild(0).GetComponent<TMP_Text>().text = _currentCardData.cardName;

        Transform goCost = transform.Find("cost");
        goCost.GetComponent<Image>().sprite = CardSpriteMap[_currentCardData.getSpritePath("cost")];
        goCost.GetChild(0).GetComponent<TMP_Text>().text = _currentCardData.cost.ToString();

        Transform goInfor = transform.Find("infor");
        goInfor.GetComponent<Image>().sprite = CardSpriteMap[_currentCardData.getSpritePath("infor")];
        goInfor.GetChild(0).GetComponent<TMP_Text>().text = _currentCardData.cardTypeString;
        goInfor.GetChild(1).GetComponent<TMP_Text>().text = _currentCardData.cardExplanation;
    }
}
