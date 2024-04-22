

public class Character
{
    public Position pos;
    public Stats stats;
    public Level level;
    public List<ArmorItem> armorItems = new List<ArmorItem>();
    public List<WeaponItem> weaponItems = new List<WeaponItem>();

    public Character(Position pos)
    {
        armorItems.Add(new ArmorItem());
        weaponItems.Add(new WeaponItem());
        armorItems.Add(new ArmorItem());
        weaponItems.Add(new WeaponItem());
        this.pos = pos;
        level = new Level(this);
        stats = new Stats(10, 10, this);
    }
    public void DebugShowStats()
    {
        Console.Write(stats.Hp);
        Console.Write(" / ");
        Console.WriteLine(stats.maxHp);
        Console.Write(stats.shield);
        Console.WriteLine(" shield");
        Console.Write(stats.armorSum);
        Console.WriteLine(" armor");
        Console.Write(stats.attackSum);
        Console.WriteLine(" attack");

    }

    public int ArmorFromArmorItems()
    {
        int sum = 0;
        foreach (ArmorItem armorItem in armorItems)
        {
            sum += armorItem.armor;
        }
        return sum;
    }
    public int AttackPowerFromItems()
    {
        int sum = 0;
        foreach (WeaponItem weaponItem in weaponItems)
        {
            sum += weaponItem.atackValue;
        }
        return sum;
    }

    public void Attack(Character other)
    {
        other.stats.Hp = -this.stats.attackSum;
    }
}

