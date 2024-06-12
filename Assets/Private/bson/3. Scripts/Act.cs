using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Act : MonoBehaviour
{
    [SerializeField] private BattlePlayer _player;
    
    [SerializeField]
    private BattleData BattleMockData;

    [SerializeField] private List<BattleData> stages;

    private BattleManager battleManager => ServiceLocator.Instance.GetService<BattleManager>();
    private BattleCardGenerator CardGenerator => ServiceLocator.Instance.GetService<BattleCardGenerator>();

    private void Awake()
    {
        battleManager.testMode = true;

        if (battleManager.testMode == true)
        {
            startTestMode();
        }
        {

        }
    }

    private void Start()
    {
        
    }

    private void startTestMode()
    {
        _player.init();
        
        for (int i = 0; i < 6; i++)
        {
            _player.AddCard(CardGenerator.GeneratorRandomCard());
        }

        battleManager.defeatCommonEnemy = 100;
        
        battleManager.StartBattle(BattleMockData);
    }
}
