using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BattleCardGenerator : MonoBehaviour, IRegisterable
{
    public int GeneratNumber = 0;
    [SerializeField] private Transform _cardParent;
    [SerializeField] private BattleCardHolder _cardHolder;

    [SerializeField] BattleCard _baseBattleCard;
    [SerializeField] private List<BattleCardData> _cardDatas;

    public void Init()
    {
        
    }
    
    //임시 생성 함수
    public BattleCard GenerateBattleCard(int idx)
    {
        if (_cardDatas.Count <= idx)
        {
            Assert.IsTrue(false);
        }

        BattleCard genCard = Instantiate(_baseBattleCard, _cardParent);
        genCard.Init(_cardHolder, _cardDatas[idx], GeneratNumber++);
        
        return genCard;
    }
}
