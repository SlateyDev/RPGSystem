public interface IRPGCharacter
{
    CharacterAttributes Attributes { get; }
    CharacterCalculatedStats Stats { get; }
    List<StatusEffect> StatusEffects { get; }

    // void TakeDamage(DamageInfo);
 
    void RoundStart();
    CharacterCalculatedStats GetBuffedStats();
    void ApplyStatusEffect(StatusEffectCode code, int numberOfStacks, int numberOfRounds);
    void DoMovement(float movementAmount);
    void RoundEnd();
}