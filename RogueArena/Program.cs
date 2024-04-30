Console.ReadKey();
    Map map = new Map(Sprites.miniMapEmpty);

while (true) 
{
    map.PrintMap();

    Console.ReadKey();
}


bool continueLoop = true;
do
{
Character player = new Character(new Position(1, 1));
Character enemy = new Character(new Position(2, 2));
player.name = "Player";
    Console.Clear();
    Fight.Battle(player, enemy);
    if (Console.ReadKey().Key != ConsoleKey.Spacebar)
    {
        break;
    }
}while(continueLoop);


