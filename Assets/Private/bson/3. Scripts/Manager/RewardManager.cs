using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour, IRegisterable
{
    [SerializeField]
    private BaseUI inRewardUI;

    [SerializeField]
    private GameObject cardRewardGameObject;
    [SerializeField]
    private Transform cardRewardParent;
    [SerializeField]
    Button moneyRewardButton;
    [SerializeField]
    TMP_Text moneyRewardText;

    [SerializeField]
    Button cardRewardButton;

    [SerializeField]
    private TMP_Text[] shopCardPriceTexts;
  


    [SerializeField]
    private Button passRewardButton;
    [SerializeField]
    private Button moveRewardButton;

    private int[] shopCardPrices = new int[3];
    private BattleCard[] shopCards = new BattleCard[3];

    private BattleCardGenerator cardGenerator => ServiceLocator.Instance.GetService<BattleCardGenerator>();
    private BattleManager battleManager => ServiceLocator.Instance.GetService<BattleManager>();
    private UIManager UIManager => ServiceLocator.Instance.GetService<UIManager>();




    public void Init()
    {
        moveRewardButton.onClick.AddListener(() => {
            if (battleManager.testMode == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                return;
            }
            NightSceneManager.Instance.UnloadScene();
        });
    }

    // 전투
    public void ShowReward(BattleData battleData)
    {
        cardRewardParent.DestroyAllChild();
        moneyRewardButton.interactable = true;

        // 보상창 켜주기
        UIManager.ShowThisUI(inRewardUI);

        // 전투 끝나고 무조건 카드는 주기 때문에 카드는 일단 생성
        GetCard();

        // 돈
        int money = Random.Range(battleData.minMoney, battleData.maxMoney);
        moneyRewardText.text = money + " 골드를 획득합니다.";
        moneyRewardButton.onClick.AddListener(() => GetMoney(money));
        moneyRewardButton.onClick.AddListener(() => moneyRewardButton.interactable = false);


        // 랜덤 카드 선택
        cardRewardButton.onClick.AddListener(() => cardRewardGameObject.SetActive(true));
    }

    // 카드 3장 생성
    public void GetCard()
    {
        GenerateRandomCard randomCardIdGenerator = FindObjectOfType<GenerateRandomCard>();

        BattleCard card1 = cardGenerator.CreateAndSetupCard(randomCardIdGenerator);
        BattleCard card2 = cardGenerator.CreateAndSetupCard(randomCardIdGenerator);
        BattleCard card3 = cardGenerator.CreateAndSetupCard(randomCardIdGenerator);

        card1.onClickAction += () => OnClickGainCard(card1);
        card2.onClickAction += () => OnClickGainCard(card2);
        card3.onClickAction += () => OnClickGainCard(card3);

        SetCardParentAndScale(card1);
        SetCardParentAndScale(card2);
        SetCardParentAndScale(card3);
    }

    private void SetCardParentAndScale(BattleCard card)
    {
        card.transform.SetParent(cardRewardParent);
        card.transform.localScale = Vector3.one;
    }


    public void GetshopCard()
    {
        GenerateRandomCard randomCardIdGenerator = FindObjectOfType<GenerateRandomCard>();

        for (int i = 0; i < 3; i++)
        {
            BattleCard card = cardGenerator.CreateAndSetupCard(randomCardIdGenerator);
            int index = i;  // 클로저 문제 해결을 위해 로컬 변수로 캡처

            shopCardPrices[index] = Random.Range(50, 101);
            shopCards[index] = card;

            card.ChangeState(ECardUsage.Gain);
            card.onClickAction = null;
            card.onClickAction += (() => OnClickBuyCard(card, index));

            card.transform.SetParent(cardRewardParent);
            card.transform.localScale = Vector3.one;

            // 가격표 설정
            shopCardPriceTexts[index].text = shopCardPrices[index] + " 골드";
        }
    }

    private void OnClickBuyCard(BattleCard clickedCard, int index)
    {
        if (clickedCard == null)
        {
            Debug.LogWarning("clickedCard가 null입니다. 올바르게 초기화되었는지 확인하세요.");
            return;
        }

        if (UserManager.Instance.Gold >= shopCardPrices[index])
        {
            UserManager.Instance.UpdateGold(UserManager.Instance.Gold - shopCardPrices[index]);
            UserManager.Instance.CardDeckIndex.Add(clickedCard.cardID);
            Debug.Log("카드를 구매했습니다: " + clickedCard.cardID);

            clickedCard.onClickAction = null; // 카드 클릭 이벤트 제거

            // 카드 UI를 흐리게 처리
            Image cardImage = clickedCard.transform.GetComponent<Image>();
            if (cardImage != null)
            {
                cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 0.5f); // 반투명하게 처리
            }
            else
            {
                Debug.LogWarning("Image 컴포넌트가 없습니다. 카드 UI를 업데이트할 수 없습니다.");
            }

            // 가격표에 구매 완료 표시
            shopCardPriceTexts[index].text = "구매 완료";
        }
        else
        {
            Debug.LogWarning("골드가 부족합니다.");
        }
    }



    // 보상 카드를 눌렀을 때 실행될 함수
    private void OnClickGainCard(BattleCard clickedCard)
    {
        int cardID = clickedCard.cardID;
        Debug.Log("클릭한 카드 ID: " + cardID);
        UserManager.Instance.CardDeckIndex.Add(cardID);
        cardRewardGameObject.gameObject.SetActive(false);
    }

    private void GetMoney(int value)
    {
        UserManager.Instance.UpdateGold(UserManager.Instance.Gold+value);
    }

}
