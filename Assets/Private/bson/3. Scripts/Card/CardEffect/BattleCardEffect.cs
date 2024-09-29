using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCardEffect
{
    protected BattleManager battleManager => ServiceLocator.Instance.GetService<BattleManager>();
    protected BattlePlayer player => battleManager.Player;
    protected List<Enemy> enemies => battleManager.Enemies;
    protected Enemy targetEnemy => battleManager.TargetEnemy;

    public void Strike()
    {
        targetEnemy.Hit(5, player);
    }

    public void SoulLiberation()
    {
        targetEnemy.Hit(13, player);
    }

    // ¹æ¾î¸·
    public void barrier()
    {
        player.PlayerStat.Shield += (5 /*+ agility*/);
    }
}
