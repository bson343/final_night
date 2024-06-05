using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "Card Game/Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public int cardValue;
    public Sprite cardImage;

    // 다른 카드 속성들 추가 가능
}
