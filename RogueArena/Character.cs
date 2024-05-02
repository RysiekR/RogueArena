
public abstract class Character
{
    public Map currentMap;
    public Position pos;
    protected Position previousPosition;
    public Stats stats;
    public Level level;
    public List<ItemType> allItems = new List<ItemType>();
    public string name;
    protected bool canMove = true;
    protected int grassPoints = 0;
    protected Dictionary<ConsoleKey, Action> movementDictionary;
    protected Dictionary<MapTileEnum, Action> logicOnPosition;
    protected ConsoleKey movementKey;
    public Character(Position position, Map currentMap)
    {
        GetRandomEQ(3);
        name = NameGenerator.GetName();

        this.pos = position;
        level = new Level(this);
        stats = new Stats(10, 10, this);
        movementDictionary = new()
        {
            {ConsoleKey.W, () => this.pos.row=-1 },
            {ConsoleKey.S, () => this.pos.row=1 },
            {ConsoleKey.A, () => this.pos.col=-1 },
            {ConsoleKey.D, () => this.pos.col=1 },
        };

        this.currentMap = currentMap;

    }
    public void DebugShowStats()
    {
        Console.WriteLine($"{stats.Hp} / {stats.maxHp} HP");
        Console.WriteLine($"{stats.shield} / {stats.maxShield} Shield");
        Console.WriteLine($"Armor: {stats.armorSum}, ArmorItems: {ArmorFromArmorItems()}");
        Console.WriteLine($"Attack: {stats.attackSum}, AttackItems: {AttackPowerFromItems()}");
        Console.WriteLine($"Strength: {stats.strenght}, Defense: {stats.defense}");
        Console.WriteLine($"Level: {level.Lvl}, Exp: {level.Exp} / {level.GetExpThreshold()}");

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
    /*public void MakeAMove()
    {
        Position previousPosition = new Position(pos);
        //get new position
        movementKey = Console.ReadKey(true).Key;
        //movementDictionary.ContainsKey(movementKey)
        if (movementDictionary.TryGetValue(movementKey, out Action movementAction))
        {
            movementAction?.Invoke();
        }
    }*/

    //public abstract void MakeAMove();

    public void MovementAfterInput()
    {
        //save last position
        previousPosition = new Position(pos);

        if (movementDictionary.ContainsKey(movementKey))
        {
            Action movementAction = movementDictionary[movementKey];
            movementAction();//change pos
        }
        //make logic on pos if canmove true then move else make other action and set pos to previous position
        if (logicOnPosition.ContainsKey(currentMap.mapOfEnums[pos.col, pos.row]))
        {
            Action movementLogic = logicOnPosition[currentMap.mapOfEnums[pos.col, pos.row]];
            movementLogic();
        }

        if (canMove)
        {
            PutCharacterOnMap();
            PutAirInTile(previousPosition);
        }
        else
        {
            pos = previousPosition;
        }



        canMove = true;
    }
    protected void PutCharacterOnMap()
    {
        if (this is Player)
        {
            currentMap.mapOfEnums[pos.col, pos.row] = MapTileEnum.player;
        }
        else if (this is Enemy)
        {
            currentMap.mapOfEnums[pos.col, pos.row] = MapTileEnum.enemy;
        }
        Console.SetCursorPosition(pos.col, pos.row);
        Console.Write(Sprites.GetCharFromEnum(currentMap.mapOfEnums[pos.col, pos.row]));
    }
    protected void PutAirInTile(Position position)
    {
        currentMap.mapOfEnums[position.col, position.row] = MapTileEnum.air;
        Console.SetCursorPosition(position.col, position.row);
        Console.Write(Sprites.GetCharFromEnum(currentMap.mapOfEnums[position.col, position.row]));
    }
    public void InitializeCharacter()
    {
        PutCharacterOnMap();
    }

    protected void FightyFight(Position position)
    {
        Fight.Battle(this, currentMap.GetCharacterInPosition(position));
    }
}

public class Enemy : Character
{
    private Random random = new Random();
    public Enemy(Position position, Map currentMap) : base(position, currentMap)
    {
        this.currentMap = currentMap;
        pos = position;
        logicOnPosition = new()
        {
            {MapTileEnum.wall,()=> canMove = false},
            {MapTileEnum.vertPortal,()=> canMove = false},
            {MapTileEnum.horrPortal,()=> canMove = false},
            {MapTileEnum.grass,()=> grassPoints++ },
            {MapTileEnum.enemy,()=> FightyFight(pos)}

        };
    }
    public void MakeAMove()
    {
        //get input and 
        movementKey = GetRandomInput();

        MovementAfterInput();
    }
    private ConsoleKey GetRandomInput()
    {
        switch (random.Next(1, 5))
        {
            case 1: return ConsoleKey.W;
            case 2: return ConsoleKey.S;
            case 3: return ConsoleKey.D;
            case 4: return ConsoleKey.A;
            default: return ConsoleKey.None;
        }
    }
}

public class Player : Character
{
    public Player(Position position, Map currentMap) : base(position, currentMap)
    {
        this.currentMap = currentMap;
        pos = position;
        name = "Player";
        logicOnPosition = new()
        {
            {MapTileEnum.wall,()=> canMove = false},
            {MapTileEnum.grass,()=> grassPoints++ },
            {MapTileEnum.enemy,()=> FightyFight(pos)},
            {MapTileEnum.horrPortal,()=> horrPortalLogic() },
            {MapTileEnum.vertPortal,()=> vertPortalLogic() }
        };
        movementDictionary.Add(ConsoleKey.M, () => Menu.GetMenu(this));

    }
    public void MakeAMove()
    {
        //get input and 
        movementKey = Console.ReadKey(true).Key;

        MovementAfterInput();
    }
    private void horrPortalLogic()
    {
        canMove = false;
        PutAirInTile(previousPosition);
        currentMap = MapHolder.mapHorr;
        currentMap.PrintMap();
        pos = previousPosition;
        PutCharacterOnMap();
    }
    private void vertPortalLogic()
    {
        canMove = false;
        PutAirInTile(previousPosition);
        currentMap = MapHolder.mapVert;
        currentMap.PrintMap();
        pos = previousPosition;
        PutCharacterOnMap();
    }
}
