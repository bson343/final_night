using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class BattleCardGenerator : MonoBehaviour, IRegisterable
{
    public int GeneratNumber = 0;
    [SerializeField] private Transform _cardParent;
    [SerializeField] private BattleCardHolder _cardHolder;

    [SerializeField] BattleCard _baseBattleCard;
    [SerializeField] BattleCard _defaultDummyCard;
    [SerializeField] private List<BattleCardData> _cardDatas;

    Dictionary<decimal, BattleCardData> CardDataMap => CardMap.Instance.CardDataMap;
    Dictionary<string, Sprite> CardSpriteMap => CardMap.Instance.CardSpriteMap;

    public void Init()
    {
        
    }

    //Deprecated
    public BattleCard GeneratorRandomCard()
    {
        return GenerateBattleCard(Random.Range(0, _cardDatas.Count));
    }

    //Deprecated
    public BattleCard GenerateBattleCard(int cardIdx)
    {
        if (_cardDatas.Count <= cardIdx)
        {
            Assert.IsTrue(false);
        }

        BattleCard genCard = Instantiate(_baseBattleCard, _cardParent);
        genCard.Init(_cardHolder, _cardDatas[cardIdx], GeneratNumber++);
        
        return genCard;
    }

    public BattleCard GenBatCard(int cardId)
    {
        if (
            !(CardMap.Instance.AttackCardIdList.Contains(cardId) 
            || CardMap.Instance.SkillCardIdList.Contains(cardId)
            || CardMap.Instance.HeroCardIdList.Contains(cardId))
            )
        {
            Assert.IsTrue(false);
        }

        BattleCard genCard = Instantiate(_defaultDummyCard, _cardParent);
        updateCardResource(genCard.gameObject.transform, cardId);

        genCard.Init(_cardHolder, cardId);

        return genCard;
    }

    private void updateCardResource(Transform genCard, int cardId)
    {
        BattleCardData cardData = CardDataMap[cardId]; 

        genCard.Find("background").GetComponent<Image>().sprite = CardSpriteMap[cardData.getSpritePath("background")];

        genCard.Find("icon").GetComponent<Image>().sprite = CardSpriteMap[cardData.getSpritePath("icon")];

        Transform goName = genCard.Find("name");
        goName.GetComponent<Image>().sprite = CardSpriteMap[cardData.getSpritePath("name")];
        goName.GetChild(0).GetComponent<TMP_Text>().text = cardData.cardName;

        Transform goCost = genCard.Find("cost");
        goCost.GetComponent<Image>().sprite = CardSpriteMap[cardData.getSpritePath("cost")];
        goCost.GetChild(0).GetComponent<TMP_Text>().text = cardData.cost.ToString();

        Transform goInfor = genCard.Find("infor");
        goInfor.GetComponent<Image>().sprite = CardSpriteMap[cardData.getSpritePath("infor")];
        goInfor.GetChild(0).GetComponent<TMP_Text>().text = cardData.cardTypeString;
        goInfor.GetChild(1).GetComponent<TMP_Text>().text = cardData.cardExplanation;
    }


}
