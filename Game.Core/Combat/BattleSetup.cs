using System.Collections.Generic;

namespace Game.Core.Combat;

public sealed class BattleSetup
{
    public BattleSetup(
        IReadOnlyList<CombatantTemplate> heroes,
        IReadOnlyList<CombatantTemplate> enemies,
        int randomSeed = 42)
    {
        Heroes = heroes;
        Enemies = enemies;
        RandomSeed = randomSeed;
    }

    public IReadOnlyList<CombatantTemplate> Heroes { get; }
    public IReadOnlyList<CombatantTemplate> Enemies { get; }
    public int RandomSeed { get; }

    public static BattleSetup Prototype => new(
        heroes:
        [
            new CombatantTemplate("Knight", MaxHealth: 45, Damage: 11, HealPower: 5, IsEnemy: false, PhysicalProtection: 30, MagicalProtection: 10, Evasion: 10, Initiative: 35, Specialization: Specialization.Warrior),
            new CombatantTemplate("Archer", MaxHealth: 32, Damage: 13, HealPower: 4, IsEnemy: false, PhysicalProtection: 15, MagicalProtection: 10, Evasion: 25, Initiative: 45, Specialization: Specialization.Archer),
            new CombatantTemplate("Priest", MaxHealth: 30, Damage: 8, HealPower: 10, IsEnemy: false, PhysicalProtection: 10, MagicalProtection: 25, Evasion: 15, Initiative: 30, Specialization: Specialization.Wizard)
        ],
        enemies:
        [
            new CombatantTemplate("Skeleton", MaxHealth: 28, Damage: 7, HealPower: 0, IsEnemy: true, PhysicalProtection: 25, MagicalProtection: 5, Evasion: 5, Initiative: 25, Specialization: Specialization.Warrior),
            new CombatantTemplate("Necromancer", MaxHealth: 36, Damage: 10, HealPower: 6, IsEnemy: true, PhysicalProtection: 10, MagicalProtection: 25, Evasion: 10, Initiative: 40, Specialization: Specialization.Wizard),
            new CombatantTemplate("Ghoul", MaxHealth: 34, Damage: 9, HealPower: 0, IsEnemy: true, PhysicalProtection: 20, MagicalProtection: 10, Evasion: 20, Initiative: 38, Specialization: Specialization.Archer)
        ]);
}
