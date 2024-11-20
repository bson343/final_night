using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ESortType
{
    Recent,
    Type,
    Cost,
    Name,
    Size
}

public class CardDeckUI : MonoBehaviour
{

    private List<BattleCard> cardList;
    private ESortType sortType = ESortType.Recent;

    [SerializeField]
    private Transform spawnParent;

    [SerializeField]
    private Button[] sortButtons;

    private bool[] isAscending = new bool[4];

}
