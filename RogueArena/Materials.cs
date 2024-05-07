//drewno, ruda, skora, trawa
//material w 3 jakosciach
//gdzie skladowac?
//czy ograniczona ilosc mozna niesc
//czy sie psuje?
//
public enum Resources
{
    Grass = 0,
    Sticks = 1,
    Wood = 2,
    Stone = 3,
    Leather = 4,
    CopperOre = 5,
    IronOre = 6,
}
public enum ResQuality
{
    Shit = 0,
    Normal = 1,
    Good = 2,
}

public class InventoryItem
{
    public Resources ResourceType { get; private set; }
    public int[] resArray { get; private set; }
    public int Capacity { get; private set; } 

    public InventoryItem(Resources resourceType, int capacity)
    {
        ResourceType = resourceType;
        Capacity = capacity;
        resArray = new int[3]; // Initialize array with capacity size
    }

    // Methods to manipulate the inventory item, like adding or using resources
    public void AddRes(int ammount, ResQuality quality)
    {

    }
    public void RemoveRes(int ammount)
    {

    }
    public void RemoveRes(int ammount, ResQuality quality)
    {

    }
}

public class Pouch : InventoryItem
{
    public Pouch(Resources resourceType) : base(resourceType, 5) // Pouch can hold 5 items
    {
    }
}

public class Bag : InventoryItem
{
    public Bag(Resources resourceType) : base(resourceType, 25) // Bag can hold 25 items
    {
    }
}

public class MagicBag : InventoryItem
{
    public MagicBag(Resources resourceType) : base(resourceType, 125) // MagicBag can hold 125 items
    {
    }
}
