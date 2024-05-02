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
        player.currentMap.PrintMap();

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
            switch (GetInput())
            {
                case ConsoleKey.Escape: thisMenuContinue = false; break;
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
            switch (GetInput())
            {
                case ConsoleKey.Escape: thisMenuContinue = false; break;
            }
        }

    }
}