using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ECardType
{
    Attack,
    Skill,
    Hero
}

public enum ECardGrade
{
    Common,
    Special,
    Unique
}

public enum ECardFrameData
{
    AbnormalStatus,
    CommonAttack,
    CommonPower,
    CommonSkill,
    SpeicalAttack,
    SpeicalPower,
    SpeicalSkill,
    UniqueAttack,
    UniquePower,
    UniqueSkill
}

[CreateAssetMenu(fileName = "aboutBattleData", menuName = "aboutBattleData/BattleCardData")]
public class BattleCardData : ScriptableObject
{
    [Header("카드 정보")]
    public int id;
    public Sprite cardImage;
    public string cardName;
    public int cost;
    public string[] constants;
    public ECardType cardType;
    public string cardTypeString;
    [Multiline(6)]
    public string cardExplanation;
    //public ECardGrade cardGrade;
    public bool isBezierCurve;
    public bool isExtinction;  // true면 소멸, false면 소멸 안 함 
    //public ECardFrameData cardFrameData;

    [Header("사용효과")]
    public List<UnityEvent> useEffect;
    public string[] effects;

    private Dictionary<string, string> SpritePaths;

    public bool setSpritePaths(Dictionary<string, string> container)
    {
        SpritePaths = container;

        return true;
    }

    public string getSpritePath(string column)
    {
        if (SpritePaths == null)
        {
            Debug.LogError("Dictionary<string, string> SpritePaths is NULL");
            return null;
        }

        return SpritePaths[column];
    }
}
