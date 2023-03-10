//Damage reduction
//Approximate formula of: (Incoming damage - (armor/10)) x (Resistances) x (General Reductions)
//For example being hit for 100 while, having 50% general Reductions, 25% resistances, and 100 armor
//would work out as: (100 - (100 / 10))(1 - 0.25)(1 - 0.50) = 33.75

// Damage Dealt is calculated in four passes, each one multiplying together. Multiple sources increasing a single variable add together.
//
//Pass 1 = (Base Damage + additional weapon damage) * (Might Modifier)
//Base Damage is the attack value of the equipped weapons. For spellpower abilities, this is based on level instead.
//    Additional weapon damage apply directly to the weapons stats, and do not effect Spell Power abilities.
//    Might Modifier is 1 at 8 might, at any might it is equal to: 1 + ((might-8)/100.
//    Pass 2 = (Generic Damage Modifiers + Mana Power Mod)
//Generic Damage Modifiers are effects that boost damage by some percent without the need for the attack to critically strike. Base value is 1, a 10% bonus would give a value of 1.1. See Buffs and Passives for more info on stacking up Generic Damage Modifiers.
//    Pass 3 = (Elemental Damage Modifiers)
//Boosts to the element the attack is will apply here, not in generic damage modifiers.
//    Pass 4 = (Critical Bonus Damage)
//Critical Bonus Damage only applies on critical hits, and the base value is 50% at 8 Dexterity, granting a bonus of 1.5.
//    Pass 5 = (Dodge Counter Bonus)
//Dodge counter attacks (and opportunity attacks) gain their own damage multiplier. At 8 reflex, this bonus is 0%, and every point beyond 8 increases it by 1% (additive).
//    Bonus Damage = (Damage To Type)
//A flat bonus to a type of damage (physical, fire, lightning, cold, healing, shadow) which is applied after all other passes.
//    Damage Dealt is equal to: (Pass 1) * (Pass 2) * (Pass 3) * (Pass 4) * (Pass 5) + (Bonus Damage)
//
//Explicitly: (Base dmg + Add weapon dmg) * Might Modifier * Dmg modifiers * Elemental Bonus * Crit bonus * Dodge Counter bonus + Dmg to Type)

public struct CharacterAttributes
{
    public int Level;           // character level
    public int Might;           // damage = might * 1%, armour = might * 1%
    public int Dexterity;       // crit rating = dexterity, crit damage = dexterity * 1.5%, movement spaces = base movement + min(dexterity / 25, 3). need formula for crit rating to crit hit chance
    public int Intellect;       // mana = base mana + intelligence * 5 + level * x, mana regen, resistances, skill range = base range + min(intelligence / 25, 3)
    public int Vitality;        // health = base health + vitality * 5 + level * x, health regen
    public int Reflex;          // counter chance = reflex * 1%, dodge counter chance = reflex * 1%, dodge rating
}

public struct Gear
{
    public string Name;
    public int MinDamage;
    public int MaxDamage;
    public float DamageLevelMultiplier;
    public float DamageQualityMultiplier;
}

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
    public float MovementAccountedFor;
    public int ActionPointsRemaining;
    public int RoundsWithoutMoving;
}

public struct CharacterCalculatedStats
{
    // Mostly from gear
    public int Armour;
    public int SpellArmour;   // for non-magic, could use mental fortitude?

    //might
    public int AttackPower;     // gear is also a major contributor
    
    //vitality
    public int HealthMax;
    public int HealthRegen;

    //intellect
    //instead of mana, could use focus, for non-magic games
    public int ManaMax;
    public int ManaRegen;
    
    //dexterity
    public float CritChance;
    public float CritPower;
    
    //reflex
    public float DodgeChance;
    public float DodgeCounterChance;
    public float CounterAttackDamage;
    public int DodgeCounterAttackLimit;
    
    //These are the only resistances currently in Stolen Realm. I thought I could just use the same ones here for now.
    public Resistances Resistances;
    public StatusEffectCode[] Immunities = Array.Empty<StatusEffectCode>();

    public int MaxRange;               // reduced by blinded (this overrides to reduce attack range which is based on the skill being used)
    public int MaxMove;                // reduced by immobilized, stunned
    public int RoundActionPoints;      // reduced by disabled, stunned
    public int MovementCost;           // increased by crippled, encumbered;
    public int StealthAmount;          // increased by stealth

    public CharacterCalculatedStats()
    {
    }
}