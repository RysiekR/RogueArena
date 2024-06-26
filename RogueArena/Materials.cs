﻿//czy ograniczona ilosc mozna niesc
//czy sie psuje?
//
public enum Resources
{
    Grass = 0, // ','
    Sticks = 1, // '-'
    Wood = 2, // 't'
    Stone = 3, // 's'
    Leather = 4, // 'l'
    CopperOre = 5, // 'c'
    IronOre = 6, // 'i'
}
public enum ResQuality
{
    Poor = 0,
    Normal = 1,
    Good = 2,
}
public static class ResDic
{
    public static Dictionary<Resources, char> ResourcesCharRepresantation = new Dictionary<Resources, char>()
    {
        {Resources.Grass, ','},
        {Resources.Sticks, '-'},
        {Resources.Wood, 't'},
        {Resources.Stone, 's'},
        {Resources.Leather, 'l'},
        {Resources.CopperOre, 'c'},
        {Resources.IronOre, 'i'}
    };

}
public static class FactoryInventory
{
    private static Random random = new Random();
    public static Pouch GetPouch(Resources resType, int poor, int normal, int good)
    {
        Pouch pouch = new Pouch(resType);
        pouch.AddRes(poor, (ResQuality)0);
        pouch.AddRes(normal, (ResQuality)1);
        pouch.AddRes(good, (ResQuality)2);
        return pouch;
    }
    public static Pouch GetPouch(Resources resType, bool wantRandom)
    {
        if (wantRandom)
        {
            return GetPouch(resType, random.Next(0, InventoryItem.pouchCap), random.Next(0, InventoryItem.pouchCap), random.Next(0, InventoryItem.pouchCap));
        }
        else
        {
            return GetPouch(resType, 0, 0, 0);
        }
    }
    public static Pouch GetBag(Resources resType, int poor, int normal, int good)
    {
        Pouch pouch = new Pouch(resType);
        pouch.AddRes(poor, (ResQuality)0);
        pouch.AddRes(normal, (ResQuality)1);
        pouch.AddRes(good, (ResQuality)2);
        return pouch;
    }
    public static Pouch GetBag(Resources resType, bool wantRandom)
    {
        if (wantRandom)
        {
            return GetBag(resType, random.Next(0, InventoryItem.bagCap), random.Next(0, InventoryItem.bagCap), random.Next(0, InventoryItem.bagCap));
        }
        else
        {
            return GetBag(resType, 0, 0, 0);
        }
    }
    public static Pouch GetMagicBag(Resources resType, int poor, int normal, int good)
    {
        Pouch pouch = new Pouch(resType);
        pouch.AddRes(poor, (ResQuality)0);
        pouch.AddRes(normal, (ResQuality)1);
        pouch.AddRes(good, (ResQuality)2);
        return pouch;
    }
    public static Pouch GetMagicBag(Resources resType, bool wantRandom)
    {
        if (wantRandom)
        {
            return GetMagicBag(resType, random.Next(0, InventoryItem.magicBagCap), random.Next(0, InventoryItem.magicBagCap), random.Next(0, InventoryItem.magicBagCap));
        }
        else
        {
            return GetMagicBag(resType, 0, 0, 0);
        }
    }
}
public abstract class InventoryItem
{
    public const int pouchCap = 5, bagCap = 25, magicBagCap = 125;

    public Resources ResourceType { get; private set; }
    public int[] resArray { get; private set; }
    public int Capacity { get; private set; }

    public InventoryItem(Resources resourceType, int capacity)
    {
        ResourceType = resourceType;
        Capacity = capacity;
        resArray = new int[] { 0, 0, 0 };
    }
    public void DebugInv()
    {
        Console.WriteLine($"this inv is a {ResourceType} holder");
        Console.WriteLine("and has :");
        for (int i = 0; i < resArray.Length; i++)
        {
            Console.WriteLine($"{resArray[i]} amount of quality {Enum.GetName(typeof(ResQuality), i)}");
        }
    }

    // Methods to manipulate the inventory item, like adding or using resources
    public int AddRes(int ammount, ResQuality quality) // zrobione jako int zeby mocdodawac resource do player inventorys a jak bedzie  pelne to zostawiac je na ziemi
    {
        if (ammount < 0)
        {
            return 0;
        }
        int leftAmmount = ammount - (Capacity - resArray[(int)quality]);
        if (leftAmmount < 0)
        {
            leftAmmount = 0;
        }
        if (resArray[(int)quality] + ammount <= Capacity)
        {
            resArray[(int)quality] += ammount;
        }
        else
        {
            resArray[(int)quality] = Capacity;
        }
        //leftAmmount = int.Abs(leftAmmount);
        return leftAmmount;
    }
    public bool RemoveRes(int ammount)
    {
        return RemoveRes(ammount, ResQuality.Poor);
    }
    public bool RemoveRes(int ammount, ResQuality quality) // zrobione jako bool zeby mozna bylo wykorzystac w craftowaniu
    {
        if (ammount < 0)
        {
            return false;
        }
        else
        {
            if (ammount <= resArray[(int)quality])
            {
                resArray[(int)quality] -= ammount;
                return true;
            }
            else if ((int)quality < 2)
            {
                if (ammount <= (resArray[(int)quality + 1] * 2 + resArray[(int)quality]))
                {
                    ammount -= resArray[(int)quality];
                    ammount = (ammount % 2 == 1) ? (ammount / 2) + 1 : ammount / 2;

                    resArray[(int)quality + 1] -= ammount;
                    resArray[(int)quality] = 0;
                    return true;
                }
                else if ((int)quality < 1)
                {
                    if (ammount <= resArray[(int)quality + 2] * 3 + resArray[(int)quality + 1] * 2 + resArray[(int)quality])
                    {
                        ammount -= resArray[(int)quality];
                        ammount = (ammount % 2 == 1) ? (ammount / 2) + 1 : ammount / 2;
                        ammount -= resArray[(int)quality + 1];
                        ammount = ((ammount % 3) > 0) ? (ammount / 3) + 1 : ammount / 3;

                        resArray[(int)quality + 2] -= ammount;
                        resArray[(int)quality + 1] = 0;
                        resArray[(int)quality] = 0;
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }
    }
    public void ChangeResourceType(Resources resourceType)
    {
        ResourceType = resourceType;
        resArray = new int[] { 0, 0, 0 };
    }
}

public class Pouch : InventoryItem
{
    public string name { get; private set; } = "Pouch";
    public Pouch(Resources resourceType) : base(resourceType, pouchCap) // Pouch can hold 5 items
    {
    }
}

public class Bag : InventoryItem
{
    public string name { get; private set; } = "Bag";

    public Bag(Resources resourceType) : base(resourceType, bagCap) // Bag can hold 25 items
    {
    }
}

public class MagicBag : InventoryItem
{
    public string name { get; private set; } = "Magic Bag";

    public MagicBag(Resources resourceType) : base(resourceType, magicBagCap) // MagicBag can hold 125 items
    {
    }
}

