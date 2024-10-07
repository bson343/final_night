using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BattleCardGenerator : MonoBehaviour, IRegisterable
{
    public int GeneratNumber = 0;
    [SerializeField] private Transform _cardParent;
    [SerializeField] private BattleCardHolder _cardHolder;

    [SerializeField] private BattleCard _baseBattleCard;
    [SerializeField] private List<BattleCardData> _MydeckList = new List<BattleCardData>();

    public void Init()
    {
        // 필요한 초기화 작업이 있으면 여기에서 처리
    }

    public void Start() // 게임 시작 시 실행, 덱 설정 및 카드 생성
    {
        List<int> deckIndices = UserManager.Instance.CardDeckIndex;
        List<BattleCard> myCardList = new List<BattleCard>();

        foreach (int id in deckIndices)
        {
            // 카드 데이터 로드
            BattleCardData cardData = Resources.Load<BattleCardData>("Data/CardData/BattleCardData_" + id);
            if (cardData != null)
            {
                _MydeckList.Add(cardData); // 덱 데이터 추가
                BattleCard card = GenerateBattleCardFromData(cardData); // 카드 객체 생성
                myCardList.Add(card);  // 덱 리스트에 카드 추가
                Debug.Log("Loaded BattleCardData: " + cardData.cardName);
            }
            else
            {
                Debug.LogError("BattleCardData with ID " + id + " not found.");
            }
        }

        _cardHolder.StartBattle(myCardList); // 카드 홀더에 덱 전달
        Debug.Log("덱 생성 완료");
    }

    // 임의의 인덱스로부터 랜덤 카드 생성
    public BattleCard GeneratorRandomCard()
    {
        return GenerateBattleCard(Random.Range(0, _MydeckList.Count));
    }

    // 모든 덱 카드를 생성
    public void GenerateAllCards()
    {
        for (int i = 0; i < _MydeckList.Count; i++)
        {
            GenerateBattleCard(i);
        }
    }

    // 카드 데이터를 기반으로 BattleCard 객체를 생성하는 메서드 (중복 코드 제거)
    private BattleCard GenerateBattleCardFromData(BattleCardData cardData)
    {
        BattleCard genCard = Instantiate(_baseBattleCard, _cardParent);
        genCard.Init(_cardHolder, cardData, GeneratNumber++);
        return genCard;
    }

    // 인덱스로부터 BattleCard를 생성하는 메서드
    public BattleCard GenerateBattleCard(int cardIdx)
    {
        if (_MydeckList.Count <= cardIdx)
        {
            Debug.LogError("Invalid card index for battle card generation.");
            Assert.IsTrue(false);
        }

        return GenerateBattleCardFromData(_MydeckList[cardIdx]);
    }
}
