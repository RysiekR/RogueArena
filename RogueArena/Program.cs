/*Console.ReadKey();
Console.CursorVisible = false;

Chunk firstChunk = new Chunk(ChunkHolder.firstChunkCoordinates,true);
Player player = new(new Position(5, 5), ChunkHolder.chunkData[ChunkHolder.firstChunkCoordinates]);
player.currentChunk.PrintMap();
player.InitializeCharacter();
foreach (Enemy enemy in player.currentChunk.enemiesList)
{
    enemy.InitializeCharacter();
}
while (true)
{
    player.MakeAMove();
    foreach (Enemy enemy in player.currentChunk.enemiesList)
    {
        enemy.MakeAMove();
    }
}*/

using static FactoryInventory;
//Pouch pouch1 = GetPouch(Resources.Wood,true);
Pouch pouch1 = GetPouch(Resources.Wood,0,0,0);
pouch1.DebugInv();
Console.WriteLine($"nie dodane: {pouch1.AddRes(3,ResQuality.Normal)}");
pouch1.DebugInv();
if (pouch1.RemoveRes(3))
{
    Console.WriteLine("jest wystarczajaco surowcow");
}
else
{
    Console.WriteLine("nie wystarczajaco");
}
pouch1.DebugInv();
Console.WriteLine($"nie dodane: {pouch1.AddRes(3, ResQuality.Poor)}");
pouch1.DebugInv();
if (pouch1.RemoveRes(3, ResQuality.Normal))
{
    Console.WriteLine("jest wystarczajaco surowcow");
}
else
{
    Console.WriteLine("nie wystarczajaco");
}
pouch1.DebugInv();

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