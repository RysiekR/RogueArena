public class ArmorItem
{
    public int armor { get; private set; }

    public ArmorItem()
    {
        Random random = new Random();
        armor = random.Next(0, 26);
    }
}

public class WeaponItem
{
    public int atackValue { get; private set; }
    public WeaponItem() 
    {
        Random random =new Random();
        atackValue = random.Next(0, 16);
    }
}