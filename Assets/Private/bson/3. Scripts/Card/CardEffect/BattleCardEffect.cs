using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerObj;

public class BattleCardEffect
{
    protected BattleManager battleManager => ServiceLocator.Instance.GetService<BattleManager>();
    protected BattlePlayer player => battleManager.Player;
    protected List<Enemy> enemies => battleManager.Enemies;
    protected Enemy targetEnemy => battleManager.TargetEnemy;

    BattleCardHolder cardHolder => battleManager.Player.CardHolder;


    [SerializeField]
    private IndentData[] indentData;

    // ��մ� ��
    public void Strike(BattleCard sender)
    {
        int damage = sender.EffectValues[0];
        targetEnemy?.Hit(damage + player.PlayerStat.Power, player);
    }

    // ���� ����
    public void DarkSpirit(BattleCard sender)
    {
        int damage = sender.EffectValues[0];
        targetEnemy?.Hit(damage + player.PlayerStat.Power, player);
    }

    // ������ �վƱ�
    public void GraspOftheUndying(BattleCard sender)
    {
        int damage = sender.EffectValues[0];
        targetEnemy?.Hit(damage + player.PlayerStat.Power, player);
    }

    // ���� ����ź 
    public void DarkMagicBullet(BattleCard sender)
    {
        int damage = sender.EffectValues[0];
        targetEnemy?.Hit(damage + (player.PlayerStat.Power*4), player);
    }

    // ��ȥ �ع�
    public void SoulLiberation(BattleCard sender)
    {
        int damage = sender.EffectValues[0];
        // ������ ����
        targetEnemy.Hit(damage + player.PlayerStat.Power, player);

        // Weak �ε�Ʈ ������ ��������
        IndentData weakIndentData = targetEnemy.CharacterIndent.GetIndentData(EIndent.Weak);

        foreach (var indent in targetEnemy.CharacterIndent.indentList)
        {
            if (indent.indentData.indent == EIndent.Weak)
            {
                // �ε�Ʈ�� �̹� �����ϴ� ��� �ؽ�Ʈ ������Ʈ
                targetEnemy.CharacterIndent.AddIndent(weakIndentData, 2);
                return;
            }
        }
        targetEnemy.CharacterIndent.AddIndent(weakIndentData, 2);
        

    }

    // ��Ƴ��� (�� ���ݷ� ��ȭ)
    public void AttackDown(BattleCard sender)
    {
        int damage = sender.EffectValues[0];

        // ������ ����
        targetEnemy.Hit(damage + player.PlayerStat.Power, player);

        // Weak �ε�Ʈ ������ ��������
        IndentData weakeningIndentData = targetEnemy.CharacterIndent.GetIndentData(EIndent.Weakening);

        foreach (var indent in targetEnemy.CharacterIndent.indentList)
        {
            if (indent.indentData.indent == EIndent.Weakening)
            {
                // �ε�Ʈ�� �̹� �����ϴ� ��� �ؽ�Ʈ ������Ʈ
                targetEnemy.CharacterIndent.AddIndent(weakeningIndentData, 2);
                return;
            }
        }
        targetEnemy.CharacterIndent.AddIndent(weakeningIndentData, 2);


    }

    // ���� ��
    public void barrier(BattleCard sender)
    {
        int shield = sender.EffectValues[0];
        player.PlayerStat.Shield += (shield /*+ agility*/);
    }

    public void GrowthAttackDamage(BattleCard sender)
    {
        targetEnemy?.Hit(sender.EffectValues[0], player);
        sender.EffectValues[0]++;
    }

    // ������
    public void AreaAttack(BattleCard sender)
    {
        int damage = sender.EffectValues[0];
        foreach (var enemy in enemies)
        {
            enemy.Hit(damage + player.PlayerStat.Power, player);
            enemy.Hit(damage + player.PlayerStat.Power, player);
            enemy.Hit(damage + player.PlayerStat.Power, player);
        }
    }


    // ������
    public void BigExplosion(BattleCard sender)
    {
        int damage = sender.EffectValues[0];
        IndentData BurndentData = targetEnemy.CharacterIndent.GetIndentData(EIndent.Burn);

        foreach (var enemy in enemies)
        {
            enemy.Hit(damage + player.PlayerStat.Power, player);

        }
    }

    // ���� ��ȯ
    public void ManaCirculation(BattleCard sender)
    {
        int value = sender.EffectValues[0];
        player.PlayerStat.CurrentOrb += value;
    }

    // ���ָ�����
    public void MagicalMeltdown(BattleCard sender)
    {
        int damege = sender.EffectValues[0];
        int energy = sender.EffectValues[1];
        int drawCount = sender.EffectValues[2];

        for (int i = 0; i < drawCount; i++)
        {
            cardHolder.DrawCard();
        }
        player.PlayerStat.CurrentOrb += energy;
        player.PlayerStat.CurrentHp -= damege;
    }

    // ȭ�� ����
    public void BurnPotion(BattleCard sender)
    {


        // ������ ����
        targetEnemy.Hit(5 + player.PlayerStat.Power, player);

        // Weak �ε�Ʈ ������ ��������
        IndentData BurndentData = targetEnemy.CharacterIndent.GetIndentData(EIndent.Burn);

        foreach (var indent in targetEnemy.CharacterIndent.indentList)
        {
            if (indent.indentData.indent == EIndent.Burn)
            {
                Debug.Log("�̹� ȭ���� ����");
                // �ε�Ʈ�� �̹� �����ϴ� ��� �ؽ�Ʈ ������Ʈ
                targetEnemy.CharacterIndent.AddIndent(BurndentData, 2);
                return;
            }
        }
        targetEnemy.CharacterIndent.AddIndent(BurndentData, 2);


    }

    // ��ȭ (�̿�)
    public void Ignite(BattleCard sender)
    {

        // ������ ����
        targetEnemy.Hit(5 + player.PlayerStat.Power, player);

        // Weak �ε�Ʈ ������ ��������
        IndentData BurndentData = targetEnemy.CharacterIndent.GetIndentData(EIndent.Burn);

        foreach (var indent in targetEnemy.CharacterIndent.indentList)
        {
            if (indent.indentData.indent == EIndent.Burn)
            {
                Debug.Log("�̹� ȭ���� ����");
                // �ε�Ʈ�� �̹� �����ϴ� ��� �ؽ�Ʈ ������Ʈ
                targetEnemy.CharacterIndent.AddIndent(BurndentData, 2);
                return;
            }
        }
        targetEnemy.CharacterIndent.AddIndent(BurndentData, 2);

    }

    // ������ �ձ� (�̿�)
    public void TouchOfCurse(BattleCard sender)
    {


        // ������ ����
        targetEnemy.Hit(5 + player.PlayerStat.Power, player);

        // Weak �ε�Ʈ ������ ��������
        IndentData BurndentData = targetEnemy.CharacterIndent.GetIndentData(EIndent.Burn);

        foreach (var indent in targetEnemy.CharacterIndent.indentList)
        {
            if (indent.indentData.indent == EIndent.Burn)
            {
                Debug.Log("�̹� ȭ���� ����");
                // �ε�Ʈ�� �̹� �����ϴ� ��� �ؽ�Ʈ ������Ʈ
                targetEnemy.CharacterIndent.AddIndent(BurndentData, 2);
                return;
            }
        }
        targetEnemy.CharacterIndent.AddIndent(BurndentData, 2);

    }

    //����
    public void EvilEye(BattleCard sender)
    {
        // �÷��̾��� �Ŀ� ����
        player.PlayerStat.Power += 2;

        // Strength �ε�Ʈ ������ ��������
        IndentData powerIndentData = player.CharacterIndent.GetIndentData(EIndent.Strength);


        foreach (var indent in player.CharacterIndent.indentList)
        {
            if (indent.indentData.indent == EIndent.Strength)
            {
                // �ε�Ʈ�� �̹� �����ϴ� ��� �ؽ�Ʈ ������Ʈ
                player.CharacterIndent.AddIndent(powerIndentData, 2);
                return;
            }
        }

        // �ε�Ʈ �߰�
        player.CharacterIndent.AddIndent(powerIndentData, 2);
       
    }

    // ���� 
    public void Incarnates(BattleCard sender)
    {
        // �÷��̾��� �Ŀ� ����
        player.PlayerStat.Power += 20;

        // Strength �ε�Ʈ ������ ��������
        IndentData powerIndentData = player.CharacterIndent.GetIndentData(EIndent.Strength);

        foreach (var indent in player.CharacterIndent.indentList)
        {
            if (indent.indentData.indent == EIndent.Strength)
            {
                // �ε�Ʈ�� �̹� �����ϴ� ��� �ؽ�Ʈ ������Ʈ
                player.CharacterIndent.AddIndent(powerIndentData, 20);
                return;
            }
        }
        player.CharacterIndent.AddIndent(powerIndentData, 20);
        IndentData deathIndentData = player.CharacterIndent.GetIndentData(EIndent.DeathCount);
        player.CharacterIndent.AddIndent(deathIndentData, 1);

    }


    /// ����������
    public void ImpMagician(BattleCard sender)
    {
        int energy = sender.EffectValues[0];
        player.PlayerStat.MaxOrb += energy;

    }

    /// �ذ��屺
    public void SkeletonGeneral(BattleCard sender)
    {
        int shield = sender.EffectValues[0];
        player.PlayerStat.Shield += (shield /*+ agility*/);
    }

    // ��� ���ް�
    public void GoblinQuartermaster(BattleCard sender)
    {
        int drawCount = sender.EffectValues[0];

        for (int i = 0; i < drawCount; i++)
        {
            cardHolder.DrawCard();
        }
    }
    
}

