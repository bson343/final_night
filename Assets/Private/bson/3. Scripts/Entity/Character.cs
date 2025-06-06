using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Character : MonoBehaviour
{
    // Indent ����
    public bool[] indent = new bool[(int)EIndent.Size];
    protected BattleManager battleManager => ServiceLocator.Instance.GetService<BattleManager>();

    public abstract void Dead();

    public abstract void magic();
    public abstract void attack();
    public abstract void Hit(int damage, Character attacker);
    public abstract void Act();
}