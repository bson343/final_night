using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCardEffect
{
    protected BattleManager battleManager => ServiceLocator.Instance.GetService<BattleManager>();
    protected BattlePlayer player => battleManager.Player;
    protected List<Enemy> enemies => battleManager.Enemies;
    protected Enemy targetEnemy => battleManager.TargetEnemy;

    [SerializeField]
    private IndentData[] indentData;

    // 꿰뚫는 손
    public void Strike(BattleCard sender)
    {
        targetEnemy?.Hit(5+ player.PlayerStat.Power, player);
    }

    // 영혼 해방
    public void SoulLiberation(BattleCard sender)
    {
        // 적이 존재하는지 확인
        if (targetEnemy == null)
        {
            Debug.LogWarning("Target enemy is null in SoulLiberation. Make sure to set TargetEnemy before using the card.");
            return;
        }

        // 공격을 수행
        targetEnemy.Hit(8 + player.PlayerStat.Power, player);

        // Weak 인덴트 데이터 가져오기
        IndentData weakIndentData = indentData[(int)EIndent.Weak];

        if (weakIndentData == null)
        {
            Debug.LogWarning("Weak indent data is null. Please ensure indentData is properly initialized.");
            return;
        }

        // 적의 인덴트 리스트에서 Weak 인덴트를 찾기
        foreach (var indent in targetEnemy.CharacterIndent.indentList)
        {
            if (indent.indentData.indent == EIndent.Weak)
            {
                // 인덴트가 이미 존재하는 경우 텍스트 업데이트
                indent.UpdateIndent();
                return;
            }
        }

        // 인덴트가 존재하지 않는 경우 새로운 인덴트 추가
        targetEnemy.CharacterIndent.AddIndent(weakIndentData, 2);
        targetEnemy.CharacterIndent.Visualize(); // 시각적 업데이트가 필요하다면 추가
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


        foreach (var indent in player.CharacterIndent.indentList)
        {
            if (indent.indentData.indent == EIndent.Strength)
            {
                // 인덴트가 이미 존재하는 경우 텍스트 업데이트
                indent.UpdateIndent();
                return;
            }
        }

        // 인덴트 추가
        player.CharacterIndent.AddIndent(powerIndentData, 2);
        player.CharacterIndent.Visualize();
    }
 }

