
using System.Collections.Generic;

public abstract class Character
{
    public Chunk currentChunk;
    public ChunkCoordinates currentChunkCoordinates;
    public ChunkCoordinates nextChunkCoordinates;

    public Position pos;
    protected Position previousPosition;
    public Stats stats;
    public Level level;
    public List<ItemType> allItems = new List<ItemType>();
    public string name;
    public char avatar { get; protected set; }
    protected bool canMove = true;
    public int grassPoints { get; protected set; } = 0;
    protected Dictionary<ConsoleKey, Action> movementDictionary;
    protected Dictionary<SmallTile, Action> logicOnPosition;
    protected Dictionary<Resources, Action> pickUpDictionary;

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

        if (this is Player)
        {
            if (CheckIfChunkBorder(pos))
            {
                GenerateAndPlaceOnNewChunk();
            }
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
    private bool CheckIfChunkBorder(Position position)
    {
        if (position.col == currentChunk.leftBorder)
        {
            nextChunkCoordinates = new ChunkCoordinates(currentChunkCoordinates.x - 1, currentChunkCoordinates.y);
            return true;
        }
        else if (position.col == currentChunk.rightBorder)
        {
            nextChunkCoordinates = new ChunkCoordinates(currentChunkCoordinates.x + 1, currentChunkCoordinates.y);
            return true;
        }
        if (position.row == currentChunk.topBorder)
        {
            nextChunkCoordinates = new ChunkCoordinates(currentChunkCoordinates.x, currentChunkCoordinates.y - 1);
            return true;
        }
        else if (position.row == currentChunk.bottomBorder)
        {
            nextChunkCoordinates = new ChunkCoordinates(currentChunkCoordinates.x, currentChunkCoordinates.y + 1);
            return true;
        }

        return false;
    }
    private void GenerateAndPlaceOnNewChunk()
    {

        if (ChunkHolder.chunkData.ContainsKey(nextChunkCoordinates))
        {
            currentChunk = ChunkHolder.chunkData[nextChunkCoordinates];
        }
        else
        {
            Chunk nextChunk = new Chunk(nextChunkCoordinates);
            currentChunk = nextChunk;
        }
        currentChunkCoordinates = nextChunkCoordinates;
        if (pos.col == currentChunk.leftBorder)
        {
            pos = new Position(pos.row, currentChunk.rightBorder - 1);
        }
        else if (pos.col == currentChunk.rightBorder)
        {
            pos = new Position(pos.row, currentChunk.leftBorder + 1);

        }
        else if (pos.row == currentChunk.bottomBorder)
        {
            pos = new Position(currentChunk.topBorder + 1, pos.col);
        }
        else if (pos.row == currentChunk.topBorder)
        {
            pos = new Position(currentChunk.bottomBorder - 1, pos.col);

        }
        currentChunk.PrintMap();
        InitializeCharacter();

    }
    protected void PutCharacterOnMap()
    {
        Console.SetCursorPosition(pos.col, pos.row);
        Console.Write(this.avatar);
    }
    protected void PutPreviousTileOnScreen(Position position)
    {
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
    protected void GrassMethod()
    {
        grassPoints++;
        currentChunk.smallTilesMap[pos.col, pos.row] = SmallTile.empty;
    }
}

public class Enemy : Character
{

    private Random random = new Random();
    public Enemy(Position position, Chunk currentChunk) : base(position, currentChunk)
    {
        avatar = '$';
        this.currentChunk = currentChunk;
        pos = position;
        logicOnPosition = new()
        {
            //{SmallTile.wall,()=> canMove = false},
            {SmallTile.grass,()=> GrassMethod() },
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
    public int numberOfInventories = 5;
    public InventoryItem[] inventories;
    public Player(Position position, Chunk currentChunk) : base(position, currentChunk)
    {
        inventories = new InventoryItem[numberOfInventories];
        avatar = '@';
        this.currentChunk = currentChunk;
        pos = position;
        name = "Player";
        logicOnPosition = new()
        {
            //{SmallTile.wall,()=> canMove = false},
            {SmallTile.grass,()=> GrassMethod() },
            {SmallTile.enemy,()=> FightyFight(pos)},
            {SmallTile.trunk,()=> ChopTree(pos) }
        };
        movementDictionary.Add(ConsoleKey.M, () => Menu.GetMenu(this));
        pickUpDictionary = new()
          {
              {Resources.Wood,()=> PickUpWood(pos) }
          };

    }
    public override void MakeAMove()
    {
        //get input and 
        movementKey = Console.ReadKey(true).Key;

        MovementAfterInput();
    }
    /*
        private void ChopTree(Position position)
        {
            canMove = false;
            currentChunk.smallTilesMap[position.col, position.row] = SmallTile.empty;
            (int col, int row) tempCoordsOfDroppedItem = (position.col, position.row);
            currentChunk.droppedResources.Add(tempCoordsOfDroppedItem, FactoryInventory.GetPouch(Resources.Wood, 1, 0, 0));

        }
    */
    private void ChopTree(Position position)
    {
        canMove = false;
        currentChunk.smallTilesMap[position.col, position.row] = SmallTile.empty;

        // Create a named tuple for the coordinates
        var tempCoordsOfDroppedItem = new Tuple<int, int>(position.col, position.row);
        // Get a Pouch object with the desired resource
        Pouch droppedPouch = FactoryInventory.GetPouch(Resources.Wood, 1, 0, 0);

        // Add the tuple and Pouch object to the list
        currentChunk.droppedResources.Add((tempCoordsOfDroppedItem, droppedPouch));
    }
    private void PickUpWood(Position position)
    {
        bool wasAdded = false;
        var positionToCheck = new Tuple<int, int>(position.col, position.row);

        foreach (var droppedResource in currentChunk.droppedResources)
        {
            // Check if the coordinates of the dropped resource match the position
            if (droppedResource.Item1.Equals(positionToCheck))
            {
                // Get the Pouch object
                var pouch = droppedResource.Item2;

                // Add the Pouch object to the inventory
                for (int i = 0; i < inventories.Length; i++)
                {
                    if (inventories[i] == null)
                    {
                        inventories[i] = pouch;
                        wasAdded = true;
                        break;
                    }
                }
            }
            if (wasAdded) break;
        }
        if (wasAdded)
        {
            Console.Beep();
        }
    }
}
