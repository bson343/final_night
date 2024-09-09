using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class SkillCardEffect : BaseCardEffect
{
    // ¹æ¾î¸·
    public void barrier()
    {
        player.PlayerStat.Shield += (5 /*+ agility*/);
    }
}
