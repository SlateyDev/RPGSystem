using System.Runtime.InteropServices;

public class RPGCharacter
{
    public CharacterAttributes Attributes;
    // public CharacterGear Gear[];

    public CharacterMutableStats MutableStats = new();
    public CharacterCalculatedStats Stats = new();
    public List<StatusEffect> StatusEffects = new();

    ///TODO:
    /// Need gear.
    /// Need function to calculate stats based on attributes and gear.

    public RPGCharacter()
    {
        // Just some default values since CalculatedStats are not yet calculated.
        Stats.HealthMax = 100;
    }
    
    public void ApplyStatusEffect(StatusEffectCode code, int numberOfStacks, int numberOfRounds)
    {
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

    public CharacterCalculatedStats GetBuffedStats()
    {
        return StatusEffects.ToList().Aggregate(Stats, (current, buff) => buff.ApplyToStats(current, buff.StacksApplied));
    }

    public void RoundStart()
    {
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

        MutableStats = newStats;
    }

    public void RoundEnd()
    {
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