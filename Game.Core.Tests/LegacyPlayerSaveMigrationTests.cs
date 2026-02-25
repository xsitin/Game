using System.Linq;
using Game.Core.Combat;
using Game.Core.Progression;
using Xunit;

namespace Game.Core.Tests;

public sealed class LegacyPlayerSaveMigrationTests
{
    [Fact]
    public void TryMigrateFromLegacyPlayerJson_PopulatesRosterAndInventory()
    {
        const string json = """
            {
              "Gold": 420,
              "Heroes": [
                {"Name": "Lyur", "Level": 3, "Specialization": 1}
              ],
              "ActiveTeam": {
                "FirstLine": [
                  {"Name": "Lyur", "Level": 3, "Specialization": 1}
                ],
                "SecondLine": []
              },
              "Storage": [
                {
                  "Name": "heal",
                  "Actions": [
                    { "Item1": 0, "Item2": 20 }
                  ],
                  "Buff": {
                    "Name": "Shield"
                  }
                }
              ],
              "Shop": [
                {
                  "Name": "fireball",
                  "Actions": [
                    { "Item1": 1, "Item2": -10 }
                  ]
                }
              ]
            }
            """;

        var migrated = LegacyPlayerSaveMigration.TryMigrateFromLegacyPlayerJson(json, out var state);

        Assert.True(migrated);
        Assert.Equal(420, state.Gold);
        Assert.Contains(state.HeroRoster, hero => hero.Name == "Lyur" && hero.IsActive);
        Assert.Single(state.Inventory);
        Assert.Equal(1, state.Inventory[0].Effects.Count);
        Assert.Equal(20, state.Inventory[0].Effects[Characteristic.Health]);
        Assert.Equal("Shield", state.Inventory[0].BuffName);
        Assert.Single(state.Shop);
        Assert.Equal(-10, state.Shop[0].Effects[Characteristic.Mana]);
    }

    [Fact]
    public void TryMigrateFromLegacyPlayerJson_ReturnsFalseForInvalidPayload()
    {
        var ok = LegacyPlayerSaveMigration.TryMigrateFromLegacyPlayerJson("not-json", out _);
        Assert.False(ok);
    }
}
