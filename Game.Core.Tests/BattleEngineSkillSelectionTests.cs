using Xunit;
using Game.Core.Combat;

namespace Game.Core.Tests;

public class BattleEngineSkillSelectionTests
{
    [Fact]
    public void GetTargetsForActiveHeroSkill_ReturnsOnlyAliveTargetsForSingleSkill()
    {
        var setup = BattleSetupFactory.CreateProgressiveEncounter([1, 1, 1], encounterCounter: 1, randomSeed: 42);
        var engine = new BattleEngine(setup);

        var skills = engine.GetActiveHeroSkills();
        var singleSkillIndex = skills
            .Select((skill, index) => new { skill, index })
            .First(x => x.skill.Range == SkillRange.Single)
            .index;

        var targets = engine.GetTargetsForActiveHeroSkill(singleSkillIndex);

        Assert.NotEmpty(targets);
        Assert.All(targets, target => Assert.True(target.IsAlive));
    }

    [Fact]
    public void ApplyHeroSkill_WithInvalidIndex_ReturnsValidationMessage()
    {
        var setup = BattleSetupFactory.CreateProgressiveEncounter([1, 1, 1], encounterCounter: 1, randomSeed: 42);
        var engine = new BattleEngine(setup);

        var log = engine.ApplyHeroSkill(999, 0);

        Assert.Contains("Selected skill is unavailable.", log);
    }
}
