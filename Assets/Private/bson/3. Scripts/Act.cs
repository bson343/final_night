using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Act : MonoBehaviour
{
    [SerializeField] private BattlePlayer _player;

    [SerializeField] private List<BattleData> Act1BattleDataList;

    private System.Random  random = new System.Random();

    private int randomNumber;

    private BattleManager battleManager => ServiceLocator.Instance.GetService<BattleManager>();
    private BattleCardGenerator CardGenerator => ServiceLocator.Instance.GetService<BattleCardGenerator>();

    private void Awake()
    {
        
    }

    private void Start()
    {
        battleManager.testMode = true;

        if (battleManager.testMode == true)
        {
            startTestMode();
        }
        {

        }
    }

    private void startTestMode()
    {
        randomNumber = random.Next(0, Act1BattleDataList.Count-1);
        _player.init();
        
        for (int i = 0; i < 6; i++)
        {
            _player.AddCard(CardGenerator.GenBatCard(5));
        }

        battleManager.defeatCommonEnemy = 100;

        battleManager.StartBattle(Act1BattleDataList[randomNumber]);
    }
}
