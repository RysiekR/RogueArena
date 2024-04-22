public interface ItemType
{
    public TypeOfItem typeOfItem { get; }
}

public enum TypeOfItem
{
    Armor,
    Weapon,
    Shield
}

public class ArmorItem : ItemType
{
    public int armor { get; private set; }
    public TypeOfItem typeOfItem { get; private set; } = TypeOfItem.Armor;
    public ArmorItem()
    {
        Random random = new Random();
        armor = random.Next(0, 26);
    }
}

public class WeaponItem : ItemType
{
    public int atackValue { get; private set; }
    public TypeOfItem typeOfItem { get; private set; } = TypeOfItem.Weapon;

    public WeaponItem()
    {
        Random random = new Random();
        atackValue = random.Next(0, 16);
    }
}