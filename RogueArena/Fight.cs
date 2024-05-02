
public static class Fight
{
    public static void Battle(Character firstFighter, Character secondFighter)
    {
        do
        {
            firstFighter.Attack(secondFighter);
            secondFighter.Attack(firstFighter);
        }while (firstFighter.stats.IsAlive && secondFighter.stats.IsAlive);

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
            firstFighter.level.Exp = 1+ secondFighter.level.Lvl;
        }
        else if (secondFighter.stats.IsAlive )
        {
            //Console.WriteLine($"{secondFighter.name} won !!");
            secondFighter.level.Exp = 1 + firstFighter.level.Lvl;
        }
        else
        {
            //Console.WriteLine("Both died !!");
        }
    }
}