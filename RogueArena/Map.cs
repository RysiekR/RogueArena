
public enum SmallTile // single cell represantation
{
    player, // player avatar
    enemy, // enemy avatar
    empty, // ' '
    grass, // ','
    trunk, // 'T'
    leaf, // '*'
    path, // '.'
}

public enum BigTile //5x5 sprite name 
{
    Empty,
    Air,
    Tree,
    Path,

}
public static class ChunkHolder
{
    public static ChunkCoordinates firstChunkCoordinates = new ChunkCoordinates(0, 0);
    public static Dictionary<ChunkCoordinates, Chunk> chunkData = new()
    /*{
        { firstChunkCoordinates, new Chunk(firstChunkCoordinates) }
    }*/;
}
public class Chunk
{
    public ChunkCoordinates ownCoordinates { get; private set; }
    public const int bigTileWidth = 24;//x axis size
    public const int bigTileHeight = 10;//y axis size
    public BigTileSprite[,] bigTileMap = new BigTileSprite[bigTileWidth, bigTileHeight];
    public SmallTile[,] smallTilesMap = new SmallTile[BigTileSprite.smallTileWidth * bigTileWidth, BigTileSprite.smallTileHeight * bigTileHeight];
    public List<Enemy> enemiesList = new List<Enemy>();
    public int leftBorder { get; private set; } = 0;
    public int topBorder { get; private set; } = 0;
    public int rightBorder { get; private set; }
    public int bottomBorder { get; private set; }

    public Chunk(ChunkCoordinates chunkCoordinates)
    {
        rightBorder = bigTileWidth * BigTileSprite.smallTileWidth - 1;
        bottomBorder = bigTileHeight * BigTileSprite.smallTileHeight - 1;
        CreateBigAndSmallMap();
        ownCoordinates = chunkCoordinates;
        ChunkHolder.chunkData.Add(chunkCoordinates, this);
    }
    private void CreateBigAndSmallMap()
    {
        for (int i = 0; i < bigTileHeight; i++)
        {
            for (int j = 0; j < bigTileWidth; j++)
            {
                //i to y, j to x
                //random sprite dodaj do tablicy spritow
                bigTileMap[j, i] = (BigTileSprite.GetNewRandomBigTileSprite());
                // a teraz z kazdej duzej komorki pododawac do malej
                for (int k = 0; k < BigTileSprite.smallTileHeight; k++)
                {
                    for (int l = 0; l < BigTileSprite.smallTileWidth; l++)
                    {
                        //k to y, l to x
                        smallTilesMap[j * BigTileSprite.smallTileWidth + l, i * BigTileSprite.smallTileHeight + k] = bigTileMap[j, i].smallTiles[k, l];
                    }
                }
            }
        }
    }
    public void PrintMap()
    {
        for (int i = 0; i < bigTileHeight * BigTileSprite.smallTileHeight; i++)
        {
            for (int j = 0; j < bigTileWidth * BigTileSprite.smallTileWidth; j++)
            {
                //i to y j to x
                Console.SetCursorPosition(j, i);
                Console.Write(BigTileSprite.fromSmallTileToChar[smallTilesMap[j, i]]);
            }
        }
        PrintChunkCoordinates();
    }
    public Character GetCharacterInPosition(Position position)
    {
        Character temp = null; // Inicjalizacja zmiennej temp wartością null
        foreach (Character character in this.enemiesList)
        {
            if (character.pos.row == position.row && character.pos.col == position.col)
            {
                temp = character;
                break; // break po znalezieniu pasującego Character
            }
        }
        return temp;
    }
    public void PrintChunkCoordinates()
    {
        Console.SetCursorPosition(bigTileWidth * BigTileSprite.smallTileWidth + 5,5);
        Console.WriteLine($"Current Chunk Coordinates:");
        for (int i = 0; i < 10;i++)
        {
            Console.SetCursorPosition(bigTileWidth * BigTileSprite.smallTileWidth + 5 + i, 6);
            Console.Write(' ');
        }
        Console.SetCursorPosition(bigTileWidth * BigTileSprite.smallTileWidth + 5, 6);
        Console.WriteLine($"{ownCoordinates.x}/{ownCoordinates.y}");

    }
}
public struct ChunkCoordinates
{
    public int x { get; private set; }
    public int y { get; private set; }
    public ChunkCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public ChunkCoordinates(ChunkCoordinates other)
    {
        this.x = other.x;
        this.y = other.y;
    }
}

public class BigTileSprite
{
    public SmallTile[,] smallTiles = new SmallTile[smallTileWidth, smallTileHeight];
    public BigTile spriteName;
    public BigTileSprite(BigTile bigTile)
    {
        this.spriteName = bigTile;
        GenerateSpriteSmallTiles(bigTile);
    }
    private void GenerateSpriteSmallTiles(BigTile bigTile)
    {
        for (int i = 0; i < smallTileHeight; i++)
        {
            for (int j = 0; j < smallTileWidth; j++)
            {
                //j to x/col, i to y/row
                //uzyj slownika zeby zamienic chara na enuma i zapisz go w smallTiles
                char tempChar = GetSpriteString(bigTile)[i][j]; // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! [j][i] / [i][j] ???????????????????????????
                smallTiles[i, j] = fromCharToSmallTile[tempChar];
            }
        }
    }
    public SmallTile GetSmallTileAt(Position position)
    {
        return smallTiles[position.row, position.col];
    }

    //BigTileSprite statics
    public const int smallTileWidth = 5;//sprite size
    public const int smallTileHeight = 5;
    public static BigTileSprite GetNewRandomBigTileSprite()
    {
        return new BigTileSprite(GetRandomBigTile());
    }
    private static BigTile GetRandomBigTile()
    {
        Random random = new Random();
        int temp = random.Next(1, 101);
        if (temp > 95)
        {
            return BigTile.Tree;
        }
        else if (temp > 90)
        {
            return BigTile.Path;
        }
        else if (temp > 10)
        {
            return BigTile.Air;
        }
        else
        {
            return BigTile.Empty;
        }
    }

    private static string[] GetSpriteString(BigTile bigTile)
    {
        switch (bigTile)
        {
            case BigTile.Air: return airSprite;
            case BigTile.Tree: return treeSprite;
            case BigTile.Path: return pathSprite;
            default: return emptySprite;
        }
    }

    //zamiast wywolywac w srodku console write(tutaj) to zrobic metode i wywolywac ja tam gdzie trzeba np Character w PutPreviousTileOnScreen
    public static Dictionary<SmallTile, char> fromSmallTileToChar = new Dictionary<SmallTile, char>()
    {
        { SmallTile.grass, ',' },
        { SmallTile.empty, ' ' },
        { SmallTile.trunk, 'T' },
        { SmallTile.leaf, '*' },
        { SmallTile.path, '.' }
    };
    public static Dictionary<char, SmallTile> fromCharToSmallTile = new Dictionary<char, SmallTile>()
    {
        { ',', SmallTile.grass },
        { ' ', SmallTile.empty },
        { 'T', SmallTile.trunk },
        { '*', SmallTile.leaf },
        { '.', SmallTile.path }
    };

    public static readonly string[] airSprite = new string[smallTileHeight]
    {
        ", , ,",
        " , , ",
        ", , ,",
        " , , ",
        ", , ,"
    };
    public static readonly string[] emptySprite = new string[smallTileHeight]
    {
        "     ",
        "     ",
        "     ",
        "     ",
        "     "
    };
    public static readonly string[] treeSprite = new string[smallTileHeight]
    {
        "     ",
        " *** ",
        " *T* ",
        "  T  ",
        "     "
    };
    public static readonly string[] pathSprite = new string[smallTileHeight]
    {
        ".....",
        ".....",
        ".....",
        ".....",
        "....."
    };

}
