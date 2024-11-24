using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserManager : MonoBehaviour
{
   
    public static UserManager Instance { get; private set; }

    public long UserNum { get; private set; }

    public long DataID { get; private set; }
    
   
    public string UserNickname { get; private set; }

    public int Gold { get; private set; }
  
    public int MaxHP { get; private set; } // 최대 HP
    public int CurrentHP { get; private set; } // 현재 HP

    public int NewGamePlay { get; private set; } // 처음 시작하나 안하나.



    public event Action OnDataChanged;
    public List<int> CardDeckIndex { get; private set; }

    public List<int> HeroCardDeckIndex { get; private set; }

   
    public string Map { get; private set; }
  

    private void Awake()
    {
        // �̱��� �ν��Ͻ� �ʱ�ȭ
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }

   
    public void SetUserNum(long userNum)
    {
        UserNum = userNum;
    }

    
    public void SetUserNickname(string userNickname)
    {
        UserNickname = userNickname;
    }

    
    public void SetData_ID(long dataID)
    {
        DataID = dataID;
    }


  
    public void SetGold(int gold)
    {
        Gold = gold;
    }

    
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

    public void SetNewGamePlay(int newgameplay)
    {
        NewGamePlay = newgameplay;
    }

    public void SetMap(string map)
    {
        Map = map;
    }

  
    public void SaveMapToManager(string json)
    {
        Map = json;
    }

   
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