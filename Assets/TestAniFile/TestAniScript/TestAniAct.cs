using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TestAniAct : MonoBehaviour
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
        randomNumber = random.Next(0, Act1BattleDataList.Count - 1);
        _player.init();

        for (int i = 1; i < 6; i++)
        {
            if (i<5)
            {
              _player.AddCard(CardGenerator.GenBatCard(7));
            }else {
                _player.AddCard(CardGenerator.GenBatCard(5)); // 깎아내기 생성 (이자리에 카드 테스트해볼꺼 넣으면 될듯?)
            }
            
            
        }

        battleManager.defeatCommonEnemy = 100;

        battleManager.StartBattle(Act1BattleDataList[randomNumber]);
    }

}
