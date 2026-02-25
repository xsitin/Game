using System.Collections.Generic;

namespace Game.Core.Combat;

public static class SkillCatalog
{
    public static IReadOnlyList<BattleSkill> BaseHit { get; } =
    [
        new BattleSkill("Base Hit", 0, SkillRange.Single, [new BattleEffect(Characteristic.Health, -15)])
    ];

    public static IReadOnlyDictionary<Specialization, IReadOnlyList<BattleSkill>> BasicSkills { get; } =
        new Dictionary<Specialization, IReadOnlyList<BattleSkill>>
        {
            {
                Specialization.Archer,
                [
                    new BattleSkill("Power Shoot", 20, SkillRange.Single, [new BattleEffect(Characteristic.Health, -45)]),
                    new BattleSkill("Power Multi Shoot", 60, SkillRange.Enemies, [new BattleEffect(Characteristic.Health, -30)]),
                    new BattleSkill("Broking arrow", 0, SkillRange.Single, [new BattleEffect(Characteristic.Health, -20)],
                        buff: new BattleBuff("Poison", 2, [new BattleEffect(Characteristic.PhysicalProtection, -15)])),
                    new BattleSkill("Hitting below the belt", 0, SkillRange.Single, [new BattleEffect(Characteristic.Health, -40)])
                ]
            },
            {
                Specialization.Wizard,
                [
                    new BattleSkill("Fire Boll", 20, SkillRange.Single, [new BattleEffect(Characteristic.Health, -30)], isMagic: true,
                        buff: new BattleBuff("Burning", 2, [new BattleEffect(Characteristic.Initiative, -10)])),
                    new BattleSkill("Magical Arrows", 60, SkillRange.Enemies, [new BattleEffect(Characteristic.Health, -30)], isMagic: true),
                    new BattleSkill("Healing hands", 30, SkillRange.Friendly, [new BattleEffect(Characteristic.Health, +30)], isMagic: true),
                    new BattleSkill("Armageddon", 100, SkillRange.All, [new BattleEffect(Characteristic.Health, -90)], isMagic: true)
                ]
            },
            {
                Specialization.Warrior,
                [
                    new BattleSkill("OraOra", 20, SkillRange.Single, [new BattleEffect(Characteristic.Health, -20)],
                        buff: new BattleBuff("Stan", 2, [new BattleEffect(Characteristic.Initiative, -100)])),
                    new BattleSkill("OraTeam", 30, SkillRange.Enemies, [new BattleEffect(Characteristic.Health, -20)]),
                    new BattleSkill("Close the shield", 10, SkillRange.Single, [],
                        buff: new BattleBuff("Shield", 3,
                        [
                            new BattleEffect(Characteristic.PhysicalProtection, +15),
                            new BattleEffect(Characteristic.MagicalProtection, +15)
                        ])),
                    new BattleSkill("Rage", 30, SkillRange.Single,
                        [
                            new BattleEffect(Characteristic.Health, +10),
                            new BattleEffect(Characteristic.PhysicalDamage, +10),
                            new BattleEffect(Characteristic.Evasion, +5)
                        ])
                ]
            }
        };
}
