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
    [SerializeField] BattleCard _defaultDummyCard;

    Dictionary<decimal, BattleCardData> CardDataMap => ResourceManager.Instance.CardDataMap;
    Dictionary<string, Sprite> CardSpriteMap => ResourceManager.Instance.CardSpriteMap;

    public void Init()
    {
        // 필요한 초기화 작업이 있으면 여기에서 처리
    }

    public BattleCard GetRandomCard()
    {
        GenerateRandomCard randomCardIdGenerator = FindObjectOfType<GenerateRandomCard>();

        int cardId = randomCardIdGenerator.GetUniqueRandomCardId();


        return GenBatCard(cardId); // 랜덤으로 선택된 카드 반환
    }

        public BattleCard GenBatCard(int cardId)
    {
        if (
            !(ResourceManager.Instance.AttackCardIdList.Contains(cardId) 
            || ResourceManager.Instance.SkillCardIdList.Contains(cardId)
            || ResourceManager.Instance.HeroCardIdList.Contains(cardId))
            )
        {
            Assert.IsTrue(false, "카드를 생성하는데 필요한 리소스가 로드되지 않음");
        }

        BattleCard genCard = Instantiate(_defaultDummyCard, _cardParent);
        //updateCardResource(genCard.gameObject.transform, cardId);
        genCard.Init(_cardHolder, cardId);

        return genCard;
    }
}
