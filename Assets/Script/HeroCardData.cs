using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New HeroCard", menuName = "Card Game/HeroCard")]
public class HeroCardData : ScriptableObject
{
    public string cardName;
    public int cardValue;
    public Sprite cardImage;

    // 추가적인 카드 속성들
}
