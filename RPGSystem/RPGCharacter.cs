using System.ComponentModel;
using System.Runtime.InteropServices;

public class RPGCharacter
{
    public event HealthChangeHandler HealthChange;

    public CharacterAttributes Attributes;
    // public CharacterGear Gear[];

    public CharacterMutableStats MutableStats = new();
    // public CharacterCalculatedStats Stats = new();
    public List<StatusEffect> StatusEffects = new();
    
    public delegate void HealthChangeHandler(int changeBy, int newHealth);

    ///TODO:
    /// Need gear.
    /// Need function to calculate stats based on attributes and gear.

    public RPGCharacter()
    {
        Attributes.Level = 1;
        Attributes.Might = 8;
        Attributes.Vitality = 8;
        Attributes.Intellect = 8;
        Attributes.Dexterity = 8;
        Attributes.Reflex = 8;

        // Stats = AttributesToStats(Attributes);
        // Just some default values since CalculatedStats are not yet calculated.
    }

    static CharacterCalculatedStats AttributesToStats(CharacterAttributes attributes /*, CharacterGear[] gear*/)
    {
        var stats = new CharacterCalculatedStats();
        stats.Resistances.Fire = attributes.Intellect;
        stats.Resistances.Cold = attributes.Intellect;
        stats.Resistances.Lightning = attributes.Intellect;
        stats.Resistances.Physical = attributes.Dexterity;

        stats.CritChance = attributes.Dexterity * 0.5f;
        stats.CritPower = attributes.Dexterity * 1.5f;
        stats.MaxMove = 10 + Int32.Min(Int32.Max(attributes.Dexterity / 20, 0), 5);
        stats.MaxRange = 50; 

        stats.AttackPower = (int)(attributes.Might / 100.0f);
        stats.HealthMax = (int)((45 + 15 * attributes.Level + 5 * attributes.Vitality) * (1 + (attributes.Vitality - 8) / 100.0f));
        stats.ManaMax = (int)((45 + 15 * attributes.Level + 5 * attributes.Intellect) * (1 + (attributes.Intellect - 8) / 100.0f));
        return stats;
    } 
    
    public void ApplyStatusEffect(StatusEffectCode code, int numberOfStacks, int numberOfRounds)
    {
        Console.WriteLine("Applying Status Effect");
        var buffedStats = GetBuffedStats();

        // Ignore if character is immune
        if (buffedStats.Immunities.Contains(code)) return;
        
        var buffIndex = StatusEffects.FindIndex(b => b.Code == code);
        if (buffIndex == -1)
        {
            var buff = RPGGameData.StatusEffects.First(effect => effect.Code == code);
            StatusEffects.Add(buff);
            buffIndex = StatusEffects.Count - 1;
        }
        
        var statusSpan = CollectionsMarshal.AsSpan(StatusEffects);

        statusSpan[buffIndex].StacksApplied = Math.Min(StatusEffects[buffIndex].StacksApplied + numberOfStacks, StatusEffects[buffIndex].StacksMax);
        statusSpan[buffIndex].RoundsRemaining = Math.Max(StatusEffects[buffIndex].RoundsRemaining, numberOfRounds);
    }

    public void DoMovement(float movementAmount)
    {
        Console.WriteLine("Performed movement");
        MutableStats.MovementMade += movementAmount;

        var calculatedStats = GetBuffedStats();

        var newStats = MutableStats;
        foreach (var effect in StatusEffects)
        {
            if (effect.MovementEffect != null)
            {
                newStats = effect.MovementEffect(calculatedStats, newStats, effect.StacksApplied);
            }
        }
        RaiseChanges(MutableStats, newStats);
        MutableStats = newStats;

        MutableStats.MovementAccountedFor = MutableStats.MovementMade;
    }

    private void RaiseChanges(CharacterMutableStats oldStats, CharacterMutableStats newStats)
    {
        if (newStats.Health != oldStats.Health)
        {
            if (HealthChange != null)
            {
                HealthChange.Invoke(newStats.Health - oldStats.Health, newStats.Health);
            }
        }
    }

    public CharacterCalculatedStats GetBuffedStats()
    {
        return StatusEffects.ToList().Aggregate(AttributesToStats(Attributes), (current, buff) => buff.ApplyToStats(current, buff.StacksApplied));
    }

    public void RoundStart()
    {
        Console.WriteLine("Doing Round Start");
        var calculatedStats = GetBuffedStats();
        MutableStats.Health = Math.Min(calculatedStats.HealthMax, MutableStats.Health + calculatedStats.HealthRegen);
        MutableStats.Mana = Math.Min(calculatedStats.ManaMax, MutableStats.Mana + calculatedStats.ManaRegen);
        if (MutableStats.MovementMade == 0)
        {
            MutableStats.RoundsWithoutMoving++;
        }
        MutableStats.ActionPointsRemaining = calculatedStats.RoundActionPoints;

        var newStats = MutableStats;
        foreach (var effect in StatusEffects)
        {
            if (effect.ApplyEffect != null)
            {
                newStats = effect.ApplyEffect(calculatedStats, newStats, effect.StacksApplied);
            }
        }
        RaiseChanges(MutableStats, newStats);
        MutableStats = newStats;
    }

    public void RoundEnd()
    {
        Console.WriteLine("Doing Round End");
        var statusSpan = CollectionsMarshal.AsSpan(StatusEffects);

        for (var effectIndex = 0; effectIndex < statusSpan.Length; effectIndex++)
        {
            statusSpan[effectIndex].RoundsRemaining -= 1;
        }
        
        foreach (var buff in StatusEffects.Where(effect => effect.StacksApplied == 0 || effect.RoundsRemaining == 0).ToList())
        {
            StatusEffects.Remove(buff);
        }
    }
    
    public void RoundTick()
    {
        RoundStart();
        RoundEnd();
    }
}