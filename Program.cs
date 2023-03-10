RPGCharacter rpgCharacter = new ();

rpgCharacter.HealthChange += (by, health) =>
{
    Console.WriteLine($"Health changed by: {by}, new health is {health}");
};

// rpgCharacter.Stats.Resistances.Fire = 5;
// rpgCharacter.Stats.MaxRange = 5;
rpgCharacter.MutableStats.Health = 100;

rpgCharacter.ApplyStatusEffect(StatusEffectCode.Blinded, 1, 3);
rpgCharacter.ApplyStatusEffect(StatusEffectCode.Burning, 1, 2);

Console.Clear();

PrintStats(0);

rpgCharacter.RoundTick();

PrintStats(1);

rpgCharacter.RoundTick();

PrintStats(2);

rpgCharacter.RoundTick();

PrintStats(3);

rpgCharacter.RoundTick();

PrintStats(4);

rpgCharacter.ApplyStatusEffect(StatusEffectCode.Bleeding, 2, 2);

rpgCharacter.RoundTick();

PrintStats(5);

rpgCharacter.DoMovement(2.0f);

PrintStats(5);

rpgCharacter.DoMovement(1.5f);

PrintStats(5);

rpgCharacter.RoundTick();

PrintStats(6);

void PrintStats(int roundNumber)
{
    var newStats = rpgCharacter.GetBuffedStats();

    Console.WriteLine($"Round {roundNumber}");
    Console.WriteLine("-----------------------------");
    Console.WriteLine($"Current Health = {rpgCharacter.MutableStats.Health}");
    // Console.WriteLine($"Real MaxRange = {rpgCharacter.Stats.MaxRange}");
    Console.WriteLine($"Buffed MaxRange = {newStats.MaxRange}");

    // Console.WriteLine($"Real FireResist = {rpgCharacter.Stats.Resistances.Fire}");
    Console.WriteLine($"Buffed FireResist = {newStats.Resistances.Fire}");

    foreach (var effect in rpgCharacter.StatusEffects)
    {
        Console.WriteLine($"Effect: {effect.Name}");    
        Console.WriteLine($"- Stacks: {effect.StacksApplied}/{effect.StacksMax}");    
        Console.WriteLine($"- Rounds: {effect.RoundsRemaining}");    
    }
    Console.WriteLine("-----------------------------");
}
