Console.ReadKey();
//Console.SetBufferSize(Console.LargestWindowWidth,Console.LargestWindowHeight);
Console.CursorVisible = false;

//Map map = new Map(Sprites.miniMapEmptyVert);
Chunk firstChunk = new Chunk(ChunkHolder.firstChunkCoordinates);
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