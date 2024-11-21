using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class IndentObject : MonoBehaviour
{
    [SerializeField] private Image indentImage;
    [SerializeField] private Text indentText;

    protected BattleManager battleManager => ServiceLocator.Instance.GetService<BattleManager>();
    public IndentData indentData { get; private set; }
    public int turn;

    public void Init(IndentData indentData, int turn)
    {
        this.indentData = indentData;
        this.turn = turn;

        indentImage.sprite = indentData.indentSprite;
        UpdateIndent();
    }

    public void AddTurn(int turn)
    {
        Debug.Log("증가 얍");
        this.turn += turn;
        UpdateIndent();
    }


    public void UpdateIndent()
    {
        if (indentData.indent == EIndent.Strength)
        {
            // 힘 버프일 경우 플레이어의 현재 파워를 표시
            indentText.text = $"{battleManager.Player.PlayerStat.Power}";
        }
        else if (indentData.isShowTurn)
        {
            // 일반적인 경우 턴 수를 표시
            indentText.text = turn.ToString();
        }
        else
        {
            indentText.text = "";
        }
    }

}
