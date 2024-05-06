public static class Menu
{

    public static void GetMenu(Player player)
    {
        bool continueMenu = true;
        while (continueMenu)
        {
            Console.Clear();
            Console.WriteLine("This is Menu(Esc to get back)");
            Console.WriteLine("Press 1 to get Items menu");
            Console.WriteLine("Press 2 to show Stats");
            //Console.WriteLine("");
            switch (GetInput())
            {
                case ConsoleKey.Escape: continueMenu = false; break;
                case ConsoleKey.D1: OpenItemsMenu(player); break;
                case ConsoleKey.D2: OpenStatsMenu(player); break;
            }
        }

        //player.currentMap.PrintMap();

    }
    private static ConsoleKey GetInput()
    {
        return Console.ReadKey(true).Key;
    }
    private static void OpenItemsMenu(Player player)
    {
        bool thisMenuContinue = true;
        while (thisMenuContinue)
        {
            Console.Clear();
            Console.WriteLine("This is Items Menu(Esc to get back)");
            Console.WriteLine("Press N for new item");
            foreach (ItemType item in player.allItems)
            {
                Console.WriteLine("item:");
                if (item is WeaponItem weaponItem)
                {
                    Console.WriteLine($"Attack: {weaponItem.atackValue}");
                }
                if (item is ArmorItem armorItem)
                {
                    Console.WriteLine($"Armor: {armorItem.armorValue}");
                }
            }
            switch (GetInput())
            {
                case ConsoleKey.Escape: thisMenuContinue = false; break;
                case ConsoleKey.N:
                    {
                        Console.Clear();
                        Console.WriteLine("1 for weapon, 2 for armor");
                        ConsoleKey pressed = Console.ReadKey(true).Key;
                        if (pressed == ConsoleKey.D1)
                        {
                            player.allItems.Add(new WeaponItem());
                        }
                        else if (pressed == ConsoleKey.D2) 
                        {
                            player.allItems.Add(new ArmorItem());
                        }
                        player.stats.UpdateStats();
                    }
                    break;
            }
        }
    }
    private static void OpenStatsMenu(Player player)
    {
        bool thisMenuContinue = true;
        while (thisMenuContinue)
        {
            Console.Clear();
            Console.WriteLine("This is Stats Menu(Esc to get back)");
            player.DebugShowStats();
            if (player.stats.Hp < player.stats.maxHp && player.grassPoints>0)
            {
                Console.WriteLine("Press 1 to heal");
            }
            switch (GetInput())
            {
                case ConsoleKey.Escape: thisMenuContinue = false; break;
                case ConsoleKey.D1: player.HealFromGrass(); break;
            }
        }

    }
}
public static class DrawItem
{

}