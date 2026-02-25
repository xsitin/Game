using System.Collections.Generic;

namespace Game.Core.Combat;

public sealed record CombatantTemplate(
    string Name,
    int MaxHealth,
    int Damage,
    int HealPower,
    bool IsEnemy,
    int PhysicalProtection = 0,
    int MagicalProtection = 0,
    int Evasion = 0,
    int Mana = 100,
    int Initiative = 30,
    Specialization? Specialization = null,
    IReadOnlyList<BattleSkill>? Skills = null)
{
    public Combatant ToCombatant()
    {
        var skills = Skills;
        if (skills is null && Specialization is not null && SkillCatalog.BasicSkills.TryGetValue(Specialization.Value, out var baseSkills))
        {
            skills = baseSkills;
        }

        var combatant = new Combatant(
            Name,
            MaxHealth,
            Damage,
            HealPower,
            IsEnemy,
            PhysicalProtection,
            MagicalProtection,
            Evasion,
            Mana,
            Initiative,
            skills);

        return combatant;
    }
}
