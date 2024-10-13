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

    [SerializeField] private BattleCard _baseBattleCard;
    [SerializeField] private List<BattleCardData> _MyDeckList = new List<BattleCardData>();
    [SerializeField] BattleCard _defaultDummyCard;
    [SerializeField] private List<BattleCardData> _cardDatas;

    Dictionary<decimal, BattleCardData> CardDataMap => ResourceManager.Instance.CardDataMap;
    Dictionary<string, Sprite> CardSpriteMap => ResourceManager.Instance.CardSpriteMap;

    public void Init()
    {
        // 필요한 초기화 작업이 있으면 여기에서 처리
    }

    /*public void Start() // 게임 시작 시 실행, 덱 설정 및 카드 생성
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
    }*/

    // 임의의 인덱스로부터 랜덤 카드 생성
    //Deprecated
    public BattleCard GeneratorRandomCard()
    {
        return GenerateBattleCard(Random.Range(0, _MyDeckList.Count));
    }

    // 모든 덱 카드를 생성
    public void GenerateAllCards()
    {
        for (int i = 0; i < _MyDeckList.Count; i++)
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
    //Deprecated
    public BattleCard GenerateBattleCard(int cardIdx)
    {
        if (_MyDeckList.Count <= cardIdx)
        {
            Debug.LogError("Invalid card index for battle card generation.");
            Assert.IsTrue(false);
        }

        return GenerateBattleCardFromData(_MyDeckList[cardIdx]);
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
