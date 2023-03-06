RPGCharacter rpgCharacter = new ();

rpgCharacter.Stats.Resistances.Fire = 5;
rpgCharacter.Stats.MaxRange = 5;
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

void PrintStats(int roundNumber)
{
    var newStats = rpgCharacter.GetBuffedStats();

    Console.WriteLine($"Round {roundNumber}");
    Console.WriteLine("-----------------------------");
    Console.WriteLine($"Current Health = {rpgCharacter.MutableStats.Health}");
    Console.WriteLine($"Real MaxRange = {rpgCharacter.Stats.MaxRange}");
    Console.WriteLine($"Buffed MaxRange = {newStats.MaxRange}");

    Console.WriteLine($"Real FireResist = {rpgCharacter.Stats.Resistances.Fire}");
    Console.WriteLine($"Buffed FireResist = {newStats.Resistances.Fire}");

    foreach (var effect in rpgCharacter.StatusEffects)
    {
        Console.WriteLine($"Effect: {effect.Name}");    
        Console.WriteLine($"- Stacks: {effect.StacksApplied}/{effect.StacksMax}");    
        Console.WriteLine($"- Rounds: {effect.RoundsRemaining}");    
    }
    Console.WriteLine("-----------------------------");
}
