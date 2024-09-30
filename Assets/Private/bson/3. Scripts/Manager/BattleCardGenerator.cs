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

    private List<BattleCardData> MybattleCardDataList = new List<BattleCardData>();

    [SerializeField] private List<BattleCardData> _MydeckList;

    public void Init()
    {
        
    }

    public void Start() // 시작할때 실행 (리스트에 덱 부여)
    {
        List<int> deckIndices = UserManager.Instance.CardDeckIndex;

        foreach (int id in deckIndices)
        {
            // Resources 폴더에서 BattleCardData를 로드
            BattleCardData cardData = Resources.Load<BattleCardData>("Data/CardData/BattleCardData_" + id);

            // 리소스를 찾았는지 확인
            if (cardData != null)
            {
                _MydeckList.Add(cardData); //덱에 카드추가
                Debug.Log("Loaded BattleCardData: " + cardData.cardName);
            }
            else
            {
                Debug.LogError("BattleCardData with ID " + id + " not found.");
            }
        }
    }

    public BattleCard GeneratorRandomCard()
    {
        return GenerateBattleCard(Random.Range(0, _MydeckList.Count));
    }

    public void GenerateAllCards()
    {
        // _MydeckList에 있는 모든 카드를 순차적으로 생성
        for (int i = 0; i < _MydeckList.Count; i++)
        {
            GenerateBattleCard(i); // 각 인덱스를 통해 카드를 생성
        }
    }

    //임시 생성 함수
    public BattleCard GenerateBattleCard(int cardIdx)
    {
        if (_MydeckList.Count <= cardIdx)
        {
            Assert.IsTrue(false);
        }

        BattleCard genCard = Instantiate(_baseBattleCard, _cardParent);
        genCard.Init(_cardHolder, _MydeckList[cardIdx], GeneratNumber++);
        
        return genCard;
    }
}
