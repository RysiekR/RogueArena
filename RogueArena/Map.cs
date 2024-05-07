/*namespace Temp
{
public static class MapHolder
{
    public static Map mapVert = new Map(Sprites.miniMapEmptyVert);
    public static Map mapHorr = new Map(Sprites.miniMapEmptyHorr);
}
public class Map
{
    private int mapaHeight;
    private int mapaWidth;
    public MapTileEnum[,] mapOfEnums;
    private MapTileEnum[,] mapChunk;
    public List<Enemy> enemyList;
    public Map(string[] miniMapFromSprites)
    {
        if (mapOfEnums == null)
        {
            GenerateArrayFromStringArray(miniMapFromSprites);
        }
        else
        {
            Console.WriteLine("mapofenums not null");
        }
        GenerateEnemies(1);
        
    }
    private void GenerateArrayFromStringArray(string[] stringArrayMiniMap)
    {
        mapaHeight = stringArrayMiniMap.Length * Sprites.spriteSize;
        mapaWidth = stringArrayMiniMap[0].Length * Sprites.spriteSize;

        mapOfEnums = new MapTileEnum[mapaWidth, mapaHeight];

        for (int i = 0; i < stringArrayMiniMap.Length; i++)
        {
            for (int j = 0; j < stringArrayMiniMap[i].Length; j++)
            {
                char fromMiniMap = stringArrayMiniMap[i][j];
                string[] toAddToMap = Sprites.GetBigSprite(fromMiniMap);
                for (int k = 0; k < Sprites.spriteSize; k++)
                {
                    for (int l = 0; l < Sprites.spriteSize; l++)
                        if (j * Sprites.spriteSize + l < mapaWidth && i * Sprites.spriteSize + k < mapaHeight)
                        {
                            mapOfEnums[j * Sprites.spriteSize + l, i * Sprites.spriteSize + k] = Sprites.GetMapaEnum(toAddToMap[l][k]);
                        }
                }
            }
        }
    }
    private void GenerateEnemies(int howMany)
    {
        enemyList = new List<Enemy>();
        for (int i = 0; i < howMany; i++)
        {
            enemyList.Add(new Enemy(new(i + Sprites.spriteSize, i + Sprites.spriteSize), this));
        }

    }
    public void RenderChanges()
    {

    }
    public void PrintMap()
    {
        Console.Clear();
        for (int i = 0; i < mapaHeight; i++)
        {
            for (int j = 0; j < mapaWidth; j++)
            {
                Console.SetCursorPosition(j, i);
                Console.Write(' ');
                Console.SetCursorPosition(j, i);
                Console.Write(Sprites.GetCharFromEnum(mapOfEnums[j, i]));
            }
        }
    }
    private void GenerateMapChunk()
    {
        //wez pozycje gracza +- 10 w obu kierunkach i zapisz wartosci odpowiedajace z mapOfEnums do mapchunk
        //jak bedzie wygenerowane trzeba wywolac print map ale na mapchunk a nie na mapofenums
        //wszystko wssdzic w jedna metode i nie wywolywac tego w kilku miejscach
    }

    public Character GetCharacterInPosition(Position position)
    {
        Character temp = null; // Inicjalizacja zmiennej temp wartością null
        foreach (Character character in this.enemyList)
        {
            if (character.pos.row == position.row && character.pos.col == position.col)
            {
                temp = character;
                break; // Dodanie break po znalezieniu pasującego Character
            }
        }
        return temp;
    }
}

public static class Sprites
{
    public const int spriteSize = 3;
    public static readonly string[] miniMapEmptyVert =
    {
        "WWWWWWWWWW",
        "W        W",
        "W H      W",
        "W        W",
        "W        W",
        "W        W",
        "W        W",
        "W        W",
        "W        W",
        "WWWWWWWWWW"
    };
    public static readonly string[] miniMapEmptyHorr =
    {
        "WWWWWWWWWWWWWWWWWWWWWWWWWWWW",
        "W                          W",
        "W V                        W",
        "W                          W",
        "WWWWWWWWWWWWWWWWWWWWWWWWWWWW",
    };
    public static readonly string[] wallSprite = new string[spriteSize]
    {
        "###",
        "#/#",
        "###",
    };
    public static readonly string[] airSprite = new string[spriteSize]
    {
        ", ,",
        " , ",
        ", ,"
    };
    public static readonly string[] horrPortalSprite = new string[spriteSize]
    {
        ", ,",
        " H ",
        ", ,"
    };
    public static readonly string[] vertPortalSprite = new string[spriteSize]
    {
        ", ,",
        " V ",
        ", ,"
    };
    public static MapTileEnum GetMapaEnum(char charToConvert)
    {
        switch (charToConvert)
        {
            case '#': return MapTileEnum.wall;
            case '/': return MapTileEnum.wallWindow;
            case ',': return MapTileEnum.grass;
            case ' ': return MapTileEnum.air;
            case 'H': return MapTileEnum.horrPortal;
            case 'V':return MapTileEnum.vertPortal;
            default: return MapTileEnum.air;
        }
    }
    public static char GetCharFromEnum(MapTileEnum enumToGetChar)
    {
        switch (enumToGetChar)
        {
            case MapTileEnum.wall: return '#';
            case MapTileEnum.wallWindow: return '/';
            case MapTileEnum.air: return ' ';
            case MapTileEnum.grass: return ',';
            case MapTileEnum.player: return '@';
            case MapTileEnum.enemy: return 'E';
            case MapTileEnum.horrPortal: return 'H';
            case MapTileEnum.vertPortal: return 'V';
            default: return ' ';
        }
    }
    public static string[] GetBigSprite(char fromMiniMap)
    {
        switch (fromMiniMap)
        {
            case 'W': return wallSprite;
            case ' ': return airSprite;
            case 'H': return horrPortalSprite;
            case 'V': return vertPortalSprite;
            default: return airSprite;
        }
    }
}

public enum MapTileEnum
{
    wall,
    wallWindow,
    air,
    grass,
    enemy,
    player,
    horrPortal,
    vertPortal,

}

}
*/

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
    public ChunkCoordinates ownCoordinates{get; private set;}
    private const int bigTileWidth = 24;//x axis size
    private const int bigTileHeight = 10;//y axis size
    public BigTileSprite[,] bigTileMap = new BigTileSprite[bigTileWidth, bigTileHeight];
    public SmallTile[,] smallTilesMap = new SmallTile[BigTileSprite.smallTileWidth * bigTileWidth, BigTileSprite.smallTileHeight * bigTileHeight];
    public List<Enemy> enemiesList = new List<Enemy>();
    public Chunk(ChunkCoordinates chunkCoordinates)
    {
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
                bigTileMap[j, i] = (new BigTileSprite(BigTile.Air)); // dodac randomness
                // a teraz z kazdej duzej komorki pododawac do malej
                for (int k = 0; k < BigTileSprite.smallTileHeight; k++)
                {
                    for(int l = 0; l<BigTileSprite.smallTileWidth; l++)
                    {
                        //k to y, l to x
                        smallTilesMap[j * BigTileSprite.smallTileWidth + l, i * BigTileSprite.smallTileHeight + k] = bigTileMap[j, i].smallTiles[l, k];
                    }
                }
            }
        }
    }
    public void PrintMap()
    {
        for(int i = 0;i < bigTileHeight*BigTileSprite.smallTileHeight;i++)
        //for(int i = 0;i < 20;i++)
        {
            for(int j = 0; j< bigTileWidth*BigTileSprite.smallTileWidth;j++)
            {
                //i to y j to x
                Console.SetCursorPosition(j,i);
                Console.Write(BigTileSprite.fromSmallTileToChar[smallTilesMap[j,i]]);
            }
        }
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
