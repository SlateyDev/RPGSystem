public static class RPGGameData
{
    public static readonly StatusEffect[] StatusEffects =
    {
        new()
        {
            Code = StatusEffectCode.Blinded,
            Name = "Blinded",
            IconId = 1,
            IsBad = true,
            StacksMax = 1,
            ApplyToStats =
                (stats, stacks) =>
                {
                    stats.MaxRange = 1;
                    return stats;
                },
            ApplyEffect = null,
        },
        new()
        {
            Code = StatusEffectCode.Burning,
            Name = "Burning",
            IconId = 2,
            IsBad = true,
            StacksMax = 5,
            ApplyToStats =
                (stats, stacks) =>
                {
                    stats.Resistances.Fire -= 1 * stacks;
                    return stats;
                },
            ApplyEffect =
                (calculatedStats, mutableStats, stacks) =>
                {
                    mutableStats.Health -= 5 * stacks;
                    return mutableStats;
                },
        },
        new()
        {
            Code = StatusEffectCode.Bleeding,
            Name = "Bleeding",
            IconId = 2,
            IsBad = true,
            StacksMax = 5,
            ApplyToStats =
                (stats, stacks) =>
                {
                    stats.Resistances.Physical -= 1 * stacks;
                    return stats;
                },
            ApplyEffect =
                (calculatedStats, mutableStats, stacks) =>
                {
                    mutableStats.Health -= 5 * stacks;
                    return mutableStats;
                },
            MovementEffect =
                (calculatedStats, mutableStats, stacks) =>
                {
                    mutableStats.Health -= (int)(5 * stacks * mutableStats.MovementMade) - (int)(5 * stacks * mutableStats.MovementAccountedFor);
                    return mutableStats;
                },
        },
    };

    public static readonly Skill[] Skills =
    {
        new()
        {
            Name = "Pistol Whip",
        },
        new()
        {
            Name = "Blind"
        },
        new()
        {
            Name = "Focus Shot"
        },
        new()
        {
            Name = "Distract"
        },
        new()
        {
            Name = "Gouge"
        },
        new()
        {
            Name = "Rupture"
        },
        new()
        {
            Name = "Disarm"
        },
        new()
        {
            Name = "Suppressive Fire"
        },
        new()
        {
            Name = "Sneak Attack"
        },
        new()
        {
            Name = "Aimed Shot"
        },
        new()
        {
            Name = "Spray and Pray"
        },
        new()
        {
            Name = "Ricochet Shot"
        },
        new()
        {
            Name = "Firing Range"
        },
    };
}

public enum StatusEffectCode
{
    Blinded,
    Burning,
    Bleeding,
    Poisoned,
    Chilled,
    Frozen,
    InnerWarmth,
    Immobilized,
    Disabled,
    Stunned,
    Crippled,
    Marked,
    Stealth,
    Salvation,  //Increased max health
    PatientHunter,
}

public delegate CharacterCalculatedStats ApplyToStatsHandler(CharacterCalculatedStats lastStats, int stacks);
public delegate CharacterMutableStats ApplyEffectHandler(CharacterCalculatedStats calculatedStats, CharacterMutableStats lastStats, int stacks);
public delegate CharacterMutableStats MovementEffectHandler(CharacterCalculatedStats calculatedStats, CharacterMutableStats lastStats, int stacks);

public struct StatusEffect
{
    public StatusEffectCode Code;
    public string Name;
    public int IconId;
    public bool IsBad;
    public int StacksMax;

    public int RoundsRemaining;
    public int StacksApplied;
    public ApplyToStatsHandler ApplyToStats;
    public ApplyEffectHandler? ApplyEffect;
    public MovementEffectHandler? MovementEffect;
}

public enum SkillCode
{
    None,
    Level1,
    Invigorate,
    Level2,
    Level3,
    Level4,
}

public delegate bool IsActiveSkillHandler(IRPGCharacter source);
public delegate bool CanPerformSkillHandler(IRPGCharacter source, IRPGCharacter destination);
public delegate void PerformSkillHandler(IRPGCharacter source, IRPGCharacter destination);

public struct Skill
{
    public SkillCode Code;
    public string Name;
    public string Description;
    public int IconId;
    public int ActionPointCost;     // Cost to perform skill
    public int CooldownTime;        // Rounds before it can be done again
    public SkillCode Prerequisite;  // What are the prerequisites to buy this skill (None, Level or Skill)
    public int SkillPointCost;      // How much does the skill cost

    public int CooldownRemaining;   // How much longer you need to wait before you can perform the skill again
    public bool Unlocked;           // Have you unlocked the skill yet
    
    // functions
    public IsActiveSkillHandler IsActive;   // May be inactive because you are not stealthed
    public CanPerformSkillHandler CanUse;   // Based on position of character, your position and which way character might be facing (if you have to be behind them for instance), also check LOS
    public PerformSkillHandler Use;         // Will most likely call CanUse first and stop use if Can't Use.
}