
public class SkillCardEffect : BaseCardEffect
{
    // ¹æ¾î¸·
    public void barrier()
    {
        player.PlayerStat.Shield += (5 /*+ agility*/);
    }
}
