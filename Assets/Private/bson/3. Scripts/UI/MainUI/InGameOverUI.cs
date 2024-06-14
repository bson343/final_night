using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameOverUI : BaseUI
{
    [SerializeField] private Button leaveBtn;
    [SerializeField] private TMP_Text txtHeight;
    [SerializeField] private TMP_Text txtCommonMonster;
    [SerializeField] private TMP_Text txtEliteMonster;
    [SerializeField] private TMP_Text txtBossMonster;
    [SerializeField] private TMP_Text txtMoney;
    private BattleManager battleManager => ServiceLocator.Instance.GetService<BattleManager>();
    private void Awake()
    {
        leaveBtn.onClick.AddListener(() =>
        {
            UserManager.Instance.SetCurrentHP(0);
            NightSceneManager.Instance.LoadScene("Main");
        });

        txtCommonMonster.text += battleManager.defeatCommonEnemy;
        txtEliteMonster.text += battleManager.defeatElite;
        txtBossMonster.text += battleManager.defeatBoss;
    }
}
