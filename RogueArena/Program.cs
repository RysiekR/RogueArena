Console.ReadKey();
Map map = new Map(Sprites.miniMapEmpty);
Player player = new(new(5,5),map);
map.PrintMap();
player.InitializeCharacter();
foreach (Enemy enemy in map.enemyList)
{
    enemy.InitializeCharacter();
}
while (true)
{
    Console.CursorVisible = false;
    player.MakeAMove();
    foreach (Enemy enemy in map.enemyList)
    {
        enemy.MakeAMove();
    }
}

/*
bool continueLoop = true;
do
{
Character player = new Character(new Position(1, 1));
Character enemy = new Character(new Position(2, 2));
player.name = "Player";
    Console.Clear();
    FightyFight.Battle(player, enemy);
    if (Console.ReadKey().Key != ConsoleKey.Spacebar)
    {
        break;
    }
}while(continueLoop);


*/