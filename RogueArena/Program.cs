
Character player = new Character(new Position(1, 1));
Character enemy = new Character(new Position(2, 2));

Console.WriteLine("Player stats:");
player.DebugShowStats();
Console.WriteLine("Enemy stats");
enemy.DebugShowStats();

do
{
    player.Attack(enemy);
    enemy.Attack(player);
    Console.WriteLine("..........");
    Console.WriteLine("Player stats:");
    player.DebugShowStats();
    Console.WriteLine("Enemy stats");
    enemy.DebugShowStats();
    Console.WriteLine("..........");

} while (player.stats.IsAlive && enemy.stats.IsAlive);
