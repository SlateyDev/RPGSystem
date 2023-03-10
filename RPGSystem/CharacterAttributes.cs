public struct CharacterAttributes
{
    public int Level;           // character level
    public int Might;           // damage = might * 1%, armour = might * 1%
    public int Dexterity;       // crit rating = dexterity, crit damage = dexterity * 1.5%, movement spaces = base movement + min(dexterity / 25, 3). need formula for crit rating to crit hit chance
    public int Intellect;       // mana = base mana + intelligence * 5 + level * x, mana regen, resistances, skill range = base range + min(intelligence / 25, 3)
    public int Vitality;        // health = base health + vitality * 5 + level * x, health regen
    public int Reflex;          // counter chance = reflex * 1%, dodge counter chance = reflex * 1%, dodge rating
}