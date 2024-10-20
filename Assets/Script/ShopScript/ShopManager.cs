using UnityEngine;

public class ShopManager : MonoBehaviour, IRegisterable
{
    [SerializeField]
    private Transform cardShopParent;

    private void Start()
    {
        GetCard();
    }

    public void Init()
    {
        // 초기화 작업이 필요한 경우 이곳에 추가
        Debug.Log("ShopManager initialized.");
    }

    private void GetCard()
    {
        // 카드 생성 로직
        BattleCard card1 = ServiceLocator.Instance.GetService<BattleCardGenerator>().GetRandomCard();
        BattleCard card2 = ServiceLocator.Instance.GetService<BattleCardGenerator>().GetRandomCard();
        BattleCard card3 = ServiceLocator.Instance.GetService<BattleCardGenerator>().GetRandomCard();

        card1.ChangeState(ECardUsage.Gain);
        card2.ChangeState(ECardUsage.Gain);
        card3.ChangeState(ECardUsage.Gain);

        card1.onClickAction = null;
        card2.onClickAction = null;
        card3.onClickAction = null;

        card1.onClickAction += () => OnClickGainCard(card1);
        card2.onClickAction += () => OnClickGainCard(card2);
        card3.onClickAction += () => OnClickGainCard(card3);

        card1.transform.SetParent(cardShopParent);
        card2.transform.SetParent(cardShopParent);
        card3.transform.SetParent(cardShopParent);

        card1.transform.localScale = Vector3.one;
        card2.transform.localScale = Vector3.one;
        card3.transform.localScale = Vector3.one;
    }

    private void OnClickGainCard(BattleCard clickedCard)
    {
        int cardID = clickedCard.cardID;
        Debug.Log("클릭한 카드 ID: " + cardID);
        UserManager.Instance.CardDeckIndex.Add(cardID);
    }
}
