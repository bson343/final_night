using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCardEffect : BaseCardEffect
{
    public void Strike()
    {
        targetEnemy.Hit(6 + power, player);
    }
}
