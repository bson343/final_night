using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Act : MonoBehaviour
{
    [SerializeField] private BattlePlayer _player;
    
    [SerializeField]
    private BattleData BattleMockData;

    private BattleManager battleManager => ServiceLocator.Instance.GetService<BattleManager>();
    private BattleCardGenerator CardGenerator => ServiceLocator.Instance.GetService<BattleCardGenerator>();

    private void Start()
    {
        _player.init();
        
        int generateNumber = 0;

        for (int i = 0; i < 6; i++)
        {
            _player.AddCard(CardGenerator.GenerateBattleCard(0));
        }
        
        battleManager.StartBattle(BattleMockData);
    }
}
