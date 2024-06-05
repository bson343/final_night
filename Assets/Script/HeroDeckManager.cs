using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDeckManager : MonoBehaviour
{
    public List<HeroCardData> herodeck = new List<HeroCardData>();

    // 영웅덱에 영웅카드를 추가하는 함수
    public void AddCardToDeck(HeroCardData herocard)
    {
        herodeck.Add(herocard);
    }

    // 영웅에서 영웅카드를 제거하는 함수
    public void RemoveCardFromDeck(HeroCardData herocard)
    {
        herodeck.Remove(herocard);
    }
}
