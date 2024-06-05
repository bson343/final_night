using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBattleUI : BaseUI
{
    [SerializeField]
    private BattleTurnUI battleTurnUI; // 턴이 시작되면 나오는 UI
    [SerializeField]
    private TurnEndUI turnEndUI; // 턴을 끝낼 수 있는 버튼 UI

    public bool EndStartTurn => !battleTurnUI.gameObject.activeSelf;

    private BattleManager battleManager => ServiceLocator.Instance.GetService<BattleManager>();

    private void Awake()
    {
        battleManager.onStartMyTurn += (() => battleTurnUI.gameObject.SetActive(true));
        battleManager.onStartEnemyTurn += (() => battleTurnUI.gameObject.SetActive(true));
        battleManager.onStartMyTurn += battleTurnUI.DisplayBattleMyTurn;
        battleManager.onStartEnemyTurn += battleTurnUI.DisplayBattleEnemyTurn;

        battleManager.onStartMyTurn += turnEndUI.ActiveButton;
        battleManager.onStartEnemyTurn += turnEndUI.OnClickButtonEvent;
    }


    public override void Show()
    {
        base.Show();

        // 카드 전투상태
        //battleManager.Player.ResumeBattle();
    }
}
