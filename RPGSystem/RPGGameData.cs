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
    };
}

public enum StatusEffectCode
{
    Blinded,
    Burning,
}

public delegate CharacterCalculatedStats ApplyToStatsHandler(CharacterCalculatedStats lastStats, int stacks);
public delegate CharacterMutableStats ApplyEffectHandler(CharacterCalculatedStats calculatedStats, CharacterMutableStats lastStats, int stacks);

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

public delegate void PerformSkillHandler(RPGCharacter source, RPGCharacter destination);

public struct Skill
{
    public SkillCode Code;
    public string Name;
    public int IconId;
    public int ActionPointCost;     // Cost to perform skill
    public int CooldownTime;        // Rounds before it can be done again
    public SkillCode Prerequisite;  // What are the prerequisites to buy this skill (None, Level or Skill)
    public int SkillPointCost;      // How much does the skill cost

    public int CooldownRemaining;   // How much longer you need to wait before you can perform the skill again
    public bool Unlocked;           // Have you unlocked the skill yet
    public PerformSkillHandler PerformSkill;
}