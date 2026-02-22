using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Core.Combat;

public static class BattleSetupFactory
{
    private static readonly string[] HeroNames = ["Alden", "Mira", "Tess", "Rurik", "Iris", "Keen"];
    private static readonly string[] EnemyNames = ["Ghast", "Skarn", "Maw", "Vex", "Dusk", "Rattle"];

    public static BattleSetup CreateProgressiveEncounter(
        IReadOnlyList<int> heroLevels,
        int encounterCounter,
        int randomSeed = 42)
    {
        if (heroLevels.Count == 0)
        {
            throw new ArgumentException("At least one hero level is required.", nameof(heroLevels));
        }

        var rng = new Random(randomSeed + encounterCounter);
        var heroes = BuildHeroes(heroLevels, rng);
        var enemies = BuildEnemies(heroLevels, encounterCounter, rng);
        return new BattleSetup(heroes, enemies, randomSeed + encounterCounter);
    }

    private static IReadOnlyList<CombatantTemplate> BuildHeroes(IReadOnlyList<int> heroLevels, Random rng)
    {
        var result = new List<CombatantTemplate>();
        for (var i = 0; i < heroLevels.Count; i++)
        {
            var specialization = (Specialization)(i % 3);
            var level = Math.Max(1, heroLevels[i]);
            result.Add(ScaleTemplate(
                BaseTemplate(HeroNames[i % HeroNames.Length], specialization, isEnemy: false),
                level,
                rng));
        }

        return result;
    }

    private static IReadOnlyList<CombatantTemplate> BuildEnemies(IReadOnlyList<int> heroLevels, int encounterCounter, Random rng)
    {
        var minLevel = Math.Max(1, heroLevels.Min() + encounterCounter / 4);
        var maxLevel = Math.Max(minLevel, heroLevels.Max() + encounterCounter / 4);

        var heroCount = Math.Min(8, heroLevels.Count + encounterCounter);
        var enemyCount = Math.Clamp(encounterCounter, 1, heroCount);

        var result = new List<CombatantTemplate>();
        for (var i = 0; i < enemyCount; i++)
        {
            var specialization = (Specialization)rng.Next(0, 3);
            var level = rng.Next(minLevel, maxLevel + 1);
            result.Add(ScaleTemplate(
                BaseTemplate(EnemyNames[(i + encounterCounter) % EnemyNames.Length], specialization, isEnemy: true),
                level,
                rng));
        }

        return result;
    }

    private static CombatantTemplate BaseTemplate(string name, Specialization specialization, bool isEnemy)
    {
        return specialization switch
        {
            Specialization.Wizard => new CombatantTemplate(name, 30, 8, 10, isEnemy, 10, 25, 15, 100, 30, specialization),
            Specialization.Warrior => new CombatantTemplate(name, 45, 11, 5, isEnemy, 30, 10, 10, 100, 35, specialization),
            _ => new CombatantTemplate(name, 32, 13, 4, isEnemy, 15, 10, 25, 100, 45, specialization)
        };
    }

    private static CombatantTemplate ScaleTemplate(CombatantTemplate baseTemplate, int level, Random rng)
    {
        var current = baseTemplate with { Name = $"{baseTemplate.Name} Lv{level}" };
        for (var i = 1; i < level; i++)
        {
            if (rng.Next(0, 2) == 1)
            {
                current = UpgradeStat(current, (Characteristic)rng.Next(0, Enum.GetValues<Characteristic>().Length));
            }
            else
            {
                current = UpgradeSkill(current, rng);
            }
        }

        return current;
    }

    private static CombatantTemplate UpgradeSkill(CombatantTemplate template, Random rng)
    {
        var skills = (template.Skills ?? SkillCatalog.BasicSkills[template.Specialization!.Value]).ToArray();
        var index = rng.Next(0, skills.Length);
        skills[index].Upgrade();
        return template with { Skills = skills };
    }

    private static CombatantTemplate UpgradeStat(CombatantTemplate template, Characteristic stat)
    {
        return stat switch
        {
            Characteristic.Health => template with { MaxHealth = (int)Math.Round(template.MaxHealth * 1.2) },
            Characteristic.Mana => template with { Mana = (int)Math.Round(template.Mana * 1.2) },
            Characteristic.Initiative => template with { Initiative = (int)Math.Round(template.Initiative * 1.2) },
            Characteristic.PhysicalDamage => template with { Damage = (int)Math.Round(template.Damage * 1.2) },
            Characteristic.PhysicalProtection => template with { PhysicalProtection = Math.Min(95, (int)Math.Round(template.PhysicalProtection * 1.2)) },
            Characteristic.Evasion => template with { Evasion = Math.Min(95, (int)Math.Round(template.Evasion * 1.2)) },
            Characteristic.MagicalProtection => template with { MagicalProtection = Math.Min(95, (int)Math.Round(template.MagicalProtection * 1.2)) },
            _ => template
        };
    }
}
