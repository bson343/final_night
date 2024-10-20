using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RewardCardGenerator : MonoBehaviour
{
    [SerializeField] BattleCard _defaultDummyCard;
    [SerializeField] private Transform _cardParent;
    [SerializeField] private BattleCardHolder _cardHolder;


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
