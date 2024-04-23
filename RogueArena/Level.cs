
public class Level
{
    private int exp;
    private int lvl;
    private Character owner;

    public Level(Character ownerPlayer)
    {
        exp = 0;
        lvl = 1;
        owner = ownerPlayer;

    }
    public int Exp
    {
        get => exp;
        set
        {
            if (value >= 0)
                if ((exp + value) >= 5 * lvl)
                {
                    exp = (exp + value) - 5 * lvl;
                    lvl++;
                    owner.stats.LevelUp();
                    owner.stats.UpdateStats();
                }
                else
                {
                    exp += value;
                }
        }
    }
    public int Lvl { get => lvl; }

}

