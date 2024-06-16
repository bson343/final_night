using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting.FullSerializer;
using static GamePlayDataLodingManager;

public class GameData
{

        public long Id;
        public User User;
        public int Playtime;
        public int ClearCount;
        public string gameData; // 
    
}

[System.Serializable]
public class GameDataContent
{
    public int MaxHP;
    public int CurrentHP;
    public int CurrentSP;
    public int Gold;
    public List<int> CardDeckIndex;
    public List<int> HeroCardDeckIndex;
    public string Map;
    // gameData JSON에 포함된 다른 필드들도 여기에 추가
}

[System.Serializable]
public class User
{
    public long UserNumber;
    public string UserId;
    public string UserPassword;
    public string Nickname;
}
