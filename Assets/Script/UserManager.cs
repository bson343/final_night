using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static UserManager Instance { get; private set; }

    // userNum�� ������ ���� (Ŭ���� �ܺο����� ���� ���� �� ������ ������ ���� ������ ����)
    public long UserNum { get; private set; }

    public long DataID { get; private set; } // �����͸� �����Ҷ� ����ϴ� ID�� �޾ƿ��� ���� , �̰ɷ� �����Ҷ� ������ �ۼ��Ѵ�.
    
    // userNickname�� ������ ����
    public string UserNickname { get; private set; }

    public int Gold { get; private set; }
    // �÷��̾� ��� 
    // �÷��̾� ü�� 
    public int MaxHP { get; private set; } // 최대 HP
    public int CurrentHP { get; private set; } // 현재 HP

    public event Action OnDataChanged;
    public List<int> CardDeckIndex { get; private set; }

    public List<int> HeroCardDeckIndex { get; private set; }

    public List<CardData> CardDeck { get; private set; }
    // �÷��̾� ��
    public List<HeroCardData> HeroCardDeck { get; private set; }
    // �÷��̾� ���� ī�� ��
    public string Map { get; private set; }
    // �÷��̾� �� ����

    private void Awake()
    {
        // �̱��� �ν��Ͻ� �ʱ�ȭ
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ü�� ���ο� �� �ε� �ÿ��� �ı����� �ʰ� ��
        }
        else
        {
            Destroy(gameObject); // �ν��Ͻ��� �̹� �����ϸ� ���ο� ��ü�� �ı�
        }
    }

    // userNum ���� �޼���
    public void SetUserNum(long userNum)
    {
        UserNum = userNum;
    }

    // userNickname ���� �޼���
    public void SetUserNickname(string userNickname)
    {
        UserNickname = userNickname;
    }

    // ������ ID ���� �޼���
    public void SetData_ID(long dataID)
    {
        DataID = dataID;
    }


    // ��� ���� �����ϴ� �޼���
    public void SetGold(int gold)
    {
        Gold = gold;
    }

    // ü�� ���� �����ϴ� �޼���
    public void SetMaxHP(int hp)
    {
        MaxHP = hp;
    }

    public void SetCurrentHP(int hp)
    {
        CurrentHP = hp;
    }

    public void SetCardDeckindex(List<int> cardDeck) // list 숫자형으로 테스트
    {
        CardDeckIndex = cardDeck;
    }

    public void SetHeroCardDeckindex(List<int> herocardDeck) // list 숫자형으로 테스트
    {
        HeroCardDeckIndex = herocardDeck;
    }

    // ī�� ���� �����ϴ� �޼���
    public void SetCardDeck(List<CardData> cardDeck)
    {
        CardDeck = cardDeck;
    }

    // ���� ī�� ���� �����ϴ� �޼���
    public void SetHeroCardDeck(List<HeroCardData> heroCardDeck)
    {
        HeroCardDeck = heroCardDeck;
    }

    // �� ������ �����ϴ� �޼���
    public void SetMap(string map)
    {
        Map = map;
    }

    // �����͸� JSON ���ڿ��� ��ȯ�Ͽ� UserManager�� �����ϴ� �޼���
    public void SaveMapToManager(string json)
    {
        Map = json;
    }

    // UserManager에서 오브젝트인 맵을 돌려줌
    public object LoadMapFromManager()
    {
        return Map;
    }

    public void UpdateGold(int newGold)
    {
        Gold = newGold;
        OnDataChanged?.Invoke();
    }

    public void UpdateMaxHP(int newHP)
    {
        MaxHP = newHP;
        OnDataChanged?.Invoke();
    }

    public void UpdateCurrentHP(int newHP)
    {
        CurrentHP = newHP;
        OnDataChanged?.Invoke();
    }
}