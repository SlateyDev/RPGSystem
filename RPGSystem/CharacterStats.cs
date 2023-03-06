//Damage reduction
//Approximate formula of: (Incoming damage - (armor/10)) x (Resistances) x (General Reductions)
//For example being hit for 100 while, having 50% general Reductions, 25% resistances, and 100 armor
//would work out as: (100 - (100 / 10))(1 - 0.25)(1 - 0.50) = 33.75

public struct Resistances
{
    public float Fire;
    public float Cold;
    public float Lightning;
    public float Physical;
}

public struct CharacterMutableStats
{
    public int Health;
    public int Mana;
    public float MovementMade;
    public int ActionPointsRemaining;
    public int RoundsWithoutMoving;
}

public struct CharacterCalculatedStats
{
    // Calculatable values
    public int Armour;
    public int SpellArmour;   // for non-magic, could use mental fortitude?

    public int HealthMax;
    public int HealthRegen;

    //instead of mana, could use focus, for non-magic games
    public int ManaMax;
    public int ManaRegen;
    
    public int AttackPower;
    public float CritChance;
    public int CritPower;
    
    //These are the only resistances currently in Stolen Realm. I thought I could just use the same ones here for now.
    public Resistances Resistances;
    public StatusEffectCode[] Immunities = Array.Empty<StatusEffectCode>();

    public int MaxRange;               // reduced by blinded
    public int MaxMove;                // reduced by immobilized, stunned
    public int RoundActionPoints;      // reduced by disabled, stunned
    public int MovementCost;           // increased by crippled, encumbered;

    public int StealthAmount;          // increased by stealth

    public CharacterCalculatedStats()
    {
        // Health = 0;
        // Mana = 0;
        // MoveRemaining = 0;
        // ActionPointsRemaining = 0;
        // RoundsWithoutMoving = 0;
        Armour = 0;
        SpellArmour = 0;
        HealthMax = 0;
        HealthRegen = 0;
        ManaMax = 0;
        ManaRegen = 0;
        AttackPower = 0;
        CritChance = 0;
        CritPower = 0;
        Resistances = default;
        MaxRange = 0;
        MaxMove = 0;
        RoundActionPoints = 0;
        MovementCost = 0;
        StealthAmount = 0;
    }
}