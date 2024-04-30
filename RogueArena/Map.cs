public class Map
{
    private int mapaHeight;
    private int mapaWidth;
    public Mapa[,] mapa;
    public List<Character> enemyList;
    public Map(string[] mapaStringowa)
    {
        GenerateArrayFromString(mapaStringowa);
        GenerateEnemies(0);
        /*mapa = new Mapa[3, 3]
        {
            {Mapa.wall, Mapa.wall,Mapa.wall },
            {Mapa.wall, Mapa.air, Mapa.air },
            {Mapa.wall, Mapa.wall,Mapa.wall }
        };*/
    }
    private void GenerateArrayFromString(string[] mapaStringowa)
    {
        mapaHeight = mapaStringowa.Length * Sprites.spriteSize;
        mapaWidth = mapaStringowa[0].Length * Sprites.spriteSize;
        mapa = new Mapa[mapaWidth, mapaHeight];

        for (int i = 0; i < mapaStringowa.Length; i++)
        {
            for (int j = 0; j < mapaStringowa[i].Length; j++)
            {
                char fromMiniMap = mapaStringowa[i][j];
                string[] toAddToMap = Sprites.GetBigSprite(fromMiniMap);
                for (int k = 0; k < Sprites.spriteSize; k++)
                {
                    for (int l = 0; l < Sprites.spriteSize; l++)
                        if (j * Sprites.spriteSize + l < mapaWidth && i * Sprites.spriteSize + k < mapaHeight)
                        {
                            mapa[ j * Sprites.spriteSize + l, i * Sprites.spriteSize + k] = Sprites.GetMapaEnum(toAddToMap[l][k]);
                        }
                }
            }
        }
    }
    private void GenerateEnemies(int howMany)
    {
        enemyList = new List<Character>();
        for (int i = 0; i < howMany; i++)
        {
            enemyList.Add(new Character(new(i + 1, i + 1)));
        }

    }
    public void RenderChanges()
    {

    }
    public void PrintMap()
    {
        for (int i = 0; i < mapaHeight; i++)
        {
            for (int j = 0; j < mapaWidth; j++)
            {
                Console.SetCursorPosition(j,i);
                Console.Write(' ');
                Console.SetCursorPosition(j, i);
                Console.Write(Sprites.GetCharFromEnum(mapa[j,i]));
            }
        }
    }
    public void PrintMapTest()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Console.SetCursorPosition(j, i);
                Console.Write(' ');
                Console.SetCursorPosition(j, i);
                Console.Write(Sprites.GetCharFromEnum(mapa[i,j]));
            }
        }
    }
}

public static class Sprites
{
    public const int spriteSize = 3;
    public static readonly string[] miniMapEmpty =
    {
        "WWWWWWWWWW",
        "W        W",
        "W        W",
        "W        W",
        "W        W",
        "W        W",
        "W        W",
        "W        W",
        "W        W",
        "WWWWWWWWWW"
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
    public static Mapa GetMapaEnum(char charToConvert)
    {
        switch (charToConvert)
        {
            case '#': return Mapa.wall;
            case '/': return Mapa.wallWindow;
            case ',': return Mapa.grass;
            case ' ': return Mapa.air;
            default: return Mapa.air;
        }
    }
    public static char GetCharFromEnum(Mapa enumToGetChar)
    {
        switch (enumToGetChar)
        {
            case Mapa.wall: return '#';
            case Mapa.wallWindow: return '/';
            case Mapa.air: return ' ';
            case Mapa.grass: return ',';
            default: return ' ';
        }
    }
    public static string[] GetBigSprite(char fromMiniMap)
    {
        switch (fromMiniMap)
        {
            case 'W': return wallSprite;
            case ' ': return airSprite;
            default: return airSprite;
        }
    }
}

public enum Mapa
{
    wall,
    wallWindow,
    air,
    grass,
    enemy,
    player
}
