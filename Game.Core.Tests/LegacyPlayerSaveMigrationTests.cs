using Game.Core.Progression;
using Xunit;

namespace Game.Core.Tests;

public sealed class LegacyPlayerSaveMigrationTests
{
    [Fact]
    public void TryMigrateFromLegacyPlayerJson_ExtractsHeroAndActiveTeamLevels()
    {
        const string json = """
            {
              "Heroes": [
                {"Level": 3},
                {"Level": 5}
              ],
              "ActiveTeam": {
                "FirstLine": [{"Level": 2}],
                "SecondLine": [{"Level": 4}]
              }
            }
            """;

        var ok = LegacyPlayerSaveMigration.TryMigrateFromLegacyPlayerJson(json, out var state);

        Assert.True(ok);
        Assert.Equal(1, state.EncounterCounter);
        Assert.Equal([3, 5, 2, 4], state.HeroLevels);
    }

    [Fact]
    public void TryMigrateFromLegacyPlayerJson_ReturnsFalseForInvalidPayload()
    {
        var ok = LegacyPlayerSaveMigration.TryMigrateFromLegacyPlayerJson("not-json", out _);
        Assert.False(ok);
    }
}
