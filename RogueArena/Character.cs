

public class Character
{
    public Position pos;
    public Stats stats;
    public Level level;
    public List<ItemType> allItems = new List<ItemType>();
    public Character(Position pos)
    {
        GetRandomEQ(5);

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
        foreach (ItemType item in allItems)
        {
            if (item.typeOfItem == TypeOfItem.Armor)
            {
                ArmorItem armorItem = (ArmorItem)item;
                sum += armorItem.armor;
            }
        }
        return sum;
    }
    public int AttackPowerFromItems()
    {
        int sum = 0;
        foreach (ItemType item in allItems)
        {
            if (item.typeOfItem == TypeOfItem.Weapon)
            {
                WeaponItem weaponItem = (WeaponItem)item;
                sum += weaponItem.atackValue;
            }
        }
        return sum;

    }

    public void Attack(Character other)
    {
        other.stats.Hp = -this.stats.attackSum;
    }

    private void GetRandomEQ(int sumOfItems)
    {
        Random rand = new Random();
        int numOfWeapons = rand.Next(0, sumOfItems);
        int numOfArmor = sumOfItems - numOfWeapons;
        for (int i = 0; i < numOfWeapons; i++)
        {
            allItems.Add(new WeaponItem());
        }
        for (int i = 0; i < numOfArmor; i++)
        {
            allItems.Add(new ArmorItem());
        }

    }
}

