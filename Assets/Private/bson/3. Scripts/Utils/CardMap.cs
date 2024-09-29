using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMap : GlobalSingleton<CardMap>
{
    bool firstInit = false;

    public Dictionary<decimal, BattleCardData> CardDataMap { get; private set; }
    public Dictionary<string, Sprite> CardSpriteMap { get; private set; }

    public List<int> AttackCardIdList { get; private set; }
    public List<int> SkillCardIdList { get; private set; }
    public List<int> HeroCardIdList { get; private set; }

    public void Init(Dictionary<decimal, BattleCardData> CardDataMap, Dictionary<string, Sprite> CardSpriteMap)
    {
        if (firstInit == true)
        {
            return;
        }

        this.CardSpriteMap = CardSpriteMap;
        this.CardDataMap = CardDataMap;

        firstInit = true;
    }

    public void Init(CardResourceLoader loader)
    {
        if (firstInit == true)
        {
            return;
        }

        this.CardSpriteMap = loader.CardSpriteMap;
        this.CardDataMap = loader.CardDataMap;
        this.AttackCardIdList = loader.AttackCardIdList;
        this.SkillCardIdList = loader.SkillCardIdList;
        this.HeroCardIdList = loader.HeroCardIdList;

        firstInit = true;

    }
}
