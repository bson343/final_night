using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum ESortType
{
    Recent,
    Type,
    Cost,
    Name,
    Size
}

public class CardDeckUI : MonoBehaviour
{

    private List<BattleCard> cardList;
    private ESortType sortType = ESortType.Recent;

    private int _preIndex;

    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private Transform spawnParent;

    [SerializeField]
    private Button[] sortButtons;

    [SerializeField]
    private Image[] sortDirImage;

    private bool[] isAscending = new bool[4];

    private BattleManager battleManager => ServiceLocator.Instance.GetService<BattleManager>();

    private void Awake()
    {
        for (int i = 0; i < sortButtons.Length; i++)
        {
            ESortType sortTypeIndex = (ESortType)i;
            int index = i;
            sortButtons[i].onClick.AddListener(() => sortType = sortTypeIndex);
            sortButtons[i].onClick.AddListener(() => Sort(index));
        }
    }

    public void Show()
    {
        cardList = new List<BattleCard>();
        cardList = battleManager.Player.CardList;
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].ChangeState(ECardUsage.Check);
            cardList[i].transform.SetParent(spawnParent);
            cardList[i].transform.localEulerAngles = Vector3.zero;
            cardList[i].transform.localScale = Vector3.one;
            cardList[i].onClickAction = null;
        }

        // 내림차순으로 초기화
        for (int i = 0; i < isAscending.Length; i++)
        {
            isAscending[i] = false;
            sortDirImage[i].transform.localScale = Vector3.one;
        }

        // 카드 수에 맞게 높이 조정
        int cardsRow = (cardList.Count - 1) / 5;
        content.sizeDelta += new Vector2(0, (cardsRow - 1) * 400f);

        Sort((int)ESortType.Recent);
    }

    public void Hide()
    {
        content.sizeDelta = Vector2.up * Screen.height;
    }

    /// <summary>
    /// 카드를 어떤 기준으로 정렬해줌
    /// </summary>
    /// <param name="isAscending">오름차순인가? </param>
    private void Sort(int index)
    {
        Debug.Log("정렬합니다. " + sortType);

        for (int i = 0; i < sortButtons.Length; i++)
        {
            foreach (Image childImage in sortButtons[i].GetComponentsInChildren<Image>())
            {
                childImage.color = (i == index) ? Color.yellow : Color.white;
            }
        }

        switch (sortType)
        {
            case ESortType.Recent:
                cardList = cardList.OrderBy(x => x.generateNumber).ToList();
                break;
            case ESortType.Type:
                cardList = cardList.OrderBy(x => x.cardType).ThenBy(x => x.generateNumber).ToList();
                break;
            case ESortType.Cost:
                cardList = cardList.OrderBy(x => x.cost).ThenBy(x => x.generateNumber).ToList();
                break;
            case ESortType.Name:
                cardList = cardList.OrderBy(x => x.cardName).ThenBy(x => x.generateNumber).ToList();
                break;
        }

        if (isAscending[index])
        {
            cardList.ForEach(card => card.transform.SetAsFirstSibling());
            isAscending[index] = false;
        }
        else
        {
            cardList.ForEach(card => card.transform.SetAsLastSibling());
            isAscending[index] = true;
        }

        sortDirImage[index].transform.localScale = new Vector3(sortDirImage[index].transform.localScale.x,
            sortDirImage[index].transform.localScale.y * -1, sortDirImage[index].transform.localScale.z);

        _preIndex = index;
    }

    public void Exit()
    {
        battleManager.Player.ResumeBattle();
        gameObject.SetActive(false);
    }

}
