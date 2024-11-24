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

    // 암흑 정령
    public void DarkSpirit(BattleCard sender)
    {
        targetEnemy?.Hit(5 + player.PlayerStat.Power, player);
    }

    // 착취의 손아귀
    public void GraspOftheUndying(BattleCard sender)
    {
        targetEnemy?.Hit(5 + player.PlayerStat.Power, player);
    }

    // 암흑 마력탄 
    public void DarkMagicBullet(BattleCard sender)
    {
        targetEnemy?.Hit(5 + (player.PlayerStat.Power*4), player);
    }

    // 영혼 해방
    public void SoulLiberation(BattleCard sender)
    {
        // 공격을 수행
        targetEnemy.Hit(8 + player.PlayerStat.Power, player);

        // Weak 인덴트 데이터 가져오기
        IndentData weakIndentData = targetEnemy.CharacterIndent.GetIndentData(EIndent.Weak);

        foreach (var indent in targetEnemy.CharacterIndent.indentList)
        {
            if (indent.indentData.indent == EIndent.Weak)
            {
                // 인덴트가 이미 존재하는 경우 텍스트 업데이트
                targetEnemy.CharacterIndent.AddIndent(weakIndentData, 2);
                return;
            }
        }
        targetEnemy.CharacterIndent.AddIndent(weakIndentData, 2);
        

    }

    // 깎아내기 (적 공격력 약화)
    public void AttackDown(BattleCard sender)
    {

        // 공격을 수행
        targetEnemy.Hit(5 + player.PlayerStat.Power, player);

        // Weak 인덴트 데이터 가져오기
        IndentData weakeningIndentData = targetEnemy.CharacterIndent.GetIndentData(EIndent.Weakening);

        foreach (var indent in targetEnemy.CharacterIndent.indentList)
        {
            if (indent.indentData.indent == EIndent.Weakening)
            {
                // 인덴트가 이미 존재하는 경우 텍스트 업데이트
                targetEnemy.CharacterIndent.AddIndent(weakeningIndentData, 2);
                return;
            }
        }
        targetEnemy.CharacterIndent.AddIndent(weakeningIndentData, 2);


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
            enemy.Hit(6 + player.PlayerStat.Power, player);
            enemy.Hit(6 + player.PlayerStat.Power, player);
            enemy.Hit(6 + player.PlayerStat.Power, player);
        }
    }


    // 대폭발
    public void BigExplosion(BattleCard sender)
    {
        IndentData BurndentData = targetEnemy.CharacterIndent.GetIndentData(EIndent.Burn);

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

    // 폭주마법진
    public void MagicalMeltdown(BattleCard sender)
    {
        player.PlayerStat.CurrentOrb += 2;
    }

    // 화상 포션
    public void BurnPotion(BattleCard sender)
    {


        // 공격을 수행
        targetEnemy.Hit(5 + player.PlayerStat.Power, player);

        // Weak 인덴트 데이터 가져오기
        IndentData BurndentData = targetEnemy.CharacterIndent.GetIndentData(EIndent.Burn);

        foreach (var indent in targetEnemy.CharacterIndent.indentList)
        {
            if (indent.indentData.indent == EIndent.Burn)
            {
                Debug.Log("이미 화상이 존재");
                // 인덴트가 이미 존재하는 경우 텍스트 업데이트
                targetEnemy.CharacterIndent.AddIndent(BurndentData, 2);
                return;
            }
        }
        targetEnemy.CharacterIndent.AddIndent(BurndentData, 2);


    }

    // 점화 (미완)
    public void Ignite(BattleCard sender)
    {


        // 공격을 수행
        targetEnemy.Hit(5 + player.PlayerStat.Power, player);

        // Weak 인덴트 데이터 가져오기
        IndentData BurndentData = targetEnemy.CharacterIndent.GetIndentData(EIndent.Burn);

        foreach (var indent in targetEnemy.CharacterIndent.indentList)
        {
            if (indent.indentData.indent == EIndent.Burn)
            {
                Debug.Log("이미 화상이 존재");
                // 인덴트가 이미 존재하는 경우 텍스트 업데이트
                targetEnemy.CharacterIndent.AddIndent(BurndentData, 2);
                return;
            }
        }
        targetEnemy.CharacterIndent.AddIndent(BurndentData, 2);

    }

    // 저주의 손길 (미완)
    public void TouchOfCurse(BattleCard sender)
    {


        // 공격을 수행
        targetEnemy.Hit(5 + player.PlayerStat.Power, player);

        // Weak 인덴트 데이터 가져오기
        IndentData BurndentData = targetEnemy.CharacterIndent.GetIndentData(EIndent.Burn);

        foreach (var indent in targetEnemy.CharacterIndent.indentList)
        {
            if (indent.indentData.indent == EIndent.Burn)
            {
                Debug.Log("이미 화상이 존재");
                // 인덴트가 이미 존재하는 경우 텍스트 업데이트
                targetEnemy.CharacterIndent.AddIndent(BurndentData, 2);
                return;
            }
        }
        targetEnemy.CharacterIndent.AddIndent(BurndentData, 2);

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
                player.CharacterIndent.AddIndent(powerIndentData, 2);
                return;
            }
        }

        // 인덴트 추가
        player.CharacterIndent.AddIndent(powerIndentData, 2);
       
    }

    // 현신 
    public void Incarnates(BattleCard sender)
    {

        // 플레이어의 파워 증가
        player.PlayerStat.Power += 20;
        // Strength 인덴트 데이터 가져오기
        IndentData powerIndentData = player.CharacterIndent.GetIndentData(EIndent.Strength);

        foreach (var indent in player.CharacterIndent.indentList)
        {
            if (indent.indentData.indent == EIndent.Strength)
            {
                // 인덴트가 이미 존재하는 경우 텍스트 업데이트
                player.CharacterIndent.AddIndent(powerIndentData, 20);
                return;
            }
        }
        player.CharacterIndent.AddIndent(powerIndentData, 20);
        IndentData deathIndentData = player.CharacterIndent.GetIndentData(EIndent.DeathCount);
        player.CharacterIndent.AddIndent(deathIndentData, 1);

    }
}

