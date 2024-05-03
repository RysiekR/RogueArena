
public static class Fight
{
    public static void Battle(Character firstFighter, Character secondFighter)
    {
        do
        {
            firstFighter.Attack(secondFighter);
            secondFighter.Attack(firstFighter);
        } while (firstFighter.stats.IsAlive && secondFighter.stats.IsAlive);

        /* Console.WriteLine("..........");
         Console.WriteLine("Fight Recap:");
         Console.WriteLine($"{firstFighter.name} stats:");
         firstFighter.DebugShowStats();
         Console.WriteLine($"{secondFighter.name} stats:");
         secondFighter.DebugShowStats();
         Console.WriteLine("..........");*/


        if (firstFighter.stats.IsAlive)
        {
            //Console.WriteLine($"{firstFighter.name} won!!");
            firstFighter.level.Exp = 1 + secondFighter.level.Lvl;
        }
        else if (secondFighter.stats.IsAlive)
        {
            //Console.WriteLine($"{secondFighter.name} won !!");
            secondFighter.level.Exp = 1 + firstFighter.level.Lvl;
        }
        else
        {
            //Console.WriteLine("Both died !!");
        }
        if (firstFighter.stats.IsAlive && firstFighter is Player player)
        {
            Random random = new Random();
            if (random.Next(1, 101) > 50)
            {
                if (random.Next(1, 101) > 50)
                {
                    player.allItems.Add(new WeaponItem());
                }
                else
                {
                    player.allItems.Add(new ArmorItem());
                }
            }
            player.stats.UpdateStats();
        }
        if (secondFighter.stats.IsAlive && secondFighter is Player player2)
        {
            Random random = new Random();
            if (random.Next(1, 101) > 50)
            {
                if (random.Next(1, 101) > 50)
                {
                    player2.allItems.Add(new WeaponItem());
                }
                else
                {
                    player2.allItems.Add(new ArmorItem());
                }
            }
            player2.stats.UpdateStats();
        }
    }
}
