public struct Stats
{
    Character owner;
    private int strenght;
    private float hp;
    public float maxHp { get; private set; }//=str*2*lvl
    private bool isAlive = true;

    private int defense;
    public float shield { get; private set; }
    public float maxShield { get; private set; }//=(def*1.5 + str*1.5) *1.2 * lvl
    private bool isShielded = true;

    public float armorSum { get; private set; }
    private float armorFromDefense;

    public float attackSum { get; private set; }
    private float attackPowerFromStats;

    //Add vittality stat and regen method

    public Stats(int str, int def, Character ownerPlayer)
    {
        owner = ownerPlayer;
        strenght = str;
        defense = def;
        UpdateStats();
        hp = 0.5f * maxHp;
    }
    public bool IsAlive
    {
        get
        {
            if (hp <= 0) { return false; }
            else { return true; }
        }
    }

    public float Hp
    {
        get => hp;
        set
        {
            if (value < 0) // Jeśli wartość jest ujemna, zadaj obrażenia.
            {
                if (!isShielded)
                {

                    TakeHpDamage(value);//hp += value; // Zadaj obrażenia.
                    if (hp <= 0)
                    {
                        hp = 0;
                        isAlive = false;
                    }
                }
                else
                {
                    HitShield(value);
                }
            }
            else // W przeciwnym razie lecz postać.
            {
                hp += value;
                if (hp > maxHp)
                {
                    hp = maxHp;
                }
            }
        }
    }

    private void TakeHpDamage(float value)
    {
        float damage = -value;
        float damageAfterArmor;
        damageAfterArmor = damage / (1.0f + (armorSum / 10.0f));
        Console.WriteLine("damage b4 armor :");
        Console.WriteLine(damage);
        Console.WriteLine("damage after armor :");
        Console.WriteLine(damageAfterArmor);

        if (damageAfterArmor < 0)
        {
            damageAfterArmor = 0;
        }
        hp -= damageAfterArmor;
    }

    public void HitShield(float damage)
    {
        if (damage < 0) // damage jest na minusie => deal dmg
        {
            shield += damage;
            if (shield <= 0)
            {
                isShielded = false;
            }
        }
    }
    public void UpdateStats()
    {
        float tempMaxHp = maxHp;
        maxHp = strenght * 2.0f * owner.level.Lvl;
        hp = (hp * maxHp) / tempMaxHp;

        maxShield = (defense * 1.5f + strenght * 1.5f) * 1.2f * owner.level.Lvl;
        shield = maxShield;
        isShielded = true;

        armorFromDefense = (defense) * owner.level.Lvl * 0.5f;
        armorSum = armorFromDefense + owner.ArmorFromArmorItems() + 10;

        attackPowerFromStats = strenght * owner.level.Lvl * 0.3f;
        attackSum = attackPowerFromStats + owner.AttackPowerFromItems() + 1;

    }
    public void LevelUp()
    {
        Console.WriteLine("LevelUp");
        strenght += 5;
        defense += 5;
    }

}

public struct Position
{
    public Position(int rowIn, int colIn)
    {
        _row = rowIn; _col = colIn;
    }
    public Position(Position posToCopy)
    {
        _row = posToCopy.row; _col = posToCopy.col;
    }
    public int row { get => _row; set { if (value < 0 && _row + value < 0) { _row = 0; } else { _row += value; } } }
    public int col { get => _col; set { if (value < 0 && _col + value < 0) { _col = 0; } else { _col += value; } } }

    private int _row;
    private int _col;
}
