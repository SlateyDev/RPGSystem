public interface IRPGCharacter
{
    CharacterAttributes Attributes { get; }
    CharacterCalculatedStats Stats { get; }
    List<StatusEffect> StatusEffects { get; }

    void DoRoundEffects();
    // void TakeDamage(DamageInfo);
    CharacterCalculatedStats GetBuffedStats();
    void ClearExpiredBuffs();
}