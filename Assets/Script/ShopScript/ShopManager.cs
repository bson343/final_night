using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Transform cardDisplayParent; // 상점에서 카드를 표시할 부모 오브젝트
    public BattleCard cardPrefab; // 카드 프리팹 (미리 만들어 둔 카드)
    private List<BattleCard> availableCards; // 상점에 표시되는 카드 목록

    void Start()
    {
        GenerateRandomCards(3); // 예: 3개의 랜덤 카드를 생성
    }

    // 랜덤 카드 생성 함수
    public void GenerateRandomCards(int cardCount)
    {
        availableCards = new List<BattleCard>();

        for (int i = 0; i < cardCount; i++)
        {
            int randomCardId = GetRandomCardId(); // 임의의 cardId를 가져오는 함수
            BattleCard newCard = Instantiate(cardPrefab, cardDisplayParent);
            newCard.Init(null, randomCardId); // 카드를 초기화 (여기서 _cardHolder는 null로 설정 가능)

            // 상점에서 이 카드를 선택할 수 있게 하기 위해 클릭 이벤트를 추가
            newCard.GetComponent<Button>().onClick.AddListener(() => OnCardSelected(newCard));

            availableCards.Add(newCard); // 생성된 카드를 상점의 카드 목록에 추가
        }
    }

    // 임의의 cardId를 반환하는 함수 (임시로 1~16 사이의 랜덤 값 반환)
    private int GetRandomCardId()
    {
        return Random.Range(1, 16); // 카드 ID 범위에 맞춰 수정 가능
    }

    // 카드 선택 시 호출되는 함수
    public void OnCardSelected(BattleCard selectedCard)
    {
        AddCardToDeck(selectedCard.cardID); // 선택된 카드를 덱에 추가
        Destroy(selectedCard.gameObject); // 선택한 카드는 상점에서 제거
    }

    // 덱에 카드를 추가하는 함수
    public void AddCardToDeck(int cardId)
    {
        // 여기서 덱에 카드를 추가하는 로직을 구현 (덱 관리 클래스가 있다면 그곳에 추가)
        Debug.Log($"Card {cardId} added to deck.");
    }
}
