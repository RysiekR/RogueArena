
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
    public static Dictionary<ChunkCoordinates, Chunk> chunkData = new();
}
public class Chunk
{
    private static Random random = new Random();
    public ChunkCoordinates ownCoordinates { get; private set; }
    public const int bigTileWidth = 40;//x axis size
    public const int bigTileHeight = 16;//y axis size
    public BigTileSprite[,] bigTileMap = new BigTileSprite[bigTileWidth, bigTileHeight];
    public SmallTile[,] smallTilesMap = new SmallTile[BigTileSprite.smallTileWidth * bigTileWidth, BigTileSprite.smallTileHeight * bigTileHeight];
    public List<Enemy> enemiesList = new List<Enemy>();
    public List<(Tuple<int , int >, Pouch)> droppedResources = new List<(Tuple<int , int>, Pouch)>();
    public int leftBorder { get; private set; } = 0;
    public int topBorder { get; private set; } = 0;
    public int rightBorder { get; private set; }
    public int bottomBorder { get; private set; }

    public Chunk(ChunkCoordinates chunkCoordinates)
    {
        rightBorder = bigTileWidth * BigTileSprite.smallTileWidth - 1;
        bottomBorder = bigTileHeight * BigTileSprite.smallTileHeight - 1;
        ownCoordinates = chunkCoordinates;
        FillWithEmpty();
        FindAndJoinPaths();
        CreateBigAndSmallMap();
        ChunkHolder.chunkData.Add(chunkCoordinates, this);
    }
    public Chunk(ChunkCoordinates chunkCoordinates, bool USEONLYIFITSFIRSTCHUNK)
    {
        if (USEONLYIFITSFIRSTCHUNK)
        {
            rightBorder = bigTileWidth * BigTileSprite.smallTileWidth - 1;
            bottomBorder = bigTileHeight * BigTileSprite.smallTileHeight - 1;
            ownCoordinates = chunkCoordinates;
            FillWithEmpty();
            CreateBigAndSmallMap();
            ChunkHolder.chunkData.Add(chunkCoordinates, this);
        }
    }
    private void CreateBigAndSmallMap()
    {
        for (int i = 0; i < bigTileHeight; i++)
        {
            for (int j = 0; j < bigTileWidth; j++)
            {
                //i to y, j to x

                //algorytm brasenhama do wsadzenia sciezek do zrobienia
                //if bigTileMap[j,i] == null || empty 
                // wtedy get random

                //random sprite dodaj do tablicy spritow
                if (bigTileMap[j, i].spriteName == BigTile.Empty)
                {
                    bigTileMap[j, i] = (BigTileSprite.GetNewRandomBigTileSprite());
                }


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
    private void FillWithEmpty()
    {
        for (int i = 0; i < bigTileHeight; i++)
        {
            for (int j = 0; j < bigTileWidth; j++)
            {
                //i to y, j to x
                bigTileMap[j, i] = new BigTileSprite(BigTile.Empty);
                /*
                                for (int k = 0; k < BigTileSprite.smallTileHeight; k++)
                                {
                                    for (int l = 0; l < BigTileSprite.smallTileWidth; l++)
                                    {
                                        //k to y, l to x
                                        smallTilesMap[j * BigTileSprite.smallTileWidth + l, i * BigTileSprite.smallTileHeight + k] = bigTileMap[j, i].smallTiles[k, l];
                                    }
                                }
                */
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
        DropedResources();
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
        Console.SetCursorPosition(bigTileWidth * BigTileSprite.smallTileWidth + 5, 5);
        Console.WriteLine($"Current Chunk Coordinates:");
        for (int i = 0; i < 10; i++)
        {
            Console.SetCursorPosition(bigTileWidth * BigTileSprite.smallTileWidth + 5 + i, 6);
            Console.Write(' ');
        }
        Console.SetCursorPosition(bigTileWidth * BigTileSprite.smallTileWidth + 5, 6);
        Console.WriteLine($"{ownCoordinates.x}/{ownCoordinates.y}");

    }
    private void FindAndJoinPaths()
    {
        int leftBorderPos = 0;
        int rightBorderPos = 0;
        int topBorderPos = 0;
        int bottomBorderPos = 0;
        int chance = 6; // wieksze niz chance (1 do 10)
        ChunkCoordinates leftChunkCoordinates = new ChunkCoordinates(ownCoordinates.x - 1, ownCoordinates.y);
        ChunkCoordinates rightChunkCoordinates = new ChunkCoordinates(ownCoordinates.x + 1, ownCoordinates.y);
        ChunkCoordinates topChunkCoordinates = new ChunkCoordinates(ownCoordinates.x, ownCoordinates.y - 1);
        ChunkCoordinates bottomChunkCoordinates = new ChunkCoordinates(ownCoordinates.x, ownCoordinates.y + 1);

        if (ChunkHolder.chunkData.ContainsKey(leftChunkCoordinates))
        {
            for (int i = 0; i < bigTileHeight - 1; i++)
            {
                if (ChunkHolder.chunkData[leftChunkCoordinates].bigTileMap[bigTileWidth - 1, i].spriteName is BigTile.Path)
                {
                    leftBorderPos = i;
                    break;
                }
            }
        }
        else
        {
            //leftBorderPos = random.Next(1, bigTileHeight - 2);
            if (random.Next(1, 11) > chance) leftBorderPos = random.Next(1, bigTileHeight - 1);
        }

        if (ChunkHolder.chunkData.ContainsKey(rightChunkCoordinates))
        {
            for (int i = 0; i < bigTileHeight - 1; i++)
            {
                if (ChunkHolder.chunkData[rightChunkCoordinates].bigTileMap[0, i].spriteName is BigTile.Path)
                {
                    rightBorderPos = i;
                    break;
                }
            }
        }
        else
        {
            //rightBorderPos = random.Next(1, bigTileHeight - 2);
            if (random.Next(1, 11) > chance) rightBorderPos = random.Next(1, bigTileHeight - 1);
        }

        if (ChunkHolder.chunkData.ContainsKey(topChunkCoordinates))
        {
            for (int i = 0; i < bigTileWidth - 1; i++)
            {
                if (ChunkHolder.chunkData[topChunkCoordinates].bigTileMap[i, bigTileHeight - 1].spriteName is BigTile.Path)
                {
                    topBorderPos = i;
                    break;
                }
            }
        }
        else
        {
            //topBorderPos = random.Next(1, bigTileWidth - 2);
            if (random.Next(1, 11) > chance) topBorderPos = random.Next(1, bigTileWidth - 1);
        }

        if (ChunkHolder.chunkData.ContainsKey(bottomChunkCoordinates))
        {
            for (int i = 0; i < bigTileWidth - 1; i++)
            {
                if (ChunkHolder.chunkData[bottomChunkCoordinates].bigTileMap[i, 0].spriteName is BigTile.Path)
                {
                    bottomBorderPos = i;
                    break;
                }
            }
        }
        else
        {
            //bottomBorderPos = random.Next(1, bigTileWidth - 2);
            if (random.Next(1, 11) > chance) bottomBorderPos = random.Next(1, bigTileWidth - 1);
        }
        //wstawic scieszki w te miejsca gdzie sie wwylosowalo/juz bylo w oprzednim chunku

        if (bottomBorderPos != 0)
        {
            bigTileMap[bottomBorderPos, bigTileHeight - 1] = new BigTileSprite(BigTile.Path);
        }
        if (topBorderPos != 0)
        {
            bigTileMap[topBorderPos, 0] = new BigTileSprite(BigTile.Path);
        }
        if (leftBorderPos != 0)
        {
            bigTileMap[0, leftBorderPos] = new BigTileSprite(BigTile.Path);
        }
        if (rightBorderPos != 0)
        {
            bigTileMap[bigTileWidth - 1, rightBorderPos] = new BigTileSprite(BigTile.Path);
        }

        //zrobic liste z sciezkami wylosowanymi i je polaczyc metoda fill
        List<(int x, int y)> coordsOfPathsOnBorder = GetListOfThisTile(BigTile.Path);
        /*
                if (coordsOfPathsOnBorder.Count > 2)
                {
                    int A = random.Next(0, coordsOfPathsOnBorder.Count);
                    (int x, int y) coordsA = coordsOfPathsOnBorder[A];
                    coordsOfPathsOnBorder.Remove(coordsA);
                    int B = random.Next(0, coordsOfPathsOnBorder.Count);
                    (int x, int y) coordsB = coordsOfPathsOnBorder[B];
                    FillAToBWith(coordsA, coordsB, BigTile.Path);
                }
        */
        if (coordsOfPathsOnBorder.Count == 1)
        {
        }
        else if (coordsOfPathsOnBorder.Count == 2)
        {
            (int x, int y) coordsA = coordsOfPathsOnBorder[0];
            (int x, int y) coordsB = coordsOfPathsOnBorder[1];
            FillAToBWith(coordsA, coordsB, BigTile.Path);
        }
        else if (coordsOfPathsOnBorder.Count == 3)
        {
            (int x, int y) coordsA = coordsOfPathsOnBorder[0];
            (int x, int y) coordsB = coordsOfPathsOnBorder[1];
            (int x, int y) coordsC = coordsOfPathsOnBorder[2];
            //= GetListOfThisTile(BigTile.Path)[random.Next(2, GetListOfThisTile(BigTile.Path).Count)];
            FillAToBWith(coordsA, coordsB, BigTile.Path);
            (int x, int y) coordsD = GetListOfThisTile(BigTile.Path)[random.Next(1, GetListOfThisTile(BigTile.Path).Count)];
            FillAToBWith(coordsC, coordsD, BigTile.Path);
        }
        else if (coordsOfPathsOnBorder.Count == 4)
        {
            (int x, int y) coordsA = coordsOfPathsOnBorder[0];
            (int x, int y) coordsB = coordsOfPathsOnBorder[1];
            (int x, int y) coordsC = coordsOfPathsOnBorder[2];
            (int x, int y) coordsE = coordsOfPathsOnBorder[3];
            //= GetListOfThisTile(BigTile.Path)[random.Next(2, GetListOfThisTile(BigTile.Path).Count)];
            FillAToBWith(coordsA, coordsB, BigTile.Path);
            (int x, int y) coordsD = GetListOfThisTile(BigTile.Path)[random.Next(1, GetListOfThisTile(BigTile.Path).Count)];
            FillAToBWith(coordsC, coordsD, BigTile.Path);
            (int x, int y) coordsF = GetListOfThisTile(BigTile.Path)[random.Next(1, GetListOfThisTile(BigTile.Path).Count)];
            FillAToBWith(coordsE, coordsF, BigTile.Path);
        }


        /*
                FillAToBWith((0, leftBorderPos), (bigTileWidth - 1, rightBorderPos), BigTile.Path);
                FillAToBWith((topBorderPos, 0), (bottomBorderPos, bigTileHeight - 1), BigTile.Path);
        */
    }
    private void DropedResources()
    {
        foreach (var r in droppedResources)
        {
            Console.SetCursorPosition(r.Item1.Item1, r.Item1.Item2);
            Console.Write(ResDic.ResourcesCharRepresantation[r.Item2.ResourceType]);

        }
    }

    private void FillAToBWith((int x, int y) coordsA, (int x, int y) coordsB, BigTile tile)
    {
        int x0 = coordsA.x, y0 = coordsA.y, x1 = coordsB.x, y1 = coordsB.y;
        int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = dx + dy, e2;

        while (true)
        {
            /*if (x0 >= 0 && x0 < bigTileWidth && y0 >= 0 && y0 < bigTileHeight)
            {
            }*/
            bigTileMap[x0, y0] = new BigTileSprite(tile);
            if (x0 == x1 && y0 == y1)
                break;

            e2 = 2 * err;

            if (e2 >= dy)
            {
                err += dy;
                x0 += sx;
            }

            if (e2 <= dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }
    private List<(int x, int y)> GetListOfThisTile(BigTile tile)
    {
        List<(int x, int y)> tempList = new List<(int x, int y)>();
        for (int i = 0; i < bigTileWidth; i++)
        {
            for (int j = 0; j < bigTileHeight; j++)
            {
                if (bigTileMap[i, j].spriteName == tile)
                {
                    tempList.Add((i, j));
                }
            }
        }

        return tempList;
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
    public const int smallTileWidth = 3;//sprite size
    public const int smallTileHeight = 3;
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
        /*else if (temp > 90)
        {
            return BigTile.Path;
        }*/
        else if (temp > 5)
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
        ", ,",
        " , ",
        ", ,",
    };
    public static readonly string[] emptySprite = new string[smallTileHeight]
    {
        "   ",
        "   ",
        "   ",
    };
    public static readonly string[] treeSprite = new string[smallTileHeight]
    {
        "***",
        "*T*",
        " T ",
    };
    public static readonly string[] pathSprite = new string[smallTileHeight]
    {
        "...",
        "...",
        "...",
    };

}
