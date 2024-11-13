using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCardEffect
{
    protected BattleManager battleManager => ServiceLocator.Instance.GetService<BattleManager>();
    protected BattlePlayer player => battleManager.Player;
    protected List<Enemy> enemies => battleManager.Enemies;
    protected Enemy targetEnemy => battleManager.TargetEnemy;

    // 꿰뚫는 손
    public void Strike(BattleCard sender)
    {
        targetEnemy?.Hit(5+ player.PlayerStat.Power, player);
    }

    // 영혼 해방
    public void SoulLiberation(BattleCard sender)
    {
        targetEnemy?.Hit(13+ player.PlayerStat.Power, player);
    }

    // 마나 방어막
    public void barrier(BattleCard sender)
    {
        player.PlayerStat.Shield += (5 /*+ agility*/);
    }

    public void GrowthAttackDamage(BattleCard sender)
    {
        targetEnemy?.Hit(sender.EffectValues[0], player);
        sender.EffectValues[0]++;
    }

    // 유성우
    public void AreaAttack(BattleCard sender)
    {
        foreach (var enemy in enemies)
        {
            enemy.Hit(10 + player.PlayerStat.Power, player);
        }
    }

    // 마나 순환
    public void ManaCirculation(BattleCard sender)
    {
        player.PlayerStat.CurrentOrb += 2;
    }

    //마안
    public void EvilEye(BattleCard sender)
    {
        // 플레이어의 파워 증가
        player.PlayerStat.Power += 2;

        // Strength 인덴트 데이터 가져오기
        IndentData powerIndentData = player.CharacterIndent.GetIndentData(EIndent.Strength);

        if (powerIndentData == null)
        {
            Debug.LogError("powerIndentData가 null입니다. IndentData 배열에 해당 타입이 있는지 확인하세요.");
            return;
        }

        // 인덴트 추가
        player.CharacterIndent.AddIndent(powerIndentData, 2);
    }
 }

