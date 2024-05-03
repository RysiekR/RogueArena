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
    private static int chunkWidth = 20;
    private static int chunkHeight = 20;
    public MapTileEnum[,] mapChunk = new MapTileEnum[chunkWidth, chunkHeight];
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
        /*mapOfEnums = new MapTileEnum[3, 3]
        {
            {MapTileEnum.wall, MapTileEnum.wall,MapTileEnum.wall },
            {MapTileEnum.wall, MapTileEnum.air, MapTileEnum.air },
            {MapTileEnum.wall, MapTileEnum.wall,MapTileEnum.wall }
        };*/
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
    public void PrintMapChunk(Position position)
    {
        GenerateMapChunk(position);
        Console.Clear();
        for (int i = 0; i < chunkHeight; i++)
        {
            for (int j = 0; j < chunkWidth; j++)
            {
                Console.SetCursorPosition(j, i);
                Console.Write(' ');
                Console.SetCursorPosition(j, i);
                Console.Write(Sprites.GetCharFromEnum(mapChunk[j, i]));
            }
        }
    }

    private void GenerateMapChunk(Position position)
    {
        int leftColStart;
        if (position.col > 10)
        {
            if (position.col < mapaWidth - 10)
            {
                leftColStart = position.col - 10;
            }
            else
            {
                leftColStart = mapaWidth - 20;
            }
        }
        else
        {
            leftColStart = 0;
        }
        leftColStart++;

        int topRowStart;
        if (position.row > 10)
        {
            if (position.row < mapaWidth - 10)
            {
                topRowStart = position.row - 10;
            }
            else
            {
                topRowStart = mapaHeight - 20;
            }
        }
        else
        {
            topRowStart = 0;
        }
        topRowStart++;

        for (int i = 0; i < chunkWidth; i++)
        {
            for (int j = 0; j < chunkHeight; j++)
            {
                if (i != 0 && j != 0 && i != chunkWidth && j != chunkHeight)
                {
                    mapChunk[i, j] = mapOfEnums[leftColStart, topRowStart];
                }
                else
                {
                    mapChunk[i, j] = MapTileEnum.chunkBorder;
                }
            }
        }

        //wez pozycje gracza +- 10 w obu kierunkach i zapisz wartosci odpowiedajace z mapOfEnums do mapchunk
        //jak bedzie wygenerowane trzeba wywolac print map ale na mapchunk a nie na mapofenums
        //wszystko wssdzic w jedna metode i nie wywolywac tego w kilku miejscach
    }

    /*    public void PrintMapTest()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(' ');
                    Console.SetCursorPosition(j, i);
                    Console.Write(Sprites.GetCharFromEnum(mapOfEnums[i, j]));
                }
            }
        }
    */
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
            case 'V': return MapTileEnum.vertPortal;
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
            case MapTileEnum.chunkBorder: return '~';
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
    chunkBorder
}
