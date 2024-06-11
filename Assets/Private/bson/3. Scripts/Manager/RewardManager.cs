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
    private Button passRewardButton;
    [SerializeField]
    private Button moveRewardButton;
    

    private BattleCardGenerator cardGenerator => ServiceLocator.Instance.GetService<BattleCardGenerator>();
    private BattleManager battleManager => ServiceLocator.Instance.GetService<BattleManager>();
    private UIManager UIManager => ServiceLocator.Instance.GetService<UIManager>();


    public void Init()
    {
        moveRewardButton.onClick.AddListener(() => {
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
        moneyRewardText.text = money + "골드를 획득합니다.";
        moneyRewardButton.onClick.AddListener(() => GetMoney(money));
        moneyRewardButton.onClick.AddListener(() => moneyRewardButton.interactable = false);



        // 랜덤 카드 선택
        cardRewardButton.onClick.AddListener(() => cardRewardGameObject.SetActive(true));
    }

    // 카드 3장 생성
    private void GetCard()
    {
        BattleCard card1 = cardGenerator.GeneratorRandomCard();
        BattleCard card2 = cardGenerator.GeneratorRandomCard();
        BattleCard card3 = cardGenerator.GeneratorRandomCard();

        card1.ChangeState(ECardUsage.Gain);
        card2.ChangeState(ECardUsage.Gain);
        card3.ChangeState(ECardUsage.Gain);

        card1.onClickAction = null;
        card2.onClickAction = null;
        card3.onClickAction = null;

        card1.onClickAction += (() => OnClickGainCard());
        card2.onClickAction += (() => OnClickGainCard());
        card3.onClickAction += (() => OnClickGainCard());

        card1.transform.SetParent(cardRewardParent);
        card2.transform.SetParent(cardRewardParent);
        card3.transform.SetParent(cardRewardParent);

        card1.transform.localScale = Vector3.one;
        card2.transform.localScale = Vector3.one;
        card3.transform.localScale = Vector3.one;
    }

    // 보상 카드를 눌렀을 때 실행될 함수
    private void OnClickGainCard()
    {
        // 카드를 얻고
        // 카드 보상 창을 닫고
        // 카드 보상을 없애고

        cardRewardGameObject.gameObject.SetActive(false);
    }

    private void GetMoney(int value)
    {
        battleManager.Player.PlayerStat.Money += value;
    }

}
