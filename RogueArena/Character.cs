
public abstract class Character
{
    public Chunk currentChunk;
    public ChunkCoordinates currentChunkCoordinates;

    public Position pos;
    protected Position previousPosition;
    public Stats stats;
    public Level level;
    public List<ItemType> allItems = new List<ItemType>();
    public string name;
    public char avatar;
    protected bool canMove = true;
    public int grassPoints { get; protected set; } = 0;
    protected Dictionary<ConsoleKey, Action> movementDictionary;
    protected Dictionary<SmallTile, Action> logicOnPosition;
    protected ConsoleKey movementKey;
    public Character(Position position, Chunk currentChunk)
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

        this.currentChunk = currentChunk;
        currentChunkCoordinates = currentChunk.ownCoordinates;
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
                sum += armorItem.armorValue;
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
    public void HealFromGrass()
    {
        if (grassPoints == 0)
        {
            Console.WriteLine("No grass to heal");
        }
        else
        {
            this.stats.shield = grassPoints;
            this.stats.Hp = grassPoints;
            grassPoints = 0;
        }
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
    public abstract void MakeAMove();

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
        if (logicOnPosition.ContainsKey(currentChunk.smallTilesMap[pos.col, pos.row]))
        {
            Action movementLogic = logicOnPosition[currentChunk.smallTilesMap[pos.col, pos.row]];
            movementLogic();
        }

        //tu bedzie problem !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (canMove)
        {
            PutCharacterOnMap();
            PutPreviousTileOnScreen(previousPosition);
        }
        else
        {
            pos = previousPosition;
        }

        canMove = true;
    }
    protected void PutCharacterOnMap()
    {
        /*if (this is Player)
        {
            currentMap.mapOfEnums[pos.col, pos.row] = MapTileEnum.player;
        }
        else if (this is Enemy)
        {
            currentMap.mapOfEnums[pos.col, pos.row] = MapTileEnum.enemy;
        }*/
        Console.SetCursorPosition(pos.col, pos.row);
        Console.Write(this.avatar);
/*
        if (this is Player)
        {
            Console.Write('@');
        }
        else if ( this is Enemy)
        {
            Console.Write(this.avatar);

        }*/
        //Console.Write(Sprites.GetCharFromEnum(currentMap.mapOfEnums[pos.col, pos.row]));
    }
    protected void PutPreviousTileOnScreen(Position position)
    {
        /*        currentMap.mapOfEnums[position.col, position.row] = MapTileEnum.air;
                Console.SetCursorPosition(position.col, position.row);
                Console.Write(Sprites.GetCharFromEnum(currentMap.mapOfEnums[position.col, position.row]));
        */
        // wywalic cala ta metode i zbudowac w przy slowniku metode ktora wypisuje i moze tez zmienia kolor
        Console.SetCursorPosition(position.col, position.row);
        Console.Write(BigTileSprite.fromSmallTileToChar[currentChunk.smallTilesMap[position.col, position.row]]);

    }
    public void InitializeCharacter()
    {
        PutCharacterOnMap();
    }

    protected void FightyFight(Position position)
    {
        Fight.Battle(this, currentChunk.GetCharacterInPosition(position));
    }
}

public class Enemy : Character
{
    public char avatar = '$';
    private Random random = new Random();
    public Enemy(Position position, Chunk currentChunk) : base(position, currentChunk)
    {
        this.currentChunk = currentChunk;
        pos = position;
        logicOnPosition = new()
        {
            //{SmallTile.wall,()=> canMove = false},
            //{MapTileEnum.vertPortal,()=> canMove = false},
            //{MapTileEnum.horrPortal,()=> canMove = false},
            {SmallTile.grass,()=> grassPoints++ },
            {SmallTile.enemy,()=> FightyFight(pos)}

        };
    }
    public override void MakeAMove()
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
    public char avatar = '@';
    public Player(Position position, Chunk currentChunk) : base(position, currentChunk)
    {
        this.currentChunk = currentChunk;
        pos = position;
        name = "Player";
        logicOnPosition = new()
        {
            //{SmallTile.wall,()=> canMove = false},
            {SmallTile.grass,()=> grassPoints++ },
            {SmallTile.enemy,()=> FightyFight(pos)},
            //{MapTileEnum.horrPortal,()=> horrPortalLogic() },
            //{MapTileEnum.vertPortal,()=> vertPortalLogic() }
        };
        movementDictionary.Add(ConsoleKey.M, () => Menu.GetMenu(this));

    }
    public override void MakeAMove()
    {
        //get input and 
        movementKey = Console.ReadKey(true).Key;

        MovementAfterInput();
    }
/*
    private void horrPortalLogic()
    {
        canMove = false;
        PutPreviousTileOnScreen(previousPosition);
        currentMap = MapHolder.mapHorr;
        currentMap.PrintMap();
        pos = previousPosition;
        PutCharacterOnMap();
    }
    private void vertPortalLogic()
    {
        canMove = false;
        PutPreviousTileOnScreen(previousPosition);
        currentMap = MapHolder.mapVert;
        currentMap.PrintMap();
        pos = previousPosition;
        PutCharacterOnMap();
    }
*/
}
