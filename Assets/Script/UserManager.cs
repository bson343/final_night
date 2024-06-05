using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int HP { get; private set; }
    // �÷��̾� ü�� 
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
    public void SetHP(int hp)
    {
        HP = hp;
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

    // UserManager�� ����� JSON ���ڿ��� �ҷ����� �޼���
    public string LoadMapFromManager()
    {
        return Map;
    }
}