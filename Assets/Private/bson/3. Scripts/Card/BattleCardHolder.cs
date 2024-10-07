using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCardHolder : MonoBehaviour
{
    public BattleCard selectedCard;  // 현재 집은 카드
    public bool isDrag = false;

    private bool isBattle = false;

    [SerializeField]
    private BattleCard _battleCardPrefab;

    [SerializeField]
    private MyList<BattleCard> testDeck;

    [SerializeField]
    private Transform _cardTransform;   //MyCard

    [SerializeField]
    private Transform _cardDeckTransform; // 카드 덱 위치
    [SerializeField]
    private Transform _cardCemetryTransform; // 카드 묘지 위치
    [SerializeField]
    private Transform _cardExtinctionTransform; // 카드 소멸 위치

    [SerializeField]
    private Text _cardDeckCountText;
    [SerializeField]
    private Text _cardCemetryCountText;
    [SerializeField]
    private Text _cardExtinctionCountText;

    [SerializeField]
    private MyList<BattleCard> _cardDeck;  // 카드 덱
    [SerializeField]
    private MyList<BattleCard> _cardHands; // 카드 패
    [SerializeField]
    private MyList<BattleCard> _cardCemetry; // 카드 묘지
    [SerializeField]
    private MyList<BattleCard> _cardExtinction; // 카드 소멸

    [SerializeField]
    private BezierCurve _bezierCurve;

    [SerializeField]
    private float offsetAngle;
    [SerializeField]
    private float angle;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float overSidePosition = 50f;  // 카드 마우스 오버 시 양옆으로 이동하는 값
    [SerializeField]
    private float overScale = 1.3f;  // 카드 마우스 오버 시 해당 카드가 커지는 값
    [SerializeField]
    private float overUpPosition = 200f; // 카드 마우스 오버 시 해당 카드 Y위치 증가량

    private List<BattleCard> _temporaryList = new List<BattleCard>();

    public BezierCurve BezierCurve => _bezierCurve;

    public void ResumeBattle(List<BattleCard> myCard)
    {
        if (!isBattle)
            return;

        foreach (BattleCard card in myCard)
        {
            card.ChangeState(ECardUsage.Battle);

            card.transform.SetParent(_cardTransform);
            card.onClickAction = null;
        }

        foreach (BattleCard card in _cardDeck)
        {
            card.transform.localPosition = _cardDeckTransform.localPosition;
            card.transform.localEulerAngles = Vector3.zero;
            card.transform.localScale = Vector3.zero;
        }

        foreach (BattleCard card in _cardCemetry)
        {
            card.transform.localPosition = _cardCemetryTransform.localPosition;
            card.transform.localEulerAngles = Vector3.zero;
            card.transform.localScale = Vector3.zero;
        }

        Relocation();
    }

    public void StartBattle(List<BattleCard> myCard)
    {
        isBattle = true;

        // 초기화
        selectedCard = null;
        isDrag = false;

        // 필요없는 카드는 제거
        while (_temporaryList.Count != 0)
        {
            BattleCard card = _temporaryList[0];
            _temporaryList.Remove(card);
            Destroy(card.gameObject);
        }

        _cardExtinctionTransform.gameObject.SetActive(false);

        _cardDeck = new MyList<BattleCard>();
        _cardHands = new MyList<BattleCard>();
        _cardCemetry = new MyList<BattleCard>();
        _cardExtinction = new MyList<BattleCard>();

        _cardDeck.onChangeList = null;
        _cardHands.onChangeList = null;
        _cardCemetry.onChangeList = null;
        _cardExtinction.onChangeList = null;

        _cardDeck.onChangeList += ShowCardCount;
        _cardHands.onChangeList += ShowCardCount;
        _cardCemetry.onChangeList += ShowCardCount;
        _cardExtinction.onChangeList += ShowCardCount;

        _temporaryList = new List<BattleCard>();

        // 내 카드 넣기
        foreach (BattleCard card in myCard)
        {
            // 위치 초기화
            card.transform.SetParent(_cardTransform);

            card.ChangeState(ECardUsage.Battle);

            card.transform.localPosition = _cardDeckTransform.localPosition;
            card.transform.localEulerAngles = Vector3.zero;
            card.transform.localScale = Vector3.zero;

            _cardDeck.Add(card);
        }

        // 셔플
        Util.ShuffleList(_cardDeck);
    }

    private void ShowCardCount()
    {
        _cardDeckCountText.text = _cardDeck.Count.ToString();
        _cardCemetryCountText.text = _cardCemetry.Count.ToString();
        _cardExtinctionCountText.text = _cardExtinction.Count.ToString();
    }

    // 일시적으로 생성
    public void AddCardTemporary(BattleCard card)
    {
        card.transform.localPosition = _cardDeckTransform.localPosition;
        card.transform.localEulerAngles = Vector3.zero;
        card.transform.localScale = Vector3.zero;

        _temporaryList.Add(card);
        _cardDeck.Add(card);

        // 셔플
        Util.ShuffleList(_cardDeck);
    }

    public void EndBattle(List<BattleCard> myCard)
    {
        DiscardAllCard();
        isBattle = false;
        _cardExtinctionTransform.gameObject.SetActive(false);
    }


    // 소멸
    public void Extinction(BattleCard card)
    {
        _cardHands.Remove(card);
        _cardExtinction.Add(card);

        Relocation();
        card.CardController.MoveCard(_cardExtinctionTransform.localPosition, Vector3.zero, Vector3.zero);

        // 소멸된 카드가 1장이라도 있으면 소멸 UI 생성
        if (_cardExtinction.Count > 0)
        {
            _cardExtinctionTransform.gameObject.SetActive(true);
        }
        else
        {
            _cardExtinctionTransform.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// 카드 드로우
    /// </summary>
    public void DrawCard()
    {
        // 패에 카드 10장이상이면 더이상 카드를 드로우하지 못함
        if (_cardHands.Count >= 10)
        {
            return;
        }

        // 뽑을 카드가 없으면 묘지에 있는 카드를 전부 덱에 넣고 셔플한 다음 드로우
        if (_cardDeck.Count <= 0)
        {
            Debug.Log("뽑을 카드가 없음");
            ReturnToDeck();
            Util.ShuffleList(_cardDeck);
        }

        // 덱에 한 장 뽑아서 패에 넣음
        BattleCard card = _cardDeck[_cardDeck.Count - 1];
        _cardHands.Add(card);
        _cardDeck.Remove(card);

        // 뽑으면 카드 레이캐스트 활성화
        card.CardController.SetActiveRaycast(true);

        Relocation();
    }

    /// <summary>
    /// 카드 버리기
    /// </summary>
    /// <param name="card">버릴 카드</param>
    public void DiscardCard(BattleCard card)
    {
        _cardCemetry.Add(card);
        _cardHands.Remove(card);

        Relocation();
        card.CardController.MoveCard(_cardCemetryTransform.localPosition, Vector3.zero, Vector3.zero);

        // 버리면 카드 레이캐스트 비활성화
        card.CardController.SetActiveRaycast(false);
    }

    /// <summary>
    /// 패에 있는 카드 마우스 오버 시 호출
    /// </summary>
    /// <param name="card">마우스를 댄 카드</param>
    public void OverCard(BattleCard card)
    {
        float startTheta = GetStartTheta();

        int index = GetCardIndex(card);

        // 해당 카드를 못 찾으면 return;
        if (index == -1)
            return;

        // 카드 배치
        for (int i = 0; i < _cardHands.Count; i++)
        {
            _cardHands[i].transform.SetAsFirstSibling();

            float theta = startTheta + angle * i;
            Vector3 targetPos = transform.position + new Vector3(Mathf.Cos(theta * Mathf.Deg2Rad), Mathf.Sin(theta * Mathf.Deg2Rad), 0) * distance;
            Vector3 targetRot = new Vector3(0f, 0f, theta - 90);
            Vector3 targetScl = Vector3.one;

            if (i < index)
            {
                targetPos += Vector3.right * overSidePosition;
            }
            else if (i > index)
            {
                targetPos -= Vector3.right * overSidePosition;
            }
            else
            {
                targetPos += Vector3.up * overUpPosition;
                targetRot = Vector3.zero;
                targetScl = Vector3.one * overScale;
            }

            _cardHands[i].CardController.MoveCard(targetPos, targetRot, targetScl);
            _cardHands[i].transform.SetAsFirstSibling();
        }

        _cardHands[index].transform.SetAsLastSibling();
    }

    public void MoveCenter(BattleCard card)
    {
        Vector3 targetPos = Vector3.up * (-Screen.height / 2 + overUpPosition / 2);
        Vector3 targetRot = Vector3.zero;
        Vector3 targetScl = Vector3.one * overScale;

        card.CardController.MoveCard(targetPos, targetRot, targetScl);
    }

    // 패에 있는 모든 카드 버리기
    public void DiscardAllCard()
    {
        while (_cardHands.Count != 0)
        {
            BattleCard card = _cardHands[_cardHands.Count - 1];
            DiscardCard(card);
        }
    }

    /// <summary>
    /// 패에 있는 카드 재배치
    /// </summary>
    public void Relocation()
    {
        float startTheta = GetStartTheta();

        // 카드 배치
        for (int i = 0; i < _cardHands.Count; i++)
        {
            _cardHands[i].transform.SetAsFirstSibling();

            float theta = startTheta + angle * i;
            Vector3 targetPos = transform.position + new Vector3(Mathf.Cos(theta * Mathf.Deg2Rad), Mathf.Sin(theta * Mathf.Deg2Rad), 0) * distance;
            Vector3 targetRot = new Vector3(0f, 0f, theta - 90);
            Vector3 targetScl = Vector3.one;

            _cardHands[i].CardController.MoveCard(targetPos, targetRot, targetScl);
            _cardHands[i].transform.SetAsFirstSibling();
        }
    }


    // _cardHands의 갯수에 따른 패의 시작 각도를 반환
    private float GetStartTheta()
    {
        float startTheta = 0;
        if (_cardHands.Count % 2 == 0)
            startTheta -= _cardHands.Count / 2 * angle - (angle / 2) - 90;
        else
            startTheta -= (_cardHands.Count / 2) * angle - 90;

        return startTheta;
    }

    // 해당 카드가 패에 몇번째있는지 반환
    private int GetCardIndex(BattleCard card)
    {
        for (int i = 0; i < _cardHands.Count; i++)
        {
            if (card == _cardHands[i])
            {
                return i;
            }
        }
        return -1;
    }

    // 묘지 카드 전부 덱으로 귀환
    private void ReturnToDeck()
    {
        while (_cardCemetry.Count != 0)
        {
            BattleCard card = _cardCemetry[_cardCemetry.Count - 1];
            _cardDeck.Add(card);
            _cardCemetry.Remove(card);

            card.transform.localPosition = _cardDeckTransform.localPosition;
        }
    }


    private void Start()
    {
       
    }
}
