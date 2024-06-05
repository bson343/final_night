using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card Game/HeroCard")]
public class HeroCardData : MonoBehaviour

{
    public string HeroCardName;
    public int HeroCardValue;
    public Sprite HeroCardImage;

    // 다른 카드 속성들 추가 가능
}
