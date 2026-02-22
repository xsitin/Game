using Xunit;
using Game.Core.Combat;

namespace Game.Core.Tests;

public class BattleSetupFactoryTests
{
    [Fact]
    public void CreateProgressiveEncounter_IsDeterministicForSameSeedAndCounter()
    {
        var heroLevels = new[] { 1, 3, 5 };

        var first = BattleSetupFactory.CreateProgressiveEncounter(heroLevels, encounterCounter: 4, randomSeed: 99);
        var second = BattleSetupFactory.CreateProgressiveEncounter(heroLevels, encounterCounter: 4, randomSeed: 99);

        Assert.Equal(first.Heroes.Count, second.Heroes.Count);
        Assert.Equal(first.Enemies.Count, second.Enemies.Count);

        AssertTeamsEquivalent(first.Heroes, second.Heroes);
        AssertTeamsEquivalent(first.Enemies, second.Enemies);
    }

    [Fact]
    public void CreateProgressiveEncounter_IncreasesEnemyCountWithCounter()
    {
        var heroLevels = new[] { 1, 1, 1 };

        var early = BattleSetupFactory.CreateProgressiveEncounter(heroLevels, encounterCounter: 1, randomSeed: 42);
        var later = BattleSetupFactory.CreateProgressiveEncounter(heroLevels, encounterCounter: 5, randomSeed: 42);

        Assert.True(later.Enemies.Count >= early.Enemies.Count);
    }

    private static void AssertTeamsEquivalent(IReadOnlyList<CombatantTemplate> left, IReadOnlyList<CombatantTemplate> right)
    {
        for (var i = 0; i < left.Count; i++)
        {
            Assert.Equal(left[i].Name, right[i].Name);
            Assert.Equal(left[i].MaxHealth, right[i].MaxHealth);
            Assert.Equal(left[i].Damage, right[i].Damage);
            Assert.Equal(left[i].HealPower, right[i].HealPower);
            Assert.Equal(left[i].PhysicalProtection, right[i].PhysicalProtection);
            Assert.Equal(left[i].MagicalProtection, right[i].MagicalProtection);
            Assert.Equal(left[i].Evasion, right[i].Evasion);
            Assert.Equal(left[i].Mana, right[i].Mana);
            Assert.Equal(left[i].Initiative, right[i].Initiative);
            Assert.Equal(left[i].Specialization, right[i].Specialization);

            var leftSkills = left[i].Skills ?? [];
            var rightSkills = right[i].Skills ?? [];
            Assert.Equal(leftSkills.Count, rightSkills.Count);

            for (var j = 0; j < leftSkills.Count; j++)
            {
                Assert.Equal(leftSkills[j].Name, rightSkills[j].Name);
                Assert.Equal(leftSkills[j].ManaCost, rightSkills[j].ManaCost);
                Assert.Equal(leftSkills[j].Level, rightSkills[j].Level);
            }
        }
    }
}
