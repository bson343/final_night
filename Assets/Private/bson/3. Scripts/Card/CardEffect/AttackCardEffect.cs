using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCardEffect : BaseCardEffect
{
    public void Strike()
    {
        targetEnemy.Hit(5 + power, player);
    }

    public void SoulLiberation()
    {
        targetEnemy.Hit(13 + power, player);
    }
}
