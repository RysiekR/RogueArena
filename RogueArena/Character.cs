

public class Character
{
    public Position pos;
    public Stats stats;
    public Level level;
    public List<ItemType> allItems = new List<ItemType>();
    public string name;
    private Dictionary<ConsoleKey, Action> movementDictionary;
    private ConsoleKey movementKey;
    public Character(Position pos)
    {
        GetRandomEQ(3);
        name = NameGenerator.GetName();

        this.pos = pos;
        level = new Level(this);
        stats = new Stats(10, 10, this);
        movementDictionary = new()
    {
        {ConsoleKey.W, () => this.pos.row-- },
        {ConsoleKey.S, () => this.pos.row++ },
        {ConsoleKey.A, () => this.pos.col-- },
        {ConsoleKey.D, () => this.pos.col++ },
    };
    }
    public void DebugShowStats()
    {
        Console.WriteLine($"{stats.Hp} / {stats.maxHp} HP");
        Console.WriteLine($"{stats.shield} / {stats.maxShield} shield");
        Console.WriteLine($"{stats.armorSum} armor");
        Console.WriteLine($"{stats.attackSum} attack");

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
    public void MakeAMove()
    {
        Position previousPosition = new Position(pos);
        //get new position
        movementKey = Console.ReadKey(true).Key;
        //movementDictionary.ContainsKey(movementKey)
        if (movementDictionary.TryGetValue(movementKey, out Action movementAction))
        {
            movementAction?.Invoke();
            /*if (movementAction != null)
            {
            movementAction();
            }*/
        }

    }
}

